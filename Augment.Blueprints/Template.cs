namespace Augment.Blueprints;

class Template
{
    private readonly StringBuilder _contents;

    public Template(string contents, Variables variables)
    {
        _contents = new(contents);

        FindReplacements(contents, variables);
    }

    public string TransformContents()
    {
        foreach (var replacement in Replacements.Default.Values)
        {
            _contents.Replace(replacement.Original, replacement.Results);
        }

        return _contents.ToString();
    }

    private static void FindReplacements(string contents, Variables variables)
    {
        string pattern = @"(?<name>\{\{\s*(.*?)\s*\}\})";

        MatchCollection matches = Regex.Matches(contents, pattern);

        foreach (Match match in matches.Cast<Match>())
        {
            string item = match.Groups["name"].Value;

            if (!Replacements.Default.ContainsKey(item))
            {
                Replacements.Default.Add(item, new Replacement(item, variables));
            }
        }
    }
}