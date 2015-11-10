using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace PasswordKeeper
{
    public static class MessageDialogHelper
    {
        public static string MandatoryFieldsMessage = "Name, Password and Category are mandatory";
        public static string EntrySuccesfullySaved = "Entry was successfully saved.";
        public static string EntryNotSaved = "There are no pending changes.";
        public static string EntrySuccesfullySavedTitle = "Saved Changes";
        public static string EntryNotSavedTitle = "Nothing to save";
        public static string IncompleteInputTitle = "Incomplete input";

        public  static async Task PromptStatus(string statusMessage, string mdTitle)
        {
            MessageDialog messageDialog = new MessageDialog(statusMessage,
               mdTitle);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        public static MessageDialog CreateDialogForConfirmOnExit()
        {
            MessageDialog messageDialog = new MessageDialog("Unsaved changes. Do you wish to exit?", "Pending changes");
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.Commands.Add(new UICommand("No"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;
            return messageDialog;
        }
    }
}
