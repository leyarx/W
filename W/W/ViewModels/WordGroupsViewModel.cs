using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using W.Models;
using W.Views;
using System.Linq;

namespace W.ViewModels
{
    public class WordGroupsViewModel : BaseViewModel
    {
        private Group _selectedGroup;

        public ObservableCollection<Group> Groups { get; }
        public Command LoadGroupsCommand { get; }
        public Command AddGroupCommand { get; }
        public Command StudiedCommand { get; }
        public Command KnownCommand { get; }
        public Command<Group> GroupTapped { get; }

        public WordGroupsViewModel()
        {
            Title = "Browse";
            Groups = new ObservableCollection<Group>();
            LoadGroupsCommand = new Command(async () => await ExecuteLoadGroupsCommand());

            GroupTapped = new Command<Group>(OnGroupSelected);

            AddGroupCommand = new Command(OnAddGroup);
            StudiedCommand = new Command(OnStudied);
            KnownCommand = new Command(OnKnown);
        }

        async Task ExecuteLoadGroupsCommand()
        {
            IsBusy = true;

            try
            {
                Groups.Clear();
                var items = (await DataStore.GetItemsAsync(true)).Reverse();
                foreach (var item in items)
                {
                    Groups.Add(item);
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
            SelectedGroup = null;
        }

        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                OnGroupSelected(value);
            }
        }

        private async void OnAddGroup(object obj)
        {
            Console.WriteLine(nameof(NewWordGroupPage));
            //Console.WriteLine(Groups.Count);
            await Shell.Current.GoToAsync(nameof(NewWordGroupPage));
        }
        
        async void OnGroupSelected(Group item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(WordGroupDetailPage)}?{nameof(WordGroupDetailViewModel.ItemId)}={item.Id}");
        }

        private async void OnStudied(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(WordGroupDetailPage)}?{nameof(WordGroupDetailViewModel.ItemId)}=0");
        }
        private async void OnKnown(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(WordGroupDetailPage)}?{nameof(WordGroupDetailViewModel.ItemId)}=0");
        }
    }
}