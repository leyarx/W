using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace W
{
    public class MainViewModel : INotifyPropertyChanged
    {

        int Id = 1;

        string boxColor = "#FFF800";

        private Random rnd = new Random();

        public string BoxColor
        {
            get => boxColor;
            set
            {
                if (boxColor == value)
                    return;

                boxColor = value;
                OnPropertyChanged(nameof(BoxColor));
            }
        }

        string name = "Lol"; // String.Empty;

        public MainViewModel()
        {
            ButtonCommand = new Command(DoButton);

            LeftCommand = new Command(()=> Name = "Left");
            RightCommand = new Command(()=> Name = "Right");

            SwipeCommand = new Command<string>(SwipeExecute);
        }

        void DoButton()
        {
            Name = "Wooooow";

            BoxColor = "#" + rnd.Next(256).ToString("X") + rnd.Next(256).ToString("X") + rnd.Next(256).ToString("X");
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;

                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command ButtonCommand { get; }

        public Command LeftCommand { get; }
        public Command RightCommand { get; }

        public Command<string> SwipeCommand { get; }

        void SwipeExecute(string name)
        {
            if (name == "Left")
            {
                NextWord();
                Name = "Go Left";
            }
            else if (name == "Right")
            {
                //var v = App.VocabularyDatabase;
                //var a = App.VocabularyDatabase.GetItemAsync(1).Result;
                //Console.WriteLine(a.Word);
                //Name = a.Word;
                
                Name = "Go Right";
            }
            else
            {

            }
        }

        private string vocabularyWord;
        public string VocabularyWord
        {
            get => vocabularyWord;
            set
            {
                if (vocabularyWord == value)
                    return;

                vocabularyWord = value;
                OnPropertyChanged(nameof(VocabularyWord));
            }
        }

        private string vocabularyTranslation;
        public string VocabularyTranslation
        {
            get => vocabularyTranslation;
            set
            {
                if (vocabularyTranslation == value)
                    return;

                vocabularyTranslation = value;
                OnPropertyChanged(nameof(VocabularyTranslation));
            }
        }

        async Task NextWord()
        {
            var newWord = await App.VocabularyDatabase.GetItemAsync(Id++);

            VocabularyWord = newWord.Word;
            VocabularyTranslation = newWord.Translation;
        }
    }
}
