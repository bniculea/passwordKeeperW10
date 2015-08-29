using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace DatabaseTools
{
    public class DataManager
    {
        public void CreateTable<T>(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath(tableName)))
            {
                connection.CreateTable<T>();
            }
        }
        public void AddItemToTable<T>(T item, string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath(tableName)))
            {
                connection.Insert(item);
                connection.Commit();
            }
        }
        public List<T> GetAllElements<T>(string tableName) where T: class
        {
            List<T> elements = null;
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath(tableName)))
            {
                elements = connection.Table<T>().ToList();
                connection.Commit();
            }

            return elements;
        }
        private string GetTablePath(string tableName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, tableName);
        }

        public void DropTable<T>(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath(tableName)))
            {
                connection.DropTable<T>();
                connection.Commit();
            }
        }
    }
}
