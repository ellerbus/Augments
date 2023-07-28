namespace Augment.BlazorCrumbs;

public partial class BreadCrumbNavigation : IDisposable
{
    [CascadingParameter]
    public BlazorCrumbService BlazorCrumbService { get; set; } = null!;

    public IList<BreadCrumb> BlazorCrumbs { get; } = new List<BreadCrumb>();

    protected override void OnInitialized()
    {
        BlazorCrumbService.Added += BlazorCrumbService_Added;
        BlazorCrumbService.Cleared += BlazorCrumbService_Clear;
    }

    private void BlazorCrumbService_Clear(object? sender, BreadCrumb e)
    {
        BlazorCrumbs.Clear();

        StateHasChanged();
    }

    private void BlazorCrumbService_Added(object? sender, BreadCrumb e)
    {
        BlazorCrumbs.Add(e);

        StateHasChanged();
    }

    public virtual void Dispose()
    {
        BlazorCrumbService.Added -= BlazorCrumbService_Added;
        BlazorCrumbService.Cleared -= BlazorCrumbService_Clear;

        GC.SuppressFinalize(this);
    }
}
