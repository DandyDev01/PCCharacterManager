using PCCharacterManager.Utility;

namespace PCCharacterManager.ViewModels.DialogWindowViewModels
{
	public class DialogWindowChangeHealthViewModel : ObservableObject
	{
		public int Amount { get; set; }

		private string _answer;
		public string Answer
		{
			get
			{
				return _answer;
			}
			set
			{
				OnPropertyChanged(ref _answer, value);
			}
		}

		private bool _isTempHealth;
		public bool IsTempHealth
		{
			get
			{
				return _isTempHealth;
			}
			set
			{
				OnPropertyChanged(ref _isTempHealth, value);
			}
		}

		public DialogWindowChangeHealthViewModel()
		{
			_answer = string.Empty;
		}

	}
}
