namespace Augment.Blueprints;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
class TransformerAttribute : Attribute
{
    public TransformerAttribute(string longName, string shortName)
    {
        LongName = longName;
        ShortName = shortName;
    }

    /// <summary>
    /// 
    /// </summary>
    public string LongName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string ShortName { get; }
}