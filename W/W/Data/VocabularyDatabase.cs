using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using W.Models;

namespace W.Data
{
    public class VocabularyDatabase
    {
        static SQLiteAsyncConnection Database;
        public ObservableCollection<VocabularyItem> CurrentWords;// { get; }

        public VocabularyDatabase()
        {
            string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                "Vocabulary.db");

            Assembly assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream embeddedDatabaseStream = assembly.GetManifestResourceStream("W.Vocabulary.db");

            if (!File.Exists(DatabasePath))
            {
                FileStream fileStreamToWrite = File.Create(DatabasePath);
                embeddedDatabaseStream.Seek(0, SeekOrigin.Begin);
                embeddedDatabaseStream.CopyTo(fileStreamToWrite);
                fileStreamToWrite.Close();
            }

        Database = new SQLiteAsyncConnection(DatabasePath);
            Database.CreateTableAsync<VocabularyItem>().Wait();
// TODO: refactor this part
            Database.CreateTableAsync<Group>().Wait();
        }

        public Task<List<VocabularyItem>> GetItemsAsync()
        {
            return Database.Table<VocabularyItem>().ToListAsync();
        }       
        
        public Task<List<Group>> GetGroupsAsync()
        {
            return Database.Table<Group>().ToListAsync();
        }

        public Task<Group> GetGroupAsync(int id)
        {
            return Database.Table<Group>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<VocabularyItem>> GetWordsByGroupIdAsync(int group_id)
        {
            //return Database.QueryAsync<VocabularyItem>("select * from VocabularyItem where Id_group IS NULL");
            //Console.WriteLine(a);
            return Database.Table<VocabularyItem>().Where(w => w.Id_group == group_id).OrderBy(x => x.KnowledgeLevel).ToListAsync();
            //return Database.Table<VocabularyItem>().ToListAsync();
        }

        public Task<VocabularyItem> GetItemAsync(int id)
        {
            return Database.Table<VocabularyItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<VocabularyItem> GetRandomItemAsync()
        {
            //var a = await Database.QueryAsync<VocabularyItem>("select * from VocabularyItem where Id_group IS NULL ORDER BY RANDOM() LIMIT 1");
            //a.FirstOrDefault();
            return (await Database.QueryAsync<VocabularyItem>("select * from VocabularyItem where Id_group IS NULL ORDER BY RANDOM() LIMIT 1")).FirstOrDefault();
            //return GetItemAsync(1);
        }

        public Task<int> AddGroupAsync(Group group)
        {
            return Database.InsertAsync(group);
            //return Task.FromResult(true);
        }

        public Task<int> RemoveGroupAsync(Group group)
        {
            return Database.DeleteAsync(group);
        }

        //public Task<bool> UpdateVocabularyItemAsync(VocabularyItem item)
        public Task UpdateVocabularyItemAsync(VocabularyItem item)
        {
            
            //return Task.FromResult(true);
            return Database.UpdateAsync(item);
        }

        public Task UpdateGroupAsync(Group group)
        {
            return Database.UpdateAsync(group);
        }

        public async Task UpdateGroupAsync(int group_id)
        {
            Group group = await GetGroupAsync(group_id);
            int count = await Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM VocabularyItem where Id_group IS " + group_id);
            int known = await Database.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM VocabularyItem where Id_group IS " + group_id + " AND KnowledgeLevel IS 2");
            group.Description = known.ToString()
                                + "/"
                                + count.ToString();
            await UpdateGroupAsync(group);
            //await Database.UpdateAsync(group);
        }
    }
}
