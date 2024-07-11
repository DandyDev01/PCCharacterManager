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
		public Action? OnSave;

		public abstract IEnumerable<CharacterBase> GetCharacters();

		public abstract IEnumerable<string> GetCharacterFilePaths();

		public abstract void Save(IEnumerable<CharacterBase> characters);

		public abstract void Save(CharacterBase character);

		public abstract void Add(CharacterBase newCharacter);

		public abstract bool Delete(CharacterBase character);

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
