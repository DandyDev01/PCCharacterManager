using Newtonsoft.Json;
using PCCharacterManager.Models;
using PCCharacterManager.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public class JsonDnD5eCharacterDataService : IDataService<DnD5eCharacter>
	{
		public void Add(DnD5eCharacter newCharacter)
		{
			Save(newCharacter);
		}

		public IEnumerable<DnD5eCharacter> GetItems()
		{
			if (Directory.Exists(DnD5eResources.CharacterDataDir) == false)
				return new List<DnD5eCharacter>();

			List<DnD5eCharacter> characters = new();	
			string[] characterEntries = Directory.GetFiles(DnD5eResources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterEntry);
				if(character != null) characters.Add(character);
			}

			return characters;
		}

		public IEnumerable<string> GetByFilePaths()
		{
			if (Directory.Exists(DnD5eResources.CharacterDataDir) == false)
				return Enumerable.Empty<string>();

			return Directory.GetFiles(DnD5eResources.CharacterDataDir);
		}

		public void Save(IEnumerable<DnD5eCharacter> characters)
		{
			foreach (DnD5eCharacter character in characters)
			{
				ReadWriteJsonFile<DnD5eCharacter>.WriteFile(CharacterTypeHelper.BuildPath(character), character);
			}
		}

		public void Save(DnD5eCharacter character)
		{
			// character data folder does not exist
			if (!Directory.Exists(DnD5eResources.CharacterDataDir))
			{
				Directory.CreateDirectory(DnD5eResources.CharacterDataDir);
			}

			if (character == null) 
				return;

			string[] characterFiles = GetByFilePaths().ToArray();

			if (characterFiles.Any())
			{
				var test = characterFiles[0].Substring(characterFiles[0].LastIndexOf('\\')+1, characterFiles[0].IndexOf("#") - characterFiles[0].LastIndexOf('\\')-1);
			}
			
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

		public bool Delete(DnD5eCharacter character)
		{
			if (File.Exists(CharacterTypeHelper.BuildPath(character)))
			{
				File.Delete(CharacterTypeHelper.BuildPath(character));
				return true;
			}

			return false;
		}
	}
}
