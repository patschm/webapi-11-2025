using ProductReviews.Client.Wpf.ViewModels;
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

namespace ProductReviews.Client.Wpf
{
    /// <summary>
    /// Interaction logic for ProductGroupDetalView.xaml
    /// </summary>
    public partial class ProductGroupDetailWindow : Window
    {
        private readonly ProductGroupDetailViewModel _model;
        public ProductGroupDetailWindow(ProductGroupDetailViewModel model)
        {
            _model = model;
            InitializeComponent(); 
            DataContext = _model;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _model.LoadGroup.Execute(Tag);
        }
    }
}
