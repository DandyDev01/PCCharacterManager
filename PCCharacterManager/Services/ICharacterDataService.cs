using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public abstract class ICharacterDataService
	{
		public abstract IEnumerable<DnD5eCharacter> GetCharacters();

		public abstract IEnumerable<string> GetCharacterFilePaths();

		public abstract void Save(IEnumerable<DnD5eCharacter> characters);

		public abstract void Save(DnD5eCharacter character);

		public abstract void Add(DnD5eCharacter newCharacter);

		public abstract bool Delete(DnD5eCharacter character);

		public bool Delete(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
				return true;
			}

			return false;
		}
	}
}
