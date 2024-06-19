using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public class JsonDarkSoulsCharacterDataService : IDataService<DarkSoulsCharacter>
	{
		public void Add(DarkSoulsCharacter item)
		{
			Save(item);
		}

		public bool Delete(DarkSoulsCharacter item)
		{
			if (File.Exists(CharacterTypeHelper.BuildPath(item)))
			{
				File.Delete(CharacterTypeHelper.BuildPath(item));
				return true;
			}

			return false;
		}

		public IEnumerable<string> GetByFilePaths()
		{
			return Directory.GetFiles(DarkSoulsResources.CharacterDataDir);
		}


		public IEnumerable<DarkSoulsCharacter> GetItems()
		{
			List<DarkSoulsCharacter> characters = new();
			string[] characterEntries = Directory.GetFiles(DarkSoulsResources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var item = ReadWriteJsonFile<DarkSoulsCharacter>.ReadFile(characterEntry);
				if (item != null)
					characters.Add(item);
			}

			return characters;
		}

		public void Save(IEnumerable<DarkSoulsCharacter> items)
		{
			throw new NotImplementedException();
		}

		public void Save(DarkSoulsCharacter character)
		{
			// character data folder does not exist
			if (!Directory.Exists(DarkSoulsResources.CharacterDataDir))
			{
				Directory.CreateDirectory(DarkSoulsResources.CharacterDataDir);
			}

			if (character == null)
				return;

			string[] characterFiles = Directory.GetFiles(DarkSoulsResources.CharacterDataDir);
			if (characterFiles.Contains(x => x.Contains(character.Id)))
			{
				string path = characterFiles.Where(x => x.Contains(character.Id)).First();
				string name = path.Substring(path.LastIndexOf('\\') + 1, path.IndexOf("#") - path.LastIndexOf('\\') - 1);
				if (name != character.Name)
				{
					File.Delete(characterFiles.Where(x => x.Contains(character.Id)).First());
				}
			}

			character.DateModified = DateTime.Now.ToString();

			ReadWriteJsonFile<DnD5eCharacter>.WriteFile(CharacterTypeHelper.BuildPath(character), character);
		}
	}
}
