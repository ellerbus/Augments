namespace Augment.DependencySidekick;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class TransientDependencyAttribute : Attribute
{
    public TransientDependencyAttribute() { }

    public TransientDependencyAttribute(Type interfaceType)
    {
        InterfaceType = interfaceType;
    }

    public Type? InterfaceType { get; private set; }
}