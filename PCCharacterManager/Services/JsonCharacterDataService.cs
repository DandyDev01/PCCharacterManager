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
		private readonly string filePath = @"Resources\characterdata.json";
		private readonly string characterDataFolder = @"Resources\CharacterData";
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
			string[] characterEntries = Directory.GetFiles(characterDataFolder);
			foreach (string characterEntry in characterEntries)
			{
				var character = ReadWriteJsonFile<Character>.ReadFile(characterEntry);
				if(character != null) characters.Add(character);
			}

			return characters;
		}

		public void Save(IEnumerable<Character> characters)
		{
			ReadWriteJsonCollection<Character>.WriteCollection(filePath, characters);
		}

		public void Save(Character character)
		{
			// character data folder does not exist
			if (!Directory.Exists(characterDataFolder))
			{
				Directory.CreateDirectory(characterDataFolder);
			}

			ReadWriteJsonFile<Character>.WriteFile(characterDataFolder + "/" + character.Name + ".json", character);
		}

		public bool Delete(Character character)
		{
			if (File.Exists(characterDataFolder + "/" + character.Name + ".json"))
			{
				File.Delete(characterDataFolder + "/" + character.Name + ".json");
				return true;
			}

			return false;
		}
	}
}
