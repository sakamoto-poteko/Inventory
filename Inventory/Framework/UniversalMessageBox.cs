#if !WINDOWS_UWP
using System.Windows;
#else
using Windows.UI.Xaml.Controls;
#endif
namespace Inventory.Framework
{
    public class UniversalMessageBox
    {
        public enum MessageBoxButton
        {
            OK = 0,
            OKCancel = 1,
            YesNoCancel = 3,
            YesNo = 4
        }

        public enum MessageBoxImage
        {
            None = 0,
            Hand = 16,
            Stop = 16,
            Error = 16,
            Question = 32,
            Exclamation = 48,
            Warning = 48,
            Asterisk = 64,
            Information = 64
        }

        public enum MessageBoxResult
        {
            None = 0,
            OK = 1,
            Cancel = 2,
            Yes = 6,
            No = 7
        }

        public static MessageBoxResult Show(string content, string caption, MessageBoxButton button, MessageBoxImage image)
        {
#if WINDOWS_UWP
            string primaryButtonText = null;
            string secondaryButtonText = null;
            string closeButtonText = null;
            switch (button)
            {
                case MessageBoxButton.OK:
                    primaryButtonText = "OK";
                    break;
                case MessageBoxButton.OKCancel:
                    primaryButtonText = "OK";
                    closeButtonText = "Cancel";
                    break;
                case MessageBoxButton.YesNoCancel:
                    primaryButtonText = "Yes";
                    secondaryButtonText = "No";
                    closeButtonText = "Cancel";
                    break;
                case MessageBoxButton.YesNo:
                    primaryButtonText = "Yes";
                    closeButtonText = "No";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }

            ContentDialog dialog = new ContentDialog
            {
                Title = caption,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText
            };

            var result = dialog.ShowAsync();


            switch (result.GetResults())
            {
                case ContentDialogResult.None:
                    switch (button)
                    {
                        case MessageBoxButton.OK:
                            return MessageBoxResult.OK;
                        case MessageBoxButton.YesNo:
                            return MessageBoxResult.No;
                        case MessageBoxButton.OKCancel:
                        case MessageBoxButton.YesNoCancel:
                            return MessageBoxResult.Cancel;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(button), button, null);
                    }
                case ContentDialogResult.Primary:
                    switch (button)
                    {
                        case MessageBoxButton.OK:
                        case MessageBoxButton.OKCancel:
                            return MessageBoxResult.OK;
                        case MessageBoxButton.YesNoCancel:
                        case MessageBoxButton.YesNo:
                            return MessageBoxResult.Yes;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(button), button, null);
                    }

                case ContentDialogResult.Secondary:
                    switch (button)
                    {
                        case MessageBoxButton.OK:
                        case MessageBoxButton.OKCancel:
                        case MessageBoxButton.YesNo:
                            throw new InvalidOperationException("MessageBox does not have a secondary button");
                        case MessageBoxButton.YesNoCancel:
                            return MessageBoxResult.No;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(button), button, null);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
#else
            return (MessageBoxResult)MessageBox.Show(content, caption, (System.Windows.MessageBoxButton)button, (System.Windows.MessageBoxImage)image);
#endif
        }
    }
    }
