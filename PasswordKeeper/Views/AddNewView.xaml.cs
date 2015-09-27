using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Common;

namespace PasswordKeeper.Views
{
    public sealed partial class AddNewView : Page
    {
        private NavigationHelper NavigationHelper { get; set; }
        private ObservableDictionary DefaultViewModel { get; set; }
        private ObservableRangeCollection<String> Categories { get; set; } 
        private  App App { get; set; }
        private bool IsDirty { get; set; }
        public AddNewView()
        {
            this.InitializeComponent();
            DefaultViewModel = new ObservableDictionary();
            Categories = DataHandler.Instance.GetUniqueCategories();
            ComboCategories.ItemsSource = Categories;
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
            App = (App)Application.Current;
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
         
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
       
        }

        private async void AppBarBtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            string password = TxtPassword.Password.Trim();
            string category = GetCategory();
            if (IsValidInput(name, password, category))
            {
                await MessageDialogHelper.PromptStatus(MessageDialogHelper.MandatoryFieldsMessage, MessageDialogHelper.IncompleteInputTitle);
            }
            else
            {
                await SaveEntry(name, password, category);
            }
        }

        private async Task SaveEntry(string name, string password, string category)
        {
            if (!IsDirty)
            {
                await
                    MessageDialogHelper.PromptStatus(MessageDialogHelper.EntryNotSaved,
                        MessageDialogHelper.EntryNotSavedTitle);
                return;
            }

            bool isEntrySuccessfullyAdded = await DataHandler.Instance.AddToDatabase(name, password, category);
            if (!isEntrySuccessfullyAdded) return;
            if (!Categories.Contains(category))
            {
                AddCategoryInOrder(category);
            }
            await MessageDialogHelper.PromptStatus(MessageDialogHelper.EntrySuccesfullySaved, MessageDialogHelper.EntrySuccesfullySavedTitle);
            ResetControls();

        }

        private void AddCategoryInOrder(string category)
        {
            Categories.Add(category);
            Categories.OrderBy(c => c);
        }
        private bool IsValidInput(string name, string password, string category)
        {
            return string.IsNullOrEmpty(name.Trim()) || string.IsNullOrEmpty(password.Trim()) ||
                   string.IsNullOrEmpty(category);
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
                    categoryToSave = selectedItem.ToString().Trim();
            }
            else
            {
                categoryToSave = TextBoxCategoryName.Text.Trim();
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

        private void TxtCategoryName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Accept)
                this.TextBoxCategoryName.Focus(FocusState.Unfocused);
        }

        #region NavigationHelper registration
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            IsDirty = false;
            App.OnBackRequested += App_OnBackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
            App.OnBackRequested -= App_OnBackRequested;
        }

        #endregion
        private async void App_OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (IsDirty)
            {
                MessageDialog messageDialog = MessageDialogHelper.CreateDialogForConfirmOnExit();
                var result = await messageDialog.ShowAsync();
                if (result.Label.Equals("Yes") && NavigationHelper.CanGoBack())
                {
                    NavigationHelper.GoBack();
                    IsDirty = false;
                }
            }
            else
            {
                if (NavigationHelper.CanGoBack())
                {
                    NavigationHelper.GoBack();
                    e.Handled = true;
                }
            }
        }
        private void ShowPasswordCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            TxtPassword.PasswordRevealMode = PasswordRevealMode.Visible;
        }
        private void ShowPasswordCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            TxtPassword.PasswordRevealMode = PasswordRevealMode.Hidden;
        }
        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDirty = !string.IsNullOrEmpty(TxtName.Text);

        }
        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            IsDirty = !string.IsNullOrEmpty(TxtPassword.Password);
        }
        private void ComboCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDirty = true;
        }
    }
}
