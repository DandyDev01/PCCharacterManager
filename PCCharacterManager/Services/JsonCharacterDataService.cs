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
		private readonly JsonDnD5eCharacterDataService _dnD5echaracterDataService;
		private readonly JsonStarFinderCharacterDataService _starFinderCharacterDataService;
		private readonly JsonDarkSoulsCharacterDataService _darkSoulsCharacterDataService;

		public JsonCharacterDataService(CharacterStore characterStore)
		{
			_dnD5echaracterDataService = new JsonDnD5eCharacterDataService(characterStore);
			_starFinderCharacterDataService = new JsonStarFinderCharacterDataService();
			_darkSoulsCharacterDataService = new JsonDarkSoulsCharacterDataService();

			characterStore.CharacterCreate += Add;
		}

		public JsonCharacterDataService()
		{
			
		}

		public override void Add(CharacterBase characterToAdd)
		{
			if(characterToAdd is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Add(starfinderCharacter);
			}
			else if (characterToAdd is DarkSoulsCharacter darkSoulsCharacter)
			{
				_darkSoulsCharacterDataService.Add(darkSoulsCharacter);
			}
			else if(characterToAdd is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Add(characterToAdd);
			}
		}

		public override bool Delete(CharacterBase characterToDelete)
		{
			if (characterToDelete is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Delete(starfinderCharacter);
				return true;
			}
			else if (characterToDelete is DarkSoulsCharacter darkSoulsCharacter)
			{
				_darkSoulsCharacterDataService.Delete(darkSoulsCharacter);
			}
			else if (characterToDelete is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Delete(characterToDelete);
				return true;
			}

			return false;
		}

		public override IEnumerable<string> GetCharacterFilePaths()
		{
			List<string> paths = new List<string>();
			paths.AddRange(Directory.GetFiles(DnD5eResources.CharacterDataDir));
			paths.AddRange(Directory.GetFiles(StarfinderResources.CharacterDataDir));
			paths.AddRange(Directory.GetFiles(DarkSoulsResources.CharacterDataDir));

			return paths;
		}

		public override IEnumerable<CharacterBase> GetCharacters()
		{
			List<StarfinderCharacter> starfinderCharacters = new();
			string[] starfinderCharacterEntries = Directory.GetFiles(StarfinderResources.CharacterDataDir);
			foreach (string characterEntry in starfinderCharacterEntries)
			{
				var item = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(characterEntry);
				if (item != null) 
					starfinderCharacters.Add(item);
			}

			List<DarkSoulsCharacter> darkSoulsCharacters = new();
			string[] darkSoulsCharacterEntries = Directory.GetFiles(DarkSoulsResources.CharacterDataDir);
			foreach (string characterEntry in darkSoulsCharacterEntries)
			{
				var character = ReadWriteJsonFile<DarkSoulsCharacter>.ReadFile(characterEntry);
				if (character != null)
					darkSoulsCharacters.Add(character);
			}
			
			List<DnD5eCharacter> dnd5eCharacters = new();
			string[] dnd5eCharacterEntries = Directory.GetFiles(DnD5eResources.CharacterDataDir);
			foreach (string characterEntry in dnd5eCharacterEntries)
			{
				var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterEntry);
				if (character != null) 
					dnd5eCharacters.Add(character);
			}

			List<CharacterBase> allCharacters = new();

			allCharacters.AddRange(dnd5eCharacters);
			allCharacters.AddRange(starfinderCharacters);
			allCharacters.AddRange(darkSoulsCharacters);

			return allCharacters;
		}

		public override void Save(IEnumerable<CharacterBase> characters)
		{
			foreach (var item in characters)
			{
				if (item is StarfinderCharacter starfinderCharacter)
				{
					_starFinderCharacterDataService.Save(starfinderCharacter);
				}
				else if (item is DarkSoulsCharacter darkSoulsCharacter)
				{
					_darkSoulsCharacterDataService.Save(darkSoulsCharacter);
				}
				else if (item is DnD5eCharacter)
				{
					_dnD5echaracterDataService.Save(item);
				}
			}
		}

		public override void Save(CharacterBase character)
		{
			if (character is StarfinderCharacter starfinderCharacter)
			{
				_starFinderCharacterDataService.Save(starfinderCharacter);
			}
			else if (character is DarkSoulsCharacter darkSoulsCharacter)
			{
				_darkSoulsCharacterDataService.Save(darkSoulsCharacter);
			}
			else if (character is DnD5eCharacter)
			{
				_dnD5echaracterDataService.Save(character);
			}

			OnSave?.Invoke();
		}
	}
}
