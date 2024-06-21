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
			if (File.Exists(CharacterTypeHelper.BuildPath(item)))
			{
				File.Delete(CharacterTypeHelper.BuildPath(item));
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

			string[] characterFiles = Directory.GetFiles(StarfinderResources.CharacterDataDir);
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

			ReadWriteJsonFile<CharacterBase>.WriteFile(CharacterTypeHelper.BuildPath(character), character);
		}
	}
}
