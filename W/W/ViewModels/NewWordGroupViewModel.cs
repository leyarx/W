using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using W.Models;
using Xamarin.Forms;

namespace W.ViewModels
{
    public class NewWordGroupViewModel : BaseViewModel
    {
        int count = 0;
        int maxCount = 5;

        private string position;

        public Command PlayCommand { get; }

        private VocabularyItem newWord;
        private Group newGroup;

        private string word;
        private string translation;

        private string text;
        private string description;

        // TODO: implement frozen button and wait for next word

        public NewWordGroupViewModel()
        {
            AddCommand = new Command(OnAdd, ValidateNext);
            KnowCommand = new Command(OnKnow);
            PlayCommand = new Command(PlayButton);

            newGroup = new Group();

            //SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            //this.PropertyChanged +=
            //    (_, __) => SaveCommand.ChangeCanExecute();        

            //this.PropertyChanged +=
            //    (_, __) => AddCommand.ChangeCanExecute();            

            //TODO: Wait for new group id
            //Count = 0;
            GetGroupId();
        }

        private bool ValidateNext()
        {
            return true;
        }       
        
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        public int Count
        {
            get => count;
            set 
            { 
                SetProperty(ref count, value);
                Position = value.ToString();
            }
        }

        public string Word
        {
            get => word;
            set => SetProperty(ref word, value);
        }

        public string Translation
        {
            get => translation;
            set => SetProperty(ref translation, value);
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

        public string Position
        {
            get => position;
            set => SetProperty(ref position, value + "/" + maxCount);
        }

        public Command AddCommand { get; }
        public Command KnowCommand { get; }

        private async void OnAdd()
        {

            Count++;
            Console.WriteLine("OnAdd start: " + Count);
            newWord.Id_group = newGroup.Id;
            await App.VocabularyDatabase.UpdateVocabularyItemAsync(newWord);
            // update Group Description
            await App.VocabularyDatabase.UpdateGroupAsync(newWord.Id_group);

            if (Count == maxCount)
            {
                Console.WriteLine("OnAdd if: " + Count);
                // TODO: save word group and exit
                //await Shell.Current.GoToAsync("..");
                await OnSave();
                return;
            }

            await NextWord();
            Console.WriteLine("OnAdd end: " + Count);
        }

        private async void OnKnow()
        {
            newWord.Id_group = 0;
            await App.VocabularyDatabase.UpdateVocabularyItemAsync(newWord);
            await NextWord();
        }

        private async void GetGroupId()
        {
            IsBusy = true;

            try
            { 
                await App.VocabularyDatabase.AddGroupAsync(newGroup);
                Console.WriteLine("GetGroupId: " + newGroup.Id);
                newGroup.Text = "Set #" + newGroup.Id.ToString();

                if (newGroup.Id <= 5)
                {
                    maxCount = 5;
                }
                else if (newGroup.Id <= 10)
                {
                    maxCount = 10;
                }
                else if (newGroup.Id <= 15)
                {
                    maxCount = 15;
                }
                else
                    maxCount = 20;

                Count = 0;

                await App.VocabularyDatabase.UpdateGroupAsync(newGroup);
                await NextWord();
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to get new Group Id");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NextWord()
        {
            //newWord = await App.VocabularyDatabase.GetItemAsync(Id++);
            newWord = await App.VocabularyDatabase.GetRandomItemAsync();
            if(newWord == null)
            {
                await OnSave();
                return;
            }
            Word = newWord.Word;
            Translation = newWord.Translation;
            PlayButton();
        }

        void PlayButton()
        {
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            //player.Load(GetStreamFromFile("abandon.mp3"));
            player.Load(new MemoryStream(newWord.Voice));
            player.Play();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {

            Console.WriteLine("OnCancel");
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async Task OnSave()
        {
            //Group newItem = new Group()
            //{
            //    Text = "la",//Text,
            //    Description = "lalala"//Description
            //};

            //await DataStore.AddItemAsync(newItem);

            //await App.VocabularyDatabase.AddGroupAsync(newGroup);

            // This will pop the current page off the navigation stack

            await App.VocabularyDatabase.UpdateGroupAsync(newGroup.Id);

            if (count == 0)
            {
                await App.VocabularyDatabase.RemoveGroupAsync(newGroup);
                //await App.Current.MainPage.DisplayAlert("Congratulations!!!", "No more words to learn", "OK");
            }
            
            await Shell.Current.GoToAsync("..");
        }
    }
}
