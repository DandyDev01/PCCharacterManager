using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public interface RecoveryBase
	{
		public DnD5eCharacter Undo();

		public DnD5eCharacter Redo();

		public void RegisterChange(DnD5eCharacter state);
	}
}
