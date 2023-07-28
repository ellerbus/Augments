namespace Augment.MoqBuddy;

public class Mockery
{
    private Dictionary<Type, object> _mocks = new();

    public Mockery()
    {
        DefaultBehavior = MockBehavior.Strict;
    }

    public Mock<T> GetMock<T>() where T : class
    {
        return GetMock<T>(DefaultBehavior);
    }

    public Mock<T> GetMock<T>(MockBehavior mockBehavior) where T : class
    {
        if (!_mocks.TryGetValue(typeof(T), out object? mock))
        {
            mock = new Mock<T>(DefaultBehavior);

            _mocks.Add(typeof(T), mock);
        }

        return (Mock<T>)mock;
    }

    public void VerifyAll()
    {
        foreach (var mock in _mocks)
        {
            Mock? mocked = mock.Value as Mock;

            mocked?.VerifyAll();
        }
    }

    public T CreateInstance<T>() where T : class
    {
        return (CreateInstance(typeof(T)) as T)!;
    }

    private object CreateInstance(Type type)
    {
        ConstructorInfo[] constructors = type.GetConstructors();

        if (constructors.Length > 1)
        {
            string msg = $"{type.FullName} has more than one constructor";

            throw new InvalidOperationException(msg);
        }

        ConstructorInfo constructor = constructors.Single();

        List<object> parameters = new();

        foreach (ParameterInfo pi in constructor.GetParameters())
        {
            object obj = GetParameterObject(pi.ParameterType);

            parameters.Add(obj);
        }

        return constructor.Invoke(parameters.ToArray());
    }

    private object GetParameterObject(Type type)
    {
        if (!type.IsInterface)
        {
            return CreateInstance(type);
        }

        if (_mocks.TryGetValue(type, out object? obj))
        {
            if (obj is Mock mocked)
            {
                return mocked.Object;
            }

            return obj;
        }

        Mock mock = (Mock)MakeMock(type);

        _mocks.Add(type, mock);

        return mock.Object;
    }

    private object MakeMock(Type interfaceType)
    {
        Type generic = typeof(Mock<>);

        Type[] genericArgTypes = new[] { interfaceType };

        Type mockType = generic.MakeGenericType(genericArgTypes);

        Type[] argTypes = new[] { typeof(MockBehavior) };

        object[] argValues = new[] { (object)DefaultBehavior };

        ConstructorInfo constructor = mockType.GetConstructor(argTypes)!;

        return constructor.Invoke(argValues);
    }

    public MockBehavior DefaultBehavior { get; }
}