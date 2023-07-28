namespace Augment.Blueprints;

static class Extensions
{
    public static StringBuilder AppendDelimiter(this StringBuilder builder, string delimiter = " ")
    {
        if (builder.Length > 0)
        {
            return builder.Append(delimiter);
        }

        return builder;
    }
}