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
    }
}
