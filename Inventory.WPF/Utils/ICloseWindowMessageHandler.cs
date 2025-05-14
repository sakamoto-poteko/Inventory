using CommunityToolkit.Mvvm.Messaging;

using Inventory.ViewModels;

using System;
using System.Windows;

namespace Inventory.WPF.Utils
{

    public class CloseWindowMessageHandler : IRecipient<WindowMessage>
    {
        private readonly Window _window;
        private readonly Guid _messageToken;

        public CloseWindowMessageHandler(Window window, Guid messageToken)
        {
            _window = window;
            _messageToken = messageToken;
            WeakReferenceMessenger.Default.Register<WindowMessage>(this);
        }

        public void Receive(WindowMessage message)
        {
            if (message.MessageToken == _messageToken && message.MessageType == WindowMessage.Type.CloseWindow)
            {
                _window.Close();
            }
        }

    }
}
