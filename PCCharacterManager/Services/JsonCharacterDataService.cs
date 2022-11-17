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
	public class JsonCharacterDataService : ICharacterDataService
	{
		private readonly CharacterStore characterStore;

		public JsonCharacterDataService(CharacterStore characterStore)
		{
			this.characterStore = characterStore;

			characterStore.CharacterCreate += Add;
		}

		public void Add(Character newCharacter)
		{
			Save(newCharacter);
		}

		public IEnumerable<Character> GetCharacters()
		{
			List<Character> characters = new List<Character>();	
			string[] characterEntries = Directory.GetFiles(Resources.CharacterDataDir);
			foreach (string characterEntry in characterEntries)
			{
				var character = ReadWriteJsonFile<Character>.ReadFile(characterEntry);
				if(character != null) characters.Add(character);
			}

			return characters;
		}

		public IEnumerable<string> GetCharacterFilePaths()
		{
			return Directory.GetFiles(Resources.CharacterDataDir);
		}

		public void Save(IEnumerable<Character> characters)
		{
			foreach (Character character in characters)
			{
				ReadWriteJsonFile<Character>.WriteFile(Resources.CharacterDataDir + "/" + character.Name + ".json", character);
			}
		}

		public void Save(Character character)
		{
			// character data folder does not exist
			if (!Directory.Exists(Resources.CharacterDataDir))
			{
				Directory.CreateDirectory(Resources.CharacterDataDir);
			}

			ReadWriteJsonFile<Character>.WriteFile(Resources.CharacterDataDir + "/" + character.Name + ".json", character);
		}

		public bool Delete(Character character)
		{
			if (File.Exists(Resources.CharacterDataDir + "/" + character.Name + ".json"))
			{
				File.Delete(Resources.CharacterDataDir + "/" + character.Name + ".json");
				return true;
			}

			return false;
		}
	}
}
