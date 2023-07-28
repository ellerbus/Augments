namespace Augment.Blueprints;

class Replacement
{
    public Replacement(string original, IDictionary<string, string> variables)
    {
        Original = original;

        Results = TransformResults(original, variables);
    }

    private static string TransformResults(string original, IDictionary<string, string> variables)
    {
        List<string> parts = GetParts(original).ToList();

        string results = variables[parts[0]];

        for (int i = 1; i < parts.Count; i++)
        {
            string transformer = parts[i];

            MethodInfo transform = Transformers.Transformations[transformer];

            object[] parameters = new object[] { results };

            object? value = transform.Invoke(null, parameters);

            if (value is string s)
            {
                results = s;
            }
        }

        return results;
    }

    private static IEnumerable<string> GetParts(string original)
    {
        string pattern = @"(?<item>(--?)?[0-9A-Za-z]+)";

        MatchCollection matches = Regex.Matches(original, pattern);

        foreach (Match match in matches.Cast<Match>())
        {
            if (match.Groups["item"].Success)
            {
                yield return match.Groups["item"].Value;
            }
        }

    }

    public string Original { get; }

    public string Results { get; }
}