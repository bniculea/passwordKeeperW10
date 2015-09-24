using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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
        private ObservableCollection<Entry> EntriesOfSelectedCategory { get; set; }
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
            CategorySplitter.CompactPaneLength = 0;
        }

        private void CategoriesList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntriesOfSelectedCategory = new ObservableRangeCollection<Entry>();
            ListBox listBox = sender as ListBox;
            string selectedCategory = listBox?.SelectedItem as string;
            if (selectedCategory != null)
            {
                var entriesAccordingToSelectedCategory= EntriesObservableCollection.Where(entry => entry.Category.Equals(selectedCategory));
                foreach (Entry entry in entriesAccordingToSelectedCategory)
                {
                    EntriesOfSelectedCategory.Add(entry);
                }
                this.AccountsPerCategoryList.ItemsSource = EntriesOfSelectedCategory;
            }

        }

        private void CopyToClipboardFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            Entry selectedEntry = GetEntryFromSelectedItem(e);
            if (selectedEntry == null) return;
            SetClipboardContent(selectedEntry);
        }

        private void EditFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            Entry selectedEntry = GetEntryFromSelectedItem(e);
            if (selectedEntry == null) return;
            Frame.Navigate(typeof (EditView), selectedEntry);
        }



        private void DeleteFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            Entry selectedEntry = GetEntryFromSelectedItem(e);
            DataHandler.Instance.RemoveEntry(selectedEntry);
            EntriesOfSelectedCategory.Remove(selectedEntry);
        }

        private Entry GetEntryFromSelectedItem(RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
            return frameworkElement?.DataContext as Entry;
        }
        private void AssociatedItemStackPanel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
        private void SetClipboardContent(Entry selectedEntry)
        {
            DataPackage dataPackage = new DataPackage();
            try
            {
                dataPackage.SetText(selectedEntry.Password);
                Clipboard.SetContent(dataPackage);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
