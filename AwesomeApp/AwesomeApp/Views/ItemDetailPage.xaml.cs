using AwesomeApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AwesomeApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}