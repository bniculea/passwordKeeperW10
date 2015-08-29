using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DatabaseTools;
using Model;

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
            DataManager = new DataManager();
            DataManager.CreateTable<Entry>(TableName);
        }

        private void btnDisplay_Click(object sender, RoutedEventArgs e)
        {
            var allResults = DataManager.GetAllElements<Entry>(TableName);
            foreach (Entry entry in allResults)
            {
                listView.Items.Add(entry);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DataManager.AddItemToTable(new Entry() {Category ="Email", Name = "Trolo", Password = "dsfdsf"}, TableName);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnDrop_Click(object sender, RoutedEventArgs e)
        {
            DataManager.DropTable<Entry>(TableName);
        }
    }
}
