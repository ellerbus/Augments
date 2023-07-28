# Augments

Various augments that are custom to a specific world of development intended to be used as a submodule (perish the thought).

```
# clone the parent
git clone --recursive <project-url>
```

## Augment.BlazorCrumbs

Provides a simple component to create easy bread crumbs for Blazor applications using Bootstrap v5.

```html
<CascadingBlazorCrumbService>

    <div class="container-fluid min-vh-100 d-flex flex-nowrap m-0 p-0">
        <div class="container p-3">
            <div class="row">
                <BreadCrumbNavigation />
            </div>
            <ChildContent>
                @Body
            </ChildContent>
        </div>
    </div>

</CascadingBlazorCrumbService>
```

```cs
//  component code behind
    protected override async Task OnParametersSetAsync()
    {
        //  ... code

        BlazorCrumbService
            .Root("Home", "/")
            .Add("Users", "/Users")
            .Label("Update", "Kal El");

        //  ... more code
    }

    [CascadingParameter]
    private IBlazorCrumbService BlazorCrumbService { get; set; } = default!;

```

## Augment.Blueprints

A simple scaffolding engine that does nothing more than transform names with the following options:

* `--plural` or `-pl` convert term to it's plural version (English only) 
* `--singular` or `-s` convert term to it's singular version (English only) 
* `--pascal` or `-p` convert term to a PascalCase 
* `--camel` or `-c` convert term to a camelCase 
* `--title` or `-t` convert term to a Titlecase 
* `--label` or `-l` convert term to a Title Case 
* `--abbr` or `-a` convert term to an abbreviation (ie) camelCase would be cc


```cs
//  command line style options
string[] args = new[]
{
    "--force",      //  force the overwrite of existing target files
    "--in",
    @"c:\path\bluprints",
    "--out",
    @"c:\path\output",
    "--vars",
    "Project=CoolProject",
    "Model=user"
}; 

Runner.RunWith(args);
```

Sample folder structure input and output

```
c:\path\blueprints\{{ Project }}.UseCases\{{ Model --plural --pascal }}.cs.bp

c:\path\output\CoolProject.UseCases\Users.cs
```

```cs
//  file sample -- input
namespace {{ Project }}.UseCases;

public class {{ Model --plural --pascal }}
{
    public {{ Model --plural --pascal }}()
    {
    }
    
    public {{ Model --pascal }} Create()
    {
        {{ Model --pascal }} {{ Model --abbr }} = new();

        return {{ Model --abbr }};
    }
}

//  file sample -- output
namespace CoolProject.UseCases;

public class Users
{
    public Users()
    {
    }
    
    public User Create()
    {
        User u = new();

        return u;
    }
}
```

## Augment.DependencySidekick

A simple sidekick library to discover services to be wired into Microsoft's Dependency Injection
eco system.

```cs
Assembly assemply = GetAssembly();

services.AddUsingDependencySidekick(assembly);
```

```cs
public interface IService { }

[TransientDependencyAttribute(typeof(IService))]
public class MyService : IService { }
```

## Augment.MoqBuddy

A simple wrapper, for better or worse, to Moq to save a few lines of code per unit test.

```cs
[TestClass]
public class SomeTests
{
    protected Mockery Mockery = default!;
    protected TestSubject Subject = default!;

    [TestInitialize]
    public virtual void Initialize()
    {
        Mocks = new Mockery();

        Subject = Mocks.CreateInstance<TestSubject>();
    }

    [TestMethod]
    public async Task Handle_Should_Talk_To_IActor_And_IService()
    {
        //  arrange
        Mocks.GetMock<IActor>()
            .Setup(x => x.UserId)
            .Returns(actorId);

        Mocks.GetMock<IService>()
            .Setup(x => x.CreateAsync(cmd))
            .Returns(Task.CompletedTask)
            .Callback((ObjectToInspect o, CancellationToken _) =>
            {
                o.Id.Display.Should().StartWith("X");
            });

        Command cmd = new("X");

        //  act
        var results = await Subject.Handle(cmd, CancellationToken.None);

        //  assert

        Mocks.VerifyAll();
    }
}
```