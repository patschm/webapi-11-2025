using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProductReviews.Client.Wpf.Utils;

namespace ProductReviews.Client.Wpf;

public class ViewContainer : IViewContainer
{
    public Window ProductGroupDetail 
    {
        get
        {
            return  App.ServiceProvider!.GetRequiredService<ProductGroupDetailWindow>();
        }
    }
}