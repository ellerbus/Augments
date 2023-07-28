namespace Augment.Blueprints;

partial class Options
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public Options(string[] args)
    {
        InPath = GetAbsolutePath(GetRequiredOption("--in", args));
        OutPath = GetAbsolutePath(GetRequiredOption("--out", args));
        Overwrite = GetFlagOption("--force", args);
        Variables = GetVariables(args);
    }

    private static Variables GetVariables(string[] args)
    {
        Variables vars = new();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--vars" || args[i] == "-v")
            {
                for (int j = i + 1; j < args.Length; j++)
                {
                    string[] data = args[j].Split('=');

                    if (data[1].StartsWith("{{"))
                    {
                        Template t = new(data[1], vars);

                        data[1] = t.TransformContents();
                    }

                    vars.Add(data[0], data[1]);
                }

                break;
            }
        }

        if (vars.Count == 0)
        {
            string msg = $"Missing Required Option --vars Name=Value Name=Value";

            throw new NotImplementedException(msg);
        }

        return vars;
    }

    private static string GetAbsolutePath(string path)
    {
        Uri uri = new(path);

        return Path.GetFullPath(uri.LocalPath);
    }

    private static string GetRequiredOption(string option, string[] args)
    {
        string? value = GetOption(option, args);

        if (value is null)
        {
            string msg = $"Missing Required Option --{option}";

            throw new NotImplementedException(msg);
        }

        return value;
    }

    private static bool GetFlagOption(string option, string[] args)
    {
        string shortOption = GetShortOption(option);

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == option || args[i] == shortOption)
            {
                return true;
            }
        }

        return false;
    }

    private static string? GetOption(string option, string[] args)
    {
        string shortOption = GetShortOption(option);

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == option || args[i] == shortOption)
            {
                return args[i + 1];
            }
        }

        return null;
    }

    private static string GetShortOption(string option)
    {
        foreach (char c in option)
        {
            if (char.IsLetter(c))
            {
                return "-" + c;
            }
        }

        throw new InvalidOperationException();
    }

    /// <summary>
    /// 
    /// </summary>
    public string InPath { get; }

    /// <summary>
    /// 
    /// </summary>
    public string OutPath { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Overwrite { get; }

    /// <summary>
    /// 
    /// </summary>
    public Variables Variables { get; }
}