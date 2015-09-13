using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Common;
using Model;

namespace PasswordKeeper.Views
{
    public sealed partial class AddNewView : Page
    {
        private static string EntryAlreadyExistWarning = "There is already an account with this name. Please choose a different one :)";
        private NavigationHelper NavigationHelper { get; set; }
        private ObservableDictionary DefaultViewModel { get; set; }
        private ObservableRangeCollection<String> Categories { get; set; } 
        public AddNewView()
        {
            this.InitializeComponent();
            DefaultViewModel = new ObservableDictionary();
            Categories = DataHandler.Instance.GetUniqueCategories();
            ComboCategories.ItemsSource = Categories;
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
            HandleBackButtonPressed();
        }

        private void HandleBackButtonPressed()
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Debug.WriteLine("BackRequested");
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };
            //if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            //{
            //    Windows.Phone.UI.Input.HardwareButtons.BackPressed += (s, a) =>
            //    {
            //        Debug.WriteLine("BackPressed");
            //        if (Frame.CanGoBack)
            //        {
            //            Frame.GoBack();
            //            a.Handled = true;
            //        }
            //    };
            //}
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
         
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
       
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
                Entry allreadyExistEntry = DataHandler.Instance.GetEntry(expression);
                if (allreadyExistEntry != null)
                {
                    await new MessageDialog(EntryAlreadyExistWarning).ShowAsync();
                }
                else
                {
                    StoreNewEntry(category, name, password);
                }
            }
        }

        private void StoreNewEntry(string category, string name, string password)
        {
            Entry entry = new Entry {Category = category, Name = name, Password = password};
            DataHandler.Instance.AddEntry(entry);
            if (!Categories.Contains(entry.Category)) Categories.Add(entry.Category);
            ResetControls();
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
            if (ComboCategories.SelectedItem != null)
            {
                var selectedItem = ComboCategories.SelectedItem.ToString();
                ShowCategoriesControls(selectedItem.Equals("Custom") ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        private void ShowCategoriesControls(Visibility visibility)
        {
            TextBlockCatName.Visibility = visibility;
            TextBoxCategoryName.Visibility = visibility;
        }

        private void TxtCategoryName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Accept)
                this.TextBoxCategoryName.Focus(FocusState.Unfocused);
        }

        #region NavigationHelper registration
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private void ShowPasswordCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            TxtPassword.PasswordRevealMode = PasswordRevealMode.Visible;
        }

        private void ShowPasswordCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            TxtPassword.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
    }
}
