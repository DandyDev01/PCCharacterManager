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
		private readonly CharacterStore characterStore;

		public JsonCharacterDataService(CharacterStore characterStore)
		{
			this.characterStore = characterStore;

			characterStore.CharacterCreate += Add;
		}

		public void Add(Character newCharacter)
		{
			List<Character> characterList = GetCharacters().ToList();
			characterList.Add(newCharacter);
			Save(characterList);
		}

		public IEnumerable<Character> GetCharacters()
		{

			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
			}

			var serializedCharacters = File.ReadAllText(filePath);
			var characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(serializedCharacters);

			if (characters == null)
				return new List<Character>();

			return characters;

		}

		public void Save(IEnumerable<Character> characters)
		{
			ReadWriteJsonCollection<Character>.WriteCollection(filePath, characters);
		}
	}
}
