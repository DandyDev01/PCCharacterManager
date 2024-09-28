using PCCharacterManager.Utility;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	class DialogWindowAddConditionViewModel : ObservableObject
	{
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
			}
		}

		private string _description;
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				OnPropertyChanged(ref _description, value);
			}
		}

		private int _duration;
		public int Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				OnPropertyChanged(ref _duration, value);
			}
		}

		public DialogWindowAddConditionViewModel()
		{
			_name = string.Empty;
			_description = string.Empty;

		}
	}
}
