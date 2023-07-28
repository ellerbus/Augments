namespace Augment.Blueprints;

static class Transformers
{
    public static readonly Dictionary<string, MethodInfo> Transformations = GetTransformations();

    private static Dictionary<string, MethodInfo> GetTransformations()
    {
        Dictionary<string, MethodInfo> functions = new(StringComparer.OrdinalIgnoreCase);

        Type type = typeof(Transformers);

        MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

        foreach (MethodInfo method in methods)
        {
            TransformerAttribute? attribute = method.GetCustomAttribute<TransformerAttribute>();

            if (attribute is not null)
            {
                functions.Add(attribute.LongName, method);
                functions.Add(attribute.ShortName, method);
            }
        }

        return functions;
    }

    private static readonly IPluralize _pluralizer = new Pluralizer();

    private static readonly RegexOptions _options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled;

    private static readonly Regex _uc = new("^[A-Z0-9_]+$", _options);

    private static readonly Regex _lc = new("^[a-z0-9_]+$", _options);

    private static readonly Regex _uclcSplit = new(
        @"  (?<=[A-Z])(?=[A-Z][a-z])        #   uc before, uc lc after
        |   (?<=[^A-Z])(?=[A-Z])            #   no uc before, uc after
        |   (?<=[A-Za-z])(?=[^A-Za-z])      #   letter before but not after
            ",
        _options
    );

    /// <summary>
    /// Makes a word plural
    /// </summary>
    [Transformer("--plural", "-pl")]
    public static string Plural(string input)
    {
        return _pluralizer.Pluralize(input);
    }

    /// <summary>
    /// Makes a word singular
    /// </summary>
    [Transformer("--singular", "-s")]
    public static string Singular(string input)
    {
        return _pluralizer.Singularize(input);
    }

    /// <summary>
    /// Make the input an Abbreviation (created_by = cb)
    /// </summary>
    [Transformer("--abbr", "-a")]
    public static string Abbr(string input)
    {
        StringBuilder sb = new();

        foreach (string x in Split(input))
        {
            sb.Append(char.ToLower(x[0]));
        }

        return sb.ToString().ToLower();
    }

    /// <summary>
    /// Make the input Pascal cased (created_by = CreatedBy)
    /// </summary>
    [Transformer("--pascal", "-p")]
    public static string Pascal(string input)
    {
        StringBuilder sb = new();

        foreach (string x in Split(input))
        {
            sb.Append(Title(x));
        }

        return sb.ToString().ToLower();
    }

    /// <summary>
    /// Make the input Camel cased (created_by = createdBy)
    /// </summary>
    [Transformer("--camel", "-c")]
    public static string Camel(string input)
    {
        if (input.Length == 1)
        {
            return input.ToLower();
        }

        string p = Pascal(input);

        return p[0..1].ToLower() + p[1..];
    }

    /// <summary>
    /// Make the input Label cased (created_by = Created By)
    /// </summary>
    [Transformer("--label", "-l")]
    public static string Label(string input)
    {
        StringBuilder sb = new();

        foreach (string x in Split(input))
        {
            sb.AppendDelimiter().Append(Title(x));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Make the input Title cased (CreatedBy = Createdby)
    /// </summary>
    [Transformer("--title", "-t")]
    public static string Title(string input)
    {
        if (input.Length == 1)
        {
            return input.ToUpper();
        }

        return input[0..1].ToUpper() + input[1..].ToLower();
    }

    /// <summary>
    /// Get the last word (created_by = by)
    /// </summary>
    [Transformer("--last", "-la")]
    public static string Last(string input)
    {
        return Split(input).Last();
    }

    /// <summary>
    /// Get the first word (created_by = created)
    /// </summary>
    [Transformer("--first", "-f")]
    public static string First(string input)
    {
        return Split(input).First();
    }

    private static IEnumerable<string> Split(string input)
    {
        if (input is (null or ""))
        {
            return Array.Empty<string>();
        }

        if (input.Length == 1)
        {
            return new[] { input };
        }

        if (input.Contains('_'))
        {
            return input.Split('_');
        }

        return _uclcSplit.Split(input);
    }
}