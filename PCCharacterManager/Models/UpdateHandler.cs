using PCCharacterManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class UpdateHandler
	{
		public void HandleCharacterFormatChanges(ICharacterDataService _dataService)
		{
			//IEnumerable<Character> enumerable = _dataService.GetCharacters();

			//foreach (var character in enumerable)
			//{
			//	if(character.Inventory.All.Count > 0)
			//	{
			//		foreach (var item in character.Inventory.All)
			//		{
			//			character.Inventory.Add(item);
			//		}
			//		character.Inventory.All.Clear();
			//	}
			//}

			//_dataService.Save(enumerable);
		}
	}
}
