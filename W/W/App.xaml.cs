using System;
using W.Data;
using W.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace W
{
    public partial class App : Application
    {
        private static VocabularyDatabase vocabularyDatabase;

        public static VocabularyDatabase VocabularyDatabase
        {
            get
            {
                if (vocabularyDatabase == null)
                {
                    vocabularyDatabase = new VocabularyDatabase();
                }
                return vocabularyDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            //MainPage = new MainPage();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
