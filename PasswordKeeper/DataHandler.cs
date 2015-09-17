using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Common;
using DatabaseTools;
using Model;

namespace PasswordKeeper
{
    public class DataHandler
    {
        private string TableName = "passwords.sqlite";
        private static string EntryAlreadyExistWarning = "There is already an account with this name. Please choose a different one :)";
        private static DataHandler _instance;
        private DataManager DataManager { get; set; }
        public static DataHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataHandler();
                return _instance;
            }
        }
        private DataHandler()
        {
            DataManager = new DataManager(TableName);
        }

        public void InitDatabase()
        {
            DataManager.CreateTable<Entry>();
        }

        public void DropTable()
        {
           DataManager.DropTable<Entry>();
        }

        public void RemoveEntry(Entry entryToDelete)
        {
           DataManager.RemoveItemFromTable(entryToDelete);
        }

        public Entry GetEntry(Expression<Func<Entry, bool>> expression)
        {
             Entry entry = DataManager.GetItem(expression);
            return entry;
        }

        public List<Entry> GetAllEntries()
        {
            return DataManager.GetAllElements<Entry>();
        }

        public ObservableRangeCollection<string> GetUniqueCategories()
        {
            ObservableRangeCollection<string> uniqueCategoriesCollection = new ObservableRangeCollection<string>();
            IEnumerable<string> uniqueCategories = GetUniqueCategoriesList();
            foreach (string category in uniqueCategories)
            {
                uniqueCategoriesCollection.Add(category);
            }
            return uniqueCategoriesCollection;
        }

        private IEnumerable<string> GetUniqueCategoriesList()
        {
            List<string> defaultCategories = new List<string> {"Email", "Website", "Other", "Custom"};
            List<string> addedCategoriesList = GetAllEntries().Select(entry => entry.Category).ToList();
            defaultCategories.AddRange(addedCategoriesList);
            IEnumerable<string> uniqueCategories = defaultCategories.Distinct();
            return uniqueCategories;
        }

        public void AddEntry(Entry entry)
        {
            DataManager.AddItemToTable(entry);
        }

        public void UpdateItem(string oldName, string newName, string newCategory, string newPassword)
        {
            string command =
                $"Update Entry SET Name='{newName}', Password='{newPassword}', Category='{newCategory}' WHERE Name='{oldName}'";
            ExecuteScalar(command);
        }
        private void ExecuteScalar(string query)
        {
            DataManager.ExecuteScalar(query, query.Length);
        }

        public async Task<bool> AddToDatabase(string name, string password, string category)
        {
            bool isAddPerformed = false;
            if (IsEntryInDatabase(name))
            {
                await PromptEntryAlreadyExistWarning();
            }
            else
            {
                StoreNewEntry(category, name, password);
                isAddPerformed = true;
            }
            return isAddPerformed;
        }

        public async Task PromptEntryAlreadyExistWarning()
        {
            await new MessageDialog(EntryAlreadyExistWarning).ShowAsync();
        }

        private void StoreNewEntry(string category, string name, string password)
        {
            Entry entry = new Entry { Category = category, Name = name, Password = password };
            AddEntry(entry);
        }

        public bool IsEntryInDatabase(string name)
        {
            Expression<Func<Entry, bool>> expression = (k => k.Name.Equals(name));
            return GetEntry(expression) != null;
        }
    }
}
