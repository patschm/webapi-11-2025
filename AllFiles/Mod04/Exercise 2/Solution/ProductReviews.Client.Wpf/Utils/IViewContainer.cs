using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProductReviews.Client.Wpf.Utils
{
    public interface IViewContainer
    {
        Window ProductGroupDetail { get; }
    }
}