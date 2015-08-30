using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Common;
using DatabaseTools;
using Model;

namespace PasswordKeeper.Views
{
    public sealed partial class AddNewView : Page
    {
        private NavigationHelper NavigationHelper { get; set; }
        private ObservableDictionary DefaultViewModel { get; set; }
        private ObservableRangeCollection<String> Categories { get; set; } 
        private DataManager DataManager { get; set; } 
        public AddNewView()
        {
            this.InitializeComponent();
            DefaultViewModel = new ObservableDictionary();
            Categories = new ObservableRangeCollection<string>();
            UpdateCategories();
            ComboCategories.ItemsSource = Categories;
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
         
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
       
        }

        private void UpdateCategories()
        {
            Categories.Add("Other");
            Categories.Add("Email");
            Categories.Add("Websites");
            Categories.Add("Custom");
            List<Entry> entries = DataManager.GetAllElements<Entry>();
            List<string> groups = entries.Select(entry => entry.Category).ToList();
            foreach (string @group in groups)
            {
                Categories.Add(@group);
            }
        }


        private async void AppBarBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text;
            string password = TxtPassword.Password;
            string category = GetCategory();
            if (string.IsNullOrEmpty(name.Trim()) || string.IsNullOrEmpty(password.Trim()) ||
                string.IsNullOrEmpty(category))
            {
                MessageDialog messageDialog = new MessageDialog("Name, Password and Category are mandatory",
                    "Incomplete input");
                await messageDialog.ShowAsync();
            }
            else
            {
                Expression<Func<Entry, bool>> expression = (k => k.Name.Equals(name));
                Entry allreadyExistEntry = DataManager.GetItem(expression);
                if (allreadyExistEntry != null)
                {
                    MessageDialog messageDialog =
                        new MessageDialog("There is already an account with this name. Please choose a different one :)");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    Entry entry = new Entry(name,password, category);
                    DataManager.AddItemToTable(entry);
                    ResetControls();
                }
            }
        }

        private void ResetControls()
        {
            TxtName.Text = string.Empty;
            TxtPassword.Password = string.Empty;
            TextBoxCategoryName.Text = string.Empty;
            ComboCategories.SelectedIndex = -1;
        }

        private string GetCategory()
        {
            string categoryToSave = string.Empty;
            if (TextBoxCategoryName.Visibility == Visibility.Collapsed)
            {
                var selectedItem = ComboCategories.SelectedItem;
                if (selectedItem != null)
                    categoryToSave = selectedItem.ToString();
            }
            else
            {
                categoryToSave = TextBoxCategoryName.Text;
            }
            return categoryToSave;
        }

        private void TxtName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Accept)
                TxtPassword.Focus(FocusState.Pointer);
        }

        private void TxtPassword_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Accept)
                ComboCategories.Focus(FocusState.Pointer);
        }

        private void ComboCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = string.Empty;
            if (ComboCategories.SelectedItem != null)
            {
                selectedItem = ComboCategories.SelectedItem.ToString();
                if (selectedItem.Equals("Custom"))
                {
                    TextBlockCatName.Visibility = Visibility.Visible;
                    TextBoxCategoryName.Visibility = Visibility.Visible;
                    TextBoxCategoryName.Select(0, 0);
                }
                else
                {
                    TextBlockCatName.Visibility = Visibility.Collapsed;
                    TextBoxCategoryName.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TxtCategoryName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Accept)
                this.TextBoxCategoryName.Focus(FocusState.Unfocused);
        }

        #region NavigationHelper registration
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataManager = e.Parameter as DataManager;
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }
        #endregion
    }
}
