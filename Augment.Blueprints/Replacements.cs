namespace Augment.Blueprints;

class Replacements : Dictionary<string, Replacement>
{
    public static readonly Replacements Default = new();

    private Replacements() : base(StringComparer.OrdinalIgnoreCase) { }
}