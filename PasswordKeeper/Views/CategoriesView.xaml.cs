using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Common;
using Model;

namespace PasswordKeeper.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriesView : Page
    {
        private ObservableCollection<Entry> EntriesObservableCollection { get; set; } 
        public CategoriesView()
        {
            this.InitializeComponent();
            InitializeCategories();
            
        }

        private void InitializeCategories()
        {
            List<Entry> entries = DataHandler.Instance.GetAllEntries().OrderBy(entry => entry.Category).ToList();
            EntriesObservableCollection = new ObservableCollection<Entry>();
            foreach (Entry entry in entries)
            {
                EntriesObservableCollection.Add(entry);
            }
            List<string> distinctCategories = EntriesObservableCollection.Select(k => k.Category).Distinct().ToList();
            CategoriesList.ItemsSource = distinctCategories;

        }

        private void ToogleButton_OnClick(object sender, RoutedEventArgs e)
        {
            CategorySplitter.IsPaneOpen = !CategorySplitter.IsPaneOpen;
        }

        private void CategoriesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Entry> entriesOfSelectedCategory = new ObservableRangeCollection<Entry>();
            ListBox listBox = sender as ListBox;
            string selectedCategory = listBox?.SelectedItem as string;
            if (selectedCategory != null)
            {
                var entriesAccordingToSelectedCategory= EntriesObservableCollection.Where(entry => entry.Category.Equals(selectedCategory));
                foreach (Entry entry in entriesAccordingToSelectedCategory)
                {
                    entriesOfSelectedCategory.Add(entry);
                }
                this.AccountsPerCategoryList.ItemsSource = entriesOfSelectedCategory;
            }

        }
    }
}
