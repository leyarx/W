using System.ComponentModel;
using Xamarin.Forms;
using W.ViewModels;

namespace W.Views
{
    public partial class WordStudyPage : ContentPage
    {
        public WordStudyPage()
        {
            InitializeComponent();
            BindingContext = new WordStudyViewModel();
        }
    }
}