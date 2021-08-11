using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using W.Models;
using W.Views;
using Xamarin.Forms;

namespace W.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class WordGroupDetailViewModel : BaseViewModel
    {
        private VocabularyItem _selectedWord;

        public Command<VocabularyItem> WordTapped { get; }

        public ObservableCollection<VocabularyItem> Words {
            get { return App.VocabularyDatabase.CurrentWords; }
        }
        public Command LoadWordsCommand { get; }
        public Command RepeatCommand { get; }
        public Command StudyCommand { get; }

        private int itemId = 0;
        public string ItemId
        {
            get
            {
                return itemId.ToString();
            }
            set
            {
                Int32.TryParse(value, out itemId);
                //itemId = value;
                //LoadItemId(value);
            }
        }

        public WordGroupDetailViewModel()
        {
            Title = "Words";
            App.VocabularyDatabase.CurrentWords = new ObservableCollection<VocabularyItem>();
            LoadWordsCommand = new Command(async () => await ExecuteLoadWordsCommand());
            WordTapped = new Command<VocabularyItem>(OnWordSelected);
            RepeatCommand = new Command(RepeatButton);
            StudyCommand = new Command(StudyButton);
        }

        async Task ExecuteLoadWordsCommand()
        {
            IsBusy = true;

            try
            {
                Words.Clear();
                //var items = await DataStore.GetItemsByGroupIdAsync(true);
                // rewrite this part
                var items = await App.VocabularyDatabase.GetWordsByGroupIdAsync(itemId);
                foreach (var item in items)
                {
                    Words.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedGroup = null;
        }
        public VocabularyItem SelectedWord
        {
            get => _selectedWord;
            set
            {
                SetProperty(ref _selectedWord, value);
                OnWordSelected(value);
            }
        }

        async void OnWordSelected(VocabularyItem word)
        {
            if (word == null)
                return;

            Console.WriteLine("Word index: " + Words.IndexOf(word));
            Console.WriteLine("Word count: " + Words.Count);
            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(WordDetailPage)}?{nameof(WordDetailViewModel.ItemId)}={word.Id}");
            await Shell.Current.GoToAsync($"{nameof(WordDetailPage)}?{nameof(WordDetailViewModel.ItemId)}={Words.IndexOf(word)}");
        }

        void RepeatButton()
        {
            OnWordSelected(Words[0]);
        }

        async void StudyButton()
        {
            await Shell.Current.GoToAsync($"{nameof(WordStudyPage)}?{nameof(WordStudyViewModel.ItemId)}={Words[0]}");
        }

    }
}
