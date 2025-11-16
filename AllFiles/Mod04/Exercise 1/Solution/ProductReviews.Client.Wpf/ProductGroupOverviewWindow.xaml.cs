using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProductReviews.Client.Wpf.Utils;
using ProductReviews.Client.Wpf.ViewModels;

namespace ProductReviews.Client.Wpf
{
    /// <summary>
    /// Interaction logic for ProductGroupOverviewWindow.xaml
    /// </summary>
    public partial class ProductGroupOverviewWindow : Window
    {
       private readonly IViewContainer _views;
        public ProductGroupOverviewWindow(ProductGroupOverviewViewModel model, IViewContainer views)
        {
            _views = views;
            InitializeComponent();
            DataContext = model;
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();            
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            var detail = _views.ProductGroupDetail;
            detail.Tag = (sender as Button)!.Tag;
            detail.Owner = this;
            if(detail.ShowDialog() ?? false)
            {
                Reload();
            }
        }
        private void Reload()
        {
            (DataContext as ProductGroupOverviewViewModel)?.LoadGroups.Execute(new object());
        }
    }
}
