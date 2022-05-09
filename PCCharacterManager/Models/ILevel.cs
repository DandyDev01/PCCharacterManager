using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public interface ILevel
	{
		int Level { get; set; }

		void LevelUp();
	}
}
