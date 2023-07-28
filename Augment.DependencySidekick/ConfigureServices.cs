namespace Augment.DependencySidekick;

public static class ConfigureServices
{
    public static void AddUsingDependencySidekick(this IServiceCollection services, Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            foreach (Attribute attribute in type.GetCustomAttributes())
            {
                if (attribute is TransientDependencyAttribute tda)
                {
                    if (tda.InterfaceType is null)
                    {
                        services.AddTransient(type);
                    }
                    else
                    {
                        services.AddTransient(tda.InterfaceType, type);
                    }
                }
            }
        }

    }
}
