using PCCharacterManager.ViewModels;

namespace PCCharacterManager.Commands
{
	public class SaveCharacterCommand : BaseCommand
	{
		private readonly MainWindowViewModel _mainWindowViewModel;

		public SaveCharacterCommand(MainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}
		public override void Execute(object? parameter)
		{
			_mainWindowViewModel.TabVM.CharacterListVM.SaveCharacter();
		}
	}
}
