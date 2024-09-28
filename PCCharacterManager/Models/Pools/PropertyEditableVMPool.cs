using PCCharacterManager.ViewModels;

namespace PCCharacterManager.Models
{
	public class PropertyEditableVMPool : IPool<PropertyEditableViewModel>
	{
		public PropertyEditableVMPool(int count)
		{
			Add(count);
		}

		public override void Add(int count)
		{
			for (int i = 0; i < count; i++)
			{
				_items.Add(new PropertyEditableViewModel());
				FreeItems++;
			}
		}
	}
}
