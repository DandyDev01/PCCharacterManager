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
	public class JsonDnD5eCharacterDataService : ICharacterDataService
	{
		private readonly CharacterStore _characterStore;

		public JsonDnD5eCharacterDataService(CharacterStore characterStore)
		{
			_characterStore = characterStore;
		}

		public override void Add(CharacterBase newCharacter)
		{
			Save(newCharacter);
		}

		public override IEnumerable<CharacterBase> GetCharacters()
		{
			List<DnD5eCharacter> characters = new List<DnD5eCharacter>();	
			string[] characterEntries = Directory.GetFiles(DnD5eResources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterEntry);
				if(character != null) characters.Add(character);
			}

			return characters;
		}

		public override IEnumerable<string> GetCharacterFilePaths()
		{
			return Directory.GetFiles(DnD5eResources.CharacterDataDir);
		}

		public override void Save(IEnumerable<CharacterBase> characters)
		{
			foreach (DnD5eCharacter character in characters)
			{
				ReadWriteJsonFile<DnD5eCharacter>.WriteFile(CharacterTypeHelper.BuildPath(character), character);
			}
		}

		public override void Save(CharacterBase character)
		{
			// character data folder does not exist
			if (!Directory.Exists(DnD5eResources.CharacterDataDir))
			{
				Directory.CreateDirectory(DnD5eResources.CharacterDataDir);
			}

			if (character == null) 
				return;

			string[] characterFiles = GetCharacterFilePaths().ToArray();

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

			ReadWriteJsonFile<CharacterBase>.WriteFile(CharacterTypeHelper.BuildPath(character), character);
		}

		public override bool Delete(CharacterBase character)
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
