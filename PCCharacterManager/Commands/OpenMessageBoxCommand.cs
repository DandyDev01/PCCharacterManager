using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class OpenMessageBoxCommand : BaseCommand
	{
		private MessageBoxResult _result;
		private MessageBoxButton _messageBoxButton;
		private MessageBoxImage _messageBoxImage;
		private string _messageBoxCaption;
		private string _messageBoxText;

		public MessageBoxResult Result => _result;

		public OpenMessageBoxCommand(string messageBoxText, string messageBoxCaption, MessageBoxButton messageBoxButton, 
			MessageBoxImage messageBoxImage)
		{
			_messageBoxImage = messageBoxImage;
			_messageBoxCaption = messageBoxCaption;
			_messageBoxText = messageBoxText;
			_messageBoxButton = messageBoxButton;
		}

		public override void Execute(object? parameter)
		{
			_result = MessageBox.Show("Would you like to add another class?",
					"Add Class", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
		}


	}
}
