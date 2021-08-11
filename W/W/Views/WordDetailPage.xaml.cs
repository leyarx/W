using System.ComponentModel;
using Xamarin.Forms;
using W.ViewModels;

namespace W.Views
{
    public partial class WordDetailPage : ContentPage
    {
        public WordDetailPage()
        {
            InitializeComponent();
            BindingContext = new WordDetailViewModel();
        }
    }
}