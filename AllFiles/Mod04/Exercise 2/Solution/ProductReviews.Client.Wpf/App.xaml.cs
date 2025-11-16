using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductReviews.Client.Proxies;
using ProductReviews.Client.Wpf.Utils;
using ProductReviews.Client.Wpf.ViewModels;

namespace ProductReviews.Client.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration? Configuration { get; private set; }
        internal static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional:false, reloadOnChange:true);
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var main = ServiceProvider.GetRequiredService<ProductGroupOverviewWindow>();
            main.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.Configure<RestOptions>(Configuration!.GetSection(nameof(RestOptions)));
            services.Configure<AzureAd>(Configuration!.GetSection(nameof(AzureAd)));
            services.AddSingleton<AzureAdClient>();

            services.AddSingleton<IProductProxy, ProductProxy>();
            services.AddSingleton<IProductGroupProxy, ProductGroupProxy>();
            services.AddSingleton<IBrandProxy, BrandProxy>();
            services.AddSingleton<IReviewProxy, ReviewProxy>();
            services.AddSingleton<IViewContainer, ViewContainer>();

            services.AddTransient<ProductGroupOverviewViewModel>();
            services.AddTransient<ProductGroupOverviewWindow>();

            services.AddTransient<ProductGroupDetailViewModel>();
            services.AddTransient<ProductGroupDetailWindow>();
        }
    }
}
