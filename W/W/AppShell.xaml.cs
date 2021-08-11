using System;
using System.Collections.Generic;
using W.Views;
using Xamarin.Forms;

namespace W
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(WordGroupDetailPage), typeof(WordGroupDetailPage));
            Routing.RegisterRoute(nameof(WordDetailPage), typeof(WordDetailPage));
            Routing.RegisterRoute(nameof(WordStudyPage), typeof(WordStudyPage));
            Routing.RegisterRoute(nameof(NewWordGroupPage), typeof(NewWordGroupPage));
        }

    }
}
