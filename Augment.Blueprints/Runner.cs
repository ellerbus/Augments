namespace Augment.Blueprints;

public static class Runner
{
    public static void RunWith(string[] args)
    {
        Options options = new(args);

        Blueprint[] blueprints = GetBlueprints(options).ToArray();

        foreach (Blueprint blueprint in blueprints)
        {
            blueprint.Build(options.Variables, options.Overwrite);
        }
    }

    private static IEnumerable<Blueprint> GetBlueprints(Options options)
    {
        foreach (var blueprint in GetBlueprints(options, options.InPath))
        {
            yield return blueprint;
        }
    }

    private static IEnumerable<Blueprint> GetBlueprints(Options options, string inPath)
    {
        DirectoryInfo di = new(inPath);

        foreach (DirectoryInfo child in di.GetDirectories())
        {
            foreach (Blueprint blueprint in GetBlueprints(options, child.FullName))
            {
                yield return blueprint;
            }
        }

        foreach (FileInfo fi in di.GetFiles("*.bp"))
        {
            string inFile = fi.FullName;

            Template t = new(inFile, options.Variables);

            string outFile = t.TransformContents()
                .Replace(options.InPath, options.OutPath)
                .Replace(".bp", "");

            string name = outFile.Replace(options.OutPath, "");

            yield return new Blueprint(name, inFile, outFile);
        }
    }

    internal static void Write(string msg)
    {
        Console.Write(msg);
    }

    internal static void WriteLine(string msg)
    {
        Console.WriteLine(msg);
    }
}