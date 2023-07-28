namespace Augment.Blueprints;

class Variables : Dictionary<string, string>
{
    public Variables() : base(StringComparer.OrdinalIgnoreCase) { }
}