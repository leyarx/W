using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using W.Models;
using W.Views;
using W.ViewModels;

namespace W.Views
{
    public partial class WordGroupDetailPage : ContentPage
    {
        WordGroupDetailViewModel _viewModel;

        public WordGroupDetailPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new WordGroupDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}