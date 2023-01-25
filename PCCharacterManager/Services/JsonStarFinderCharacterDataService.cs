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
			if (File.Exists(StarfinderResources.CharacterDataDir + "/" + item.Name + ".json"))
			{
				File.Delete(StarfinderResources.CharacterDataDir + "/" + item.Name + ".json");
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
				if (item != null) characters.Add(item);
			}

			return characters;
		}

		public void Save(IEnumerable<StarfinderCharacter> items)
		{
			throw new NotImplementedException();
		}

		public void Save(StarfinderCharacter item)
		{
			// character data folder does not exist
			if (!Directory.Exists(StarfinderResources.CharacterDataDir))
			{
				Directory.CreateDirectory(StarfinderResources.CharacterDataDir);
			}

			if (item == null) return;

			item.DateModified = DateTime.Now.ToString();

			ReadWriteJsonFile<DnD5eCharacter>.WriteFile(StarfinderResources.CharacterDataDir + "/" + item.Name + 
				".json", item);
		}
	}
}
