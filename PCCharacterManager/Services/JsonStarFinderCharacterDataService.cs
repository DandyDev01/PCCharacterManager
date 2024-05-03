using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public class JsonStarFinderCharacterDataService : IDataService<StarfinderCharacter>
	{
		public void Add(StarfinderCharacter item)
		{
			Save(item);
		}

		public bool Delete(StarfinderCharacter item)
		{
			if (File.Exists(StarfinderResources.CharacterDataDir + "/" + item.Name + item.Id + ".json"))
			{
				File.Delete(StarfinderResources.CharacterDataDir + "/" + item.Name + item.Id + ".json");
				return true;
			}

			return false;
		}

		public IEnumerable<string> GetByFilePaths()
		{
			return Directory.GetFiles(StarfinderResources.CharacterDataDir);
		}


		public IEnumerable<StarfinderCharacter> GetItems()
		{
			List<StarfinderCharacter> characters = new List<StarfinderCharacter>();
			string[] characterEntries = Directory.GetFiles(StarfinderResources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var item = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(characterEntry);
				if (item != null) 
					characters.Add(item);
			}

			return characters;
		}

		public void Save(IEnumerable<StarfinderCharacter> items)
		{
			throw new NotImplementedException();
		}

		public void Save(StarfinderCharacter character)
		{
			// character data folder does not exist
			if (!Directory.Exists(StarfinderResources.CharacterDataDir))
			{
				Directory.CreateDirectory(StarfinderResources.CharacterDataDir);
			}

			if (character == null) 
				return;

			string[] characterFiles = Directory.GetFiles(DnD5eResources.CharacterDataDir);
			var test = characterFiles[0].Substring(characterFiles[0].LastIndexOf('\\') + 1, characterFiles[0].IndexOf("#") - characterFiles[0].LastIndexOf('\\') - 1);
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

			ReadWriteJsonFile<DnD5eCharacter>.WriteFile(StarfinderResources.CharacterDataDir + "/" + character.Name + character.Id + ".json", character);
		}
	}
}
