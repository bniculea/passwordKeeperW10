using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Model;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace DatabaseTools
{
    public class DataManager
    {
        private string TableName { get; set; }
        public void CreateTable<T>()
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                connection.CreateTable<T>();
            }
        }
        public void AddItemToTable<T>(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                connection.Insert(item);
                connection.Commit();
            }
        }
        public List<T> GetAllElements<T>() where T: class
        {
            List<T> elements = null;
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                elements = connection.Table<T>().ToList();
                connection.Commit();
            }

            return elements;
        }
        private string GetTablePath()
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, TableName);
        }

        public DataManager(string tableName)
        {
            TableName = tableName;
        }

        public void DropTable<T>()
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                connection.DropTable<T>();
                connection.Commit();
            }
        }

        public T GetItem<T>( Expression<Func<T, bool>> predicatExpression) where T:class
        {
            T retItem = default(T);
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                retItem = connection.Table<T>().Where(predicatExpression).First();
            }
            return retItem;
        }

        public void RemoveItemFromTable<T>(T item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), GetTablePath()))
            {
                connection.Delete(item);
                connection.Commit();
            }
        }
    }
}
