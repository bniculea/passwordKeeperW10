using System;
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
    public sealed partial class EditView : Page
    {
        private NavigationHelper NavigationHelper { get; set; }
        private ObservableRangeCollection<string> AvailableCategories { get; set; } 
        private bool IsDirty { get; set; }
        private string DefaultName { get; set; }
        private string DefaultPassword { get; set; }
        private string DefaultCategory { get; set; }
        private App App { get; set; }
        public EditView()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            App = (App)Application.Current;
        }

        private async void App_OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (IsDirty)
            {
                MessageDialog messageDialog = CreateMessageDialog();
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

        private static MessageDialog CreateMessageDialog()
        {
            MessageDialog messageDialog = new MessageDialog("Unsaved changes. Do you wish to exit?", "Pending changes");
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.Commands.Add(new UICommand("No"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            return messageDialog;
        }

        private void TxtName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        private void TxtPassword_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        private void TextBoxCategoryName_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
                
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            App.OnBackRequested += App_OnBackRequested;
            IsDirty = false;
            Entry entry = e.Parameter as Entry;
            if (entry != null)
            {
                InitDefaultSettings(entry);
                AvailableCategories = DataHandler.Instance.GetUniqueCategories();
                TxtName.Text = entry.Name;
                TxtPassword.Password = entry.Password;
                ComboCategories.ItemsSource = AvailableCategories;
                ComboCategories.SelectedIndex = AvailableCategories.IndexOf(entry.Category);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.OnBackRequested -= App_OnBackRequested;
        }

        private void InitDefaultSettings(Entry entry)
        {
            DefaultName = entry.Name;
            DefaultPassword = entry.Password;
            DefaultCategory = entry.Category;
        }
        #endregion

        private void ComboCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void AppBarBtnSaveEdit_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void ComboCategories_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!DefaultCategory.Equals(ComboCategories.SelectedItem.ToString()))
            IsDirty = true;
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!DefaultName.Equals(TxtName.Text))
                IsDirty = true;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!TxtPassword.Password.Equals(DefaultPassword))
                IsDirty = true;
        }
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