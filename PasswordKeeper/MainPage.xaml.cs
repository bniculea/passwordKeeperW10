using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatabaseTools;
using Model;
using PasswordKeeper.Views;

namespace PasswordKeeper
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string TableName = "passwords.sqlite";

        public DataManager DataManager { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            DataManager = new DataManager(TableName);
            DataManager.CreateTable<Entry>();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (AddNewView), DataManager);
        }

        private void BtnViewAll_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DisplayAllView), DataManager);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DataManager.DropTable<Entry>();
        }
    }
}
