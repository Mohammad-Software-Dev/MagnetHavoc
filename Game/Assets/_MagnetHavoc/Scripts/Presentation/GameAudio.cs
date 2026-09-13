using UnityEngine;

namespace MagnetHavoc
{
    public sealed class GameAudio : MonoBehaviour
    {
        public static GameAudio Instance { get; private set; }
        private AudioSource _source;
        private AudioClip _push;
        private AudioClip _dash;
        private AudioClip _pickup;
        private AudioClip _knockout;
        private AudioClip _win;

        private void Awake()
        {
            Instance = this;
            _source = gameObject.AddComponent<AudioSource>();
            _source.spatialBlend = 0f;
            _push = Tone("push", 120f, 0.08f, 0.25f);
            _dash = Tone("dash", 260f, 0.06f, 0.17f);
            _pickup = Tone("pickup", 620f, 0.08f, 0.15f);
            _knockout = Tone("ko", 90f, 0.14f, 0.25f);
            _win = Tone("win", 880f, 0.28f, 0.15f);
        }

        public void PlayPush(Vector3 _) => _source.PlayOneShot(_push);
        public void PlayDash(Vector3 _) => _source.PlayOneShot(_dash);
        public void PlayPickup(Vector3 _) => _source.PlayOneShot(_pickup);
        public void PlayKnockout(Vector3 _) => _source.PlayOneShot(_knockout);
        public void PlayWin(Vector3 _) => _source.PlayOneShot(_win);

        private static AudioClip Tone(string clipName, float frequency, float duration, float volume)
        {
            int sampleRate = 22050;
            int count = Mathf.Max(1, Mathf.CeilToInt(sampleRate * duration));
            AudioClip clip = AudioClip.Create(clipName, count, 1, sampleRate, false);
            float[] samples = new float[count];
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)sampleRate;
                float envelope = 1f - i / (float)count;
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * volume;
            }
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
