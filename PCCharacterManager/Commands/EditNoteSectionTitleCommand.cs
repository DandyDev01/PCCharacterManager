using PCCharacterManager.DialogWindows;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManager.Commands
{
	public class EditNoteSectionTitleCommand : BaseCommand
	{
		private readonly CharacterNoteBookViewModel viewModel;

		public EditNoteSectionTitleCommand(CharacterNoteBookViewModel _viewModel)
		{
			viewModel = _viewModel;
		}

		public override void Execute(object? parameter)
		{
			if (viewModel.SelectedSection == null)
			{
				MessageBox.Show("No section selected", "Requres selected section",
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Window window = new StringInputDialogWindow();
			DialogWindowStringInputViewModel dataContext = new(window);
			window.DataContext = dataContext;

			bool? result = window.ShowDialog();

			if (result == false)
				return;

			string inputTitle = dataContext.Answer;
			viewModel.SelectedSection.SectionTitle = inputTitle;
		}
	}
}
