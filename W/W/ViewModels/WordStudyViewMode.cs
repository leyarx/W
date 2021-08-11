using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using W.Models;
using W.Views;
using Xamarin.Forms;

namespace W.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class WordStudyViewModel : BaseViewModel
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
                //Description = word.Translation;
                Translation = "";
                PlayButton();
            }
        }

        public ObservableCollection<VocabularyItem> Words;

        private bool step;

        private int itemId = 0;
        private string position;
        private string text;
        private string translation;
        public string Id { get; set; }
        public Command PlayCommand { get; }
        public Command YesCommand { get; }
        public Command NoCommand { get; }


        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Translation
        {
            get => translation;
            set => SetProperty(ref translation, value);
        }

        public string ItemId
        {
            get
            {
                return itemId.ToString();
            }
            set
            {
                Int32.TryParse(value, out itemId);
                // check for list boundaries
                Word = Words[itemId];
                Position = (itemId + 1).ToString();
            }
        }
        public string Position
        {
            get => position;
            set => SetProperty(ref position, value + "/" + Words.Count.ToString());
        }

        public WordStudyViewModel()
        {
            Title = "Study";
            Words = App.VocabularyDatabase.CurrentWords;
            PlayCommand = new Command(PlayButton);
            YesCommand = new Command(async () => await OnYesAsync());
            NoCommand = new Command(async () => await OnNoAsync());
        }
        private void PlayButton()
        {
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(new MemoryStream(word.Voice));
            player.Play();
        }
        private async Task OnYesAsync()
        {           
            if(step)
            {
                word.KnowledgeLevel = 2;
                await App.VocabularyDatabase.UpdateVocabularyItemAsync(word);

                step = false;

                if (itemId < Words.Count - 1)
                {
                    ItemId = (itemId + 1).ToString();
                }
                else
                {
                    await App.VocabularyDatabase.UpdateGroupAsync(word.Id_group);
                    Shell.Current.SendBackButtonPressed();
                    //await Shell.Current.GoToAsync("..");
                    //await Shell.Current.GoToAsync($"{nameof(WordGroupDetailPage)}?{nameof(WordGroupDetailViewModel.ItemId)}={word.Id_group}");
                }
            }
            else
            {
                Translation = word.Translation;
                step = true;
            }
        }

        private async Task OnNoAsync()
        {
            if (step)
            {
                word.KnowledgeLevel = 1;
                await App.VocabularyDatabase.UpdateVocabularyItemAsync(word);

                step = false;

                if (itemId < Words.Count - 1)
                {
                    ItemId = (itemId + 1).ToString();
                }
                else
                {
                    await App.VocabularyDatabase.UpdateGroupAsync(word.Id_group);
                    Shell.Current.SendBackButtonPressed();
                    //await Shell.Current.GoToAsync("..");
                    //await Shell.Current.GoToAsync($"{nameof(WordGroupDetailPage)}?{nameof(WordGroupDetailViewModel.ItemId)}={word.Id_group}");
                }
            }
            else
            {
                Translation = word.Translation;
                step = true;
            }
        }


    }
}
