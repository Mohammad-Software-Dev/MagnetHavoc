using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: CSharpSyntaxCheck <directory>");
    return 2;
}

string root = Path.GetFullPath(args[0]);
if (!Directory.Exists(root))
{
    Console.Error.WriteLine($"Directory does not exist: {root}");
    return 2;
}

string[] files = Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories)
    .OrderBy(path => path, StringComparer.Ordinal)
    .ToArray();

int errorCount = 0;
foreach (string file in files)
{
    string text = File.ReadAllText(file);
    SyntaxTree tree = CSharpSyntaxTree.ParseText(
        text,
        new CSharpParseOptions(LanguageVersion.Latest),
        file);

    foreach (Diagnostic diagnostic in tree.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error))
    {
        errorCount++;
        Console.Error.WriteLine(diagnostic.ToString());
    }
}

if (errorCount > 0)
{
    Console.Error.WriteLine($"C# syntax validation failed with {errorCount} error(s) across {files.Length} files.");
    return 1;
}

Console.WriteLine($"C# syntax validation passed for {files.Length} files.");
return 0;
