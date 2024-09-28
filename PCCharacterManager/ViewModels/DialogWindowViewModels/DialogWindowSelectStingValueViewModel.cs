using PCCharacterManager.Utility;
using System.Collections.Generic;
using System.Linq;

namespace PCCharacterManager.ViewModels
{
	public class DialogWindowSelectStingValueViewModel : ObservableObject
	{
		private readonly ListViewMultiSelectItemsLimitedCountViewModel _limitedMultiSelectVM;

		public ListViewMultiSelectItemsLimitedCountViewModel LimitedMultiSelectVM { get { return _limitedMultiSelectVM; } }
		public IEnumerable<string> SelectedItems { get { return LimitedMultiSelectVM.SelectedItems; } }


		public DialogWindowSelectStingValueViewModel(string[] options, int maxSelections = 1)
		{
			_limitedMultiSelectVM = new ListViewMultiSelectItemsLimitedCountViewModel(maxSelections, options.ToList());
		}
	}
}
