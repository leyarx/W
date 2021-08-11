using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using W.Models;
using Xamarin.Forms;

namespace W.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class WordDetailViewModel : BaseViewModel
    {
        private VocabularyItem word;
        private VocabularyItem Word
        {
            get
            {
                return word;
            }
            set
            {
                word = value;
                Id = word.Id.ToString();
                Text = word.Word;
                Description = word.Translation;
                PlayButton();
            }
        }

        public ObservableCollection<VocabularyItem> Words;// { get; }

        private string position;
        private int itemId = 0;
        private string text;
        private string description;
        public string Id { get; set; }
        public Command PlayCommand { get; }
        public Command NextCommand { get; }
        public Command PreviousCommand { get; }

        public string Position
        {
            get => position;
            set => SetProperty(ref position, value + "/" + Words.Count.ToString());
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId.ToString();
            }
            set
            {
                //itemId = value;
                Int32.TryParse(value, out itemId);
                // check for list boundaries
                Word = Words[itemId];
                Position = (itemId + 1).ToString();
                //Words[]
                //LoadItemId(itemId);
            }
        }

        public WordDetailViewModel()
        {
            Words = App.VocabularyDatabase.CurrentWords;// new ObservableCollection<VocabularyItem>();
            //ExecuteLoadWordsCommand();
            PlayCommand = new Command(PlayButton);
            NextCommand = new Command(OnNext);
            PreviousCommand = new Command(OnPrevious);
            //Console.WriteLine("WordDetailViewModel: " + Words[0].Word);
        }
        //async Task ExecuteLoadWordsCommand()
        //{
        //    IsBusy = true;

        //    try
        //    {
        //        Words.Clear();
        //        //var items = await DataStore.GetItemsByGroupIdAsync(true);
        //        // rewrite this part
        //        var items = await App.VocabularyDatabase.GetWordsByGroupIdAsync(itemId);
        //        foreach (var item in items)
        //        {
        //            Words.Add(item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}


        private void PlayButton()
        {
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            //player.Load(GetStreamFromFile("abandon.mp3"));
            player.Load(new MemoryStream(word.Voice));
            player.Play();
        }

        //Stream GetStreamFromFile(string filename)
        //{
        //    var assembly = typeof(App).GetTypeInfo().Assembly;
        //    var stream = assembly.GetManifestResourceStream("W." + filename);
        //    return stream;
        //}

        //public async void LoadItemId(int itemId)
        //{
        //    try
        //    {
        //        //var item = await DataStore.GetItemAsync(itemId);
        //        word = await App.VocabularyDatabase.GetItemAsync(itemId);
        //        Id = word.Id.ToString();
        //        Text = word.Word;
        //        Description = word.Translation;
        //    }
        //    catch (Exception)
        //    {
        //        Debug.WriteLine("Failed to Load Item");
        //    }
        //}
        private void OnNext()
        {
            //int i = Words.IndexOf(word);
            //if (i == -1)
            //{
            //    return;
            //}

            //i++;
            
            if (itemId < Words.Count-1)
            {
                ItemId = (itemId+1).ToString();
                //LoadItemId(Words[i].Id);

            }
            Console.WriteLine("Next word id: " + ItemId + " count " + Words.Count);
            //await NextWord();
        }

        private void OnPrevious()
        {
            if (itemId > 0)
            {
                ItemId = (itemId-1).ToString();
                //LoadItemId(Words[i].Id);
            }

            Console.WriteLine("Previous word id: " + ItemId + " count " + Words.Count);
        }
    }
}
