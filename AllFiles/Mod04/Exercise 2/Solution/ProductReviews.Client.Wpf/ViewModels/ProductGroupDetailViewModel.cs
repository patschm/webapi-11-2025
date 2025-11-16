using System;
using System.Collections.Generic;
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
    public class ProductGroupDetailViewModel: BaseViewModel
    {
        private readonly IProductGroupProxy _proxy;
        private readonly AzureAdClient _adClient;
        private ProductGroup? _productGroup = new ProductGroup();
        private bool _canSave = false;
        public bool CanSave
        {
            get { return _canSave; }
            set 
            { 
                _canSave = value; 
                NotifyChanged();
            }
        }
        
        public string? Name
        {
            get { return _productGroup!.Name; }
            set 
            { 
                _productGroup!.Name = value; 
                CanSave = true;
                NotifyChanged();
            }
        }   
         public string? Image
        {
            get { return _productGroup!.Image; }
            set 
            { 
                _productGroup!.Image = value;
                CanSave = true;
                NotifyChanged();
            }
        }   
        
        public ICommand LoadGroup { get; }
        public ICommand Save { get; }
        public ICommand Close { get; }
        
        public ProductGroupDetailViewModel(IProductGroupProxy proxy, AzureAdClient adClient)
        {
            _proxy = proxy;
            _adClient = adClient;
            LoadGroup=new RelayCommand(LoadGroupAsync);
            Save = new RelayCommand(SaveAsync);
            Close = new RelayCommand(CloseAsync);
        }

        private Task CloseAsync(object arg)
        {
            if(arg is ProductGroupDetailWindow)
            {
                var win = arg as ProductGroupDetailWindow;
                win!.DialogResult = true;
                
            }
            return Task.CompletedTask;
        }

        private async Task SaveAsync(object arg)
        {
            var scopes = new string[]{ApiAccessScopes.Read, ApiAccessScopes.Write};
            try
            {
                var token = await _adClient.GetAccessTokenForAsync(scopes);
                if (_productGroup!.Id > 0)
                {
                    _productGroup = await _proxy.WithBearer(token).PutAsync(_productGroup.Id, _productGroup);
                }
                else
                {
                    _productGroup = await _proxy.WithBearer(token).PostAsync(_productGroup);
                }
                CanSave = false;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadGroupAsync(object arg)
        {
            _productGroup = arg as ProductGroup;
            if (_productGroup == null)
            {
                _productGroup = new ProductGroup();
                return;
            }
            Name = _productGroup.Name;
            Image = _productGroup.Image;
            
            var scopes = new string[]{ApiAccessScopes.Read, ApiAccessScopes.Write};
            try
            {
                var token = await _adClient.GetAccessTokenForAsync(scopes);
                _productGroup = await _proxy.WithBearer(token).GetByIdAsync(_productGroup.Id);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}