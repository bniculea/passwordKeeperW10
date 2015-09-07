using System;
using System.Threading.Tasks;
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
        public EditView()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
        }

        private async void App_OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            MessageDialog messageDialog = new MessageDialog("Unsaved changes. Do you wish to exit?", "Pending changes");
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.Commands.Add(new UICommand("No"));

            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;

            var result = await messageDialog.ShowAsync();
            if (result.Label.Equals("No"))
            {
               
            }
            else
            {
                if (NavigationHelper.CanGoBack())
                {
                    NavigationHelper.GoBack();
                    e.Handled = false;
                }
               
            }
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            App app = (App)Application.Current;
            app.OnBackRequested += App_OnBackRequested;
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

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {

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

        private void TxtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!DefaultName.Equals(TxtName.Text))
            IsDirty = true;
        }

        private void TxtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!TxtPassword.Password.Equals(DefaultPassword))
            IsDirty = true;
        }

        private void ComboCategories_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!DefaultCategory.Equals(ComboCategories.SelectedItem.ToString()))
            IsDirty = true;
        }
    }
}