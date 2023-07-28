namespace Augment.Blueprints;

class Blueprint
{
    /// <summary>
    /// 
    /// </summary>
    public Blueprint(string name, string inPath, string outPath)
    {
        Name = name;
        InPath = inPath;
        OutPath = outPath;
    }

    /// <summary>
    /// 
    /// </summary>

    public void Build(Variables variables, bool force)
    {
        Runner.Write(Name.PadRight(64));

        EnsureDirectory();

        Template t = new(File.ReadAllText(InPath), variables);

        if (!File.Exists(OutPath) || force)
        {
            File.WriteAllText(OutPath, t.TransformContents());

            Runner.WriteLine(" ... Built");
        }
        else
        {
            Runner.WriteLine(" ... Skipped");
        }
    }

    private void EnsureDirectory()
    {
        string? dir = Path.GetDirectoryName(OutPath);

        if (dir is not null && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    public string InPath { get; }

    /// <summary>
    /// 
    /// </summary>
    public string OutPath { get; }
}