using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Common;
using Model;
using PasswordKeeper.Repository;

namespace PasswordKeeper.Views
{
    public sealed partial class DisplayAllView : Page
    {
        private NavigationHelper NavigationHelper { get; set; }
        private ObservableDictionary DefaultViewModel { get; set; }
        private ObservableRangeCollection<GroupInfoList<object>> _dataLetter = null;
        private EntryRepository EntryRepository { get; set; }
        private ListViewBase ListViewBase { get; set; }
        private Border GroupHeaderBorder { get; set; }
        private int TotalEntries { get; set; }
        public DisplayAllView()
        {
            this.InitializeComponent();
            EntryRepository = new EntryRepository();
            NavigationHelper = new NavigationHelper(this);
            DefaultViewModel = new ObservableDictionary();
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        private void GroupHeaderBorder_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            SemanticZoom.IsZoomedInViewActive = false;
        }

        private void GroupHeaderBorder_OnLoaded(object sender, RoutedEventArgs e)
        {
            GroupHeaderBorder = sender as Border;
            if (GroupHeaderBorder != null)
            {
                EnableDisableGroupHeader(GroupHeaderBorder);
            }
        }

        private void ListViewItem_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);

            //To get the clicked item
            // var datacontext = (e.OriginalSource as FrameworkElement).DataContext;
        }

        private void MenuFlyoutItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
           MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem != null)
            {
                Entry entryToDelete = GetEntry(menuFlyoutItem);
      
                DataHandler.Instance.RemoveEntry(entryToDelete);
                var group = _dataLetter.First(k => k.Contains(entryToDelete));
                int entryIndexInGroup = group.IndexOf(entryToDelete);
                group.RemoveAt(entryIndexInGroup);
                TotalEntries--;
                if (GroupHeaderBorder != null)
                {
                    EnableDisableGroupHeader(GroupHeaderBorder);
                }
            }
        }

        private Entry GetEntry(MenuFlyoutItem menuFlyoutItem)
        {
            string tag = menuFlyoutItem.Tag.ToString();
            Expression<Func<Entry, bool>> expression = (k => k.Name.Equals(tag));
            Entry entry = DataHandler.Instance.GetEntry(expression);
            return entry;
        }

        private void EnableDisableGroupHeader(Border groupHeaderBorder)
        {
            if (TotalEntries < 10)
            {
                groupHeaderBorder.Width = 0;
                groupHeaderBorder.Height = 0;
                groupHeaderBorder.Opacity = 0;
                //Opacity = 0;
            }
        }

        private void EditFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem != null)
            {
                Entry entryToEdit = GetEntry(menuFlyoutItem);
                Frame.Navigate(typeof (EditView), entryToEdit);
            }
        }

        private void CopyToClipboardFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem != null)
            {
                Entry selectedEntry = GetEntry(menuFlyoutItem);
                if (selectedEntry  != null) SetClipboardContent(selectedEntry);
            }
           
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

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        #region NavigationHelper registration
        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            var entryList = AddDataToRepository();
            EntryRepository.Collection.AddRange(entryList);
            TotalEntries = EntryRepository.Collection.Count;
            _dataLetter = EntryRepository.GetGroupsByLetter;
            Cvs.Source = _dataLetter;

            ListViewBase = SemanticZoom.ZoomedOutView as ListViewBase;
            if (ListViewBase != null)
                ListViewBase.ItemsSource = EntryRepository.PasswordHeaders;

            //ListViewZoomedInPasswords.SelectionChanged -=  ListViewZoomedIn_SelectionChanged;
            ListViewZoomedInPasswords.SelectedItem = null;

            //ListViewZoomedInPasswords.SelectionChanged +=  ListViewZoomedIn_SelectionChanged;

            this.SemanticZoom.ViewChangeStarted -= SemanticZoom_ViewChangeStarted;
            this.SemanticZoom.ViewChangeStarted += SemanticZoom_ViewChangeStarted;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem == null || e.SourceItem.Item == null) return;
            if (e.SourceItem.Item.GetType() == typeof (HeaderItem))
            {
                HeaderItem headerItem = (HeaderItem) e.SourceItem.Item;
                var group = _dataLetter.First(d => ((char) d.Key) == headerItem.HeaderName);
                if (group != null)
                {
                    e.DestinationItem = new SemanticZoomLocation {Item = group};
                }
            }
        }

        private async void ListViewZoomedIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewZoomedInPasswords.SelectedItem != null)
            {
                Entry entry = ListViewZoomedInPasswords.SelectedItem as Entry;
                MessageDialog md = new MessageDialog(String.Format("You selected: {0} with Psw {1} and Category: {2}", entry.Name, entry.Password, entry.Password));
                await md.ShowAsync();
                ListViewZoomedInPasswords.SelectedItem = null;
            }
        }

        private IEnumerable<Entry> AddDataToRepository()
        {
            List<Entry> allEntries = DataHandler.Instance.GetAllEntries();
            return allEntries;
        }

        #endregion

        private void ListViewItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
    }
}
