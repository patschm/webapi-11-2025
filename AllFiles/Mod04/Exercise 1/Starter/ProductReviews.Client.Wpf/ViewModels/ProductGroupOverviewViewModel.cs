using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProductReviews.Client.Proxies;
using ProductReviews.Client.Utils;
using ProductReviews.Client.Wpf.Utils;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Client.Wpf.ViewModels
{
    
    public class ProductGroupOverviewViewModel: BaseViewModel
    {
        private ObservableCollection<ProductGroup> _productGroups = new ObservableCollection<ProductGroup>();
        private readonly IProductGroupProxy _proxy;
        private readonly AzureAdClient _adClient;
        private int _page = 1;
        private const int count = 10;
       
       private bool _nextEnabled = true;
       private bool _prevEnabled = false;

        public ObservableCollection<ProductGroup> ProductGroups
        {
            get
            {
                return _productGroups;
            }
        }
       public bool PreviousEnabled
       {
           get { return _prevEnabled; }
           set 
           { 
               _prevEnabled = value; 
               NotifyChanged();
            }
       }      
       public bool NextEnabled
       {
           get { return _nextEnabled; }
           set
            {
                _nextEnabled = value;
               NotifyChanged();
            }
       }
        public int Page
        {
            get { return _page; }
            set 
            { 
                _page = value; 
                NotifyChanged();
            }
        }
        
        public ICommand LoadGroups { get; }
        public ICommand Next {get; }
        public ICommand Previous {get; }
        

        public ProductGroupOverviewViewModel(IProductGroupProxy proxy, AzureAdClient adClient)
        {
            _proxy = proxy;
            _adClient = adClient;
            LoadGroups = new RelayCommand(LoadProductGroupsAsync);
            Next = new RelayCommand(NextPageAsync);
            Previous = new RelayCommand(PreviousCommandAsync);
        }

        private async Task PreviousCommandAsync(object obj)
        {
            Page--;
            await LoadProductGroupsAsync(null);
            PreviousEnabled =  Page > 1;
            NextEnabled = _productGroups.Count == count;
            if (Page < 1)
            {
                Page = 1;
            }
        }

        private async Task NextPageAsync(object obj)
        {
            Page++;
            await LoadProductGroupsAsync(null);
            NextEnabled = _productGroups.Count == count;
            PreviousEnabled = Page > 1;
        }

        private async Task LoadProductGroupsAsync(object? obj)
        {
            
        }
    }
}