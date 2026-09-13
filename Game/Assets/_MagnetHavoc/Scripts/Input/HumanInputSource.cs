using UnityEngine;

namespace MagnetHavoc
{
    public sealed class HumanInputSource : MonoBehaviour, IPlayerInputSource
    {
        private Vector2 _touchMove;
        private int _moveFinger = -1;
        private Vector2 _moveOrigin;
        private int _magnetFinger = -1;
        private float _magnetDownTime;
        private bool _pushQueued;
        private bool _dashQueued;
        private bool _pullHeld;

        public PlayerCommand ReadCommand()
        {
            UpdateTouchState();

            Vector2 keyboard = Vector2.zero;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) keyboard.x -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) keyboard.x += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) keyboard.y -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) keyboard.y += 1f;
            keyboard = Vector2.ClampMagnitude(keyboard, 1f);

            bool push = _pushQueued || Input.GetMouseButtonDown(0);
            bool dash = _dashQueued || Input.GetKeyDown(KeyCode.Space);
            bool pull = _pullHeld || Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift);
            Vector2 move = keyboard.sqrMagnitude > 0.01f ? keyboard : _touchMove;

            _pushQueued = false;
            _dashQueued = false;

            return new PlayerCommand
            {
                Move = move,
                DashPressed = dash,
                PushPressed = push,
                PullHeld = pull
            };
        }

        private void UpdateTouchState()
        {
            _touchMove = Vector2.zero;
            _pullHeld = false;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 normalized = new Vector2(touch.position.x / Screen.width, touch.position.y / Screen.height);

                if (touch.phase == TouchPhase.Began)
                {
                    if (normalized.x < 0.5f && _moveFinger < 0)
                    {
                        _moveFinger = touch.fingerId;
                        _moveOrigin = touch.position;
                    }
                    else if (IsInside(normalized, new Vector2(0.84f, 0.2f), 0.13f) && _magnetFinger < 0)
                    {
                        _magnetFinger = touch.fingerId;
                        _magnetDownTime = Time.unscaledTime;
                    }
                    else if (IsInside(normalized, new Vector2(0.64f, 0.17f), 0.085f))
                    {
                        _dashQueued = true;
                    }
                }

                if (touch.fingerId == _moveFinger)
                {
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        _moveFinger = -1;
                    }
                    else
                    {
                        Vector2 delta = (touch.position - _moveOrigin) / (Mathf.Min(Screen.width, Screen.height) * 0.16f);
                        _touchMove = Vector2.ClampMagnitude(delta, 1f);
                    }
                }

                if (touch.fingerId == _magnetFinger)
                {
                    float heldFor = Time.unscaledTime - _magnetDownTime;
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (heldFor <= RuntimeContext.Tuning.MagnetHoldThreshold) _pushQueued = true;
                        _magnetFinger = -1;
                    }
                    else if (heldFor > RuntimeContext.Tuning.MagnetHoldThreshold)
                    {
                        _pullHeld = true;
                    }
                }
            }

            if (_moveFinger < 0) _touchMove = Vector2.zero;
            if (_magnetFinger < 0) _pullHeld = false;
        }

        private static bool IsInside(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).sqrMagnitude <= radius * radius;
        }
    }
}
