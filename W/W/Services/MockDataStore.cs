using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using W.Models;

namespace W.Services
{
    public class MockDataStore : IDataStore<Group>
    {
        readonly List<Group> items;

        public MockDataStore()
        {
            items = new List<Group>();
            //{
            //    new Group { Id = 0, Text = "First item", Description="This is an item description." }
            //};
        }

        public async Task<bool> AddItemAsync(Group item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Group item)
        {
            var oldItem = items.Where((Group arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Group arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Group> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Group>> GetItemsAsync(bool forceRefresh = false)
        {
            //return await Task.FromResult(items);
            return await App.VocabularyDatabase.GetGroupsAsync();
        }

        public async Task<IEnumerable<VocabularyItem>> GetItemsByGroupIdAsync(bool forceRefresh = false)
        {
            //return await Task.FromResult(items);
            return await App.VocabularyDatabase.GetWordsByGroupIdAsync(1);
        }
    }
}