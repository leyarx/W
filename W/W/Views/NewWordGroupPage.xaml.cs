using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using W.Models;
using W.ViewModels;

namespace W.Views
{
    public partial class NewWordGroupPage : ContentPage
    {
        public Group Group { get; set; }

        public NewWordGroupPage()
        {
            InitializeComponent();
            BindingContext = new NewWordGroupViewModel();
        }
    }
}