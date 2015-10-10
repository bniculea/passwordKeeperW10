using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Model;
using PasswordKeeper.Views;

namespace PasswordKeeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            DataHandler.Instance.InitDatabase();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (AddNewView));
        }

        private void BtnViewAll_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DisplayAllView));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DataHandler.Instance.DropTable();
        }

        private void BtnExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (CategoriesView));
        }

        private async void BtnPopulateDatabase_OnClick(object sender, RoutedEventArgs e)
        {
           await  DataHandler.Instance.AddToDatabase("Facebook", "12345", "Social");
           await  DataHandler.Instance.AddToDatabase("Skype", "12345", "Social");
           await  DataHandler.Instance.AddToDatabase("Twitter", "12345", "Social");
           await  DataHandler.Instance.AddToDatabase("Tinder", "12345", "Social");
           await  DataHandler.Instance.AddToDatabase("Google Plus", "12345", "Social");
           await  DataHandler.Instance.AddToDatabase("9Gag", "Fun", "Social");
           await  DataHandler.Instance.AddToDatabase("Youtube", "12345", "Blogging");
           await  DataHandler.Instance.AddToDatabase("GsmArena", "12345", "News");
           await  DataHandler.Instance.AddToDatabase("Sport.ro", "12345", "Sport");
           await  DataHandler.Instance.AddToDatabase("Gsp.ro", "12345", "Sport");
           await  DataHandler.Instance.AddToDatabase("Sky Sports", "12345", "Sport");
           await  DataHandler.Instance.AddToDatabase("Outlook", "12345", "Mail");
           await  DataHandler.Instance.AddToDatabase("Yahoo", "12345", "Mail");
           await  DataHandler.Instance.AddToDatabase("Gmail", "12345", "Mail");
           await  DataHandler.Instance.AddToDatabase("Zimbra", "12345", "Mail");
           await  DataHandler.Instance.AddToDatabase("WikiPedios", "12345", "MindBlow");
           await  DataHandler.Instance.AddToDatabase("Harward", "12345", "MindBlow");
           await  DataHandler.Instance.AddToDatabase("Standford", "12345", "MindBlow");
           await  DataHandler.Instance.AddToDatabase("Dropbox", "12345", "Cloud");
           await  DataHandler.Instance.AddToDatabase("Box", "12345", "Cloud");
           await  DataHandler.Instance.AddToDatabase("OneDrive", "12345", "Cloud");
        }

        private void BtnRemoveAll_OnClick(object sender, RoutedEventArgs e)
        {
            DataHandler.Instance.RemoveAllEntries();
        }
    }
}
