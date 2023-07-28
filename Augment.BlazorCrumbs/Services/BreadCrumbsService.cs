namespace Augment.BlazorCrumbs.Services;

public record BreadCrumb(string Label, string? Url, string? Filter = null);

public interface IBlazorCrumbService
{
    /// <summary>
    /// 
    /// </summary>
    event EventHandler<BreadCrumb>? Added;

    /// <summary>
    /// 
    /// </summary>
    event EventHandler<BreadCrumb>? Cleared;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    void Clear();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    BlazorCrumbService Root(string label, string url);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    BlazorCrumbService Add(string label, string url);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="filter"></param>
    void Label(string label, string filter);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    void Label(string label);
}

public class BlazorCrumbService : IBlazorCrumbService
{
    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<BreadCrumb>? Added;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<BreadCrumb>? Cleared;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public void Clear()
    {
        BreadCrumb breadCrumb = new("X", null);

        Cleared?.Invoke(this, breadCrumb);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public BlazorCrumbService Root(string label, string url)
    {
        BreadCrumb breadCrumb = new(label, url);

        Cleared?.Invoke(this, breadCrumb);

        Added?.Invoke(this, breadCrumb);

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public BlazorCrumbService Add(string label, string url)
    {
        BreadCrumb breadCrumb = new(label, url);

        Added?.Invoke(this, breadCrumb);

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="filter"></param>
    public void Label(string label, string? filter)
    {
        BreadCrumb breadCrumb = new(label, null, filter);

        Added?.Invoke(this, breadCrumb);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    public void Label(string label)
    {
        BreadCrumb breadCrumb = new(label, null, null);

        Added?.Invoke(this, breadCrumb);
    }
}