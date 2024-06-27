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

		public JsonCharacterDataService(CharacterStore? characterStore)
		{
			_dnD5echaracterDataService = new JsonDnD5eCharacterDataService();
			_starFinderCharacterDataService = new JsonStarFinderCharacterDataService();
			_darkSoulsCharacterDataService = new JsonDarkSoulsCharacterDataService();

			if (characterStore != null ) 
				characterStore.CharacterCreate += Add;
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
			else if(characterToAdd is DnD5eCharacter dnd5e)
			{
				_dnD5echaracterDataService.Add(dnd5e);
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
			else if (characterToDelete is DnD5eCharacter dnd5e)
			{
				_dnD5echaracterDataService.Delete(dnd5e);
				return true;
			}

			return false;
		}

		public override IEnumerable<string> GetCharacterFilePaths()
		{
			List<string> paths = new List<string>();
			paths.AddRange(_dnD5echaracterDataService.GetByFilePaths());
			paths.AddRange(_starFinderCharacterDataService.GetByFilePaths());
			paths.AddRange(_darkSoulsCharacterDataService.GetByFilePaths());

			return paths;
		}

		public override IEnumerable<CharacterBase> GetCharacters()
		{
			List<StarfinderCharacter> starfinderCharacters = new();
			List<DarkSoulsCharacter> darkSoulsCharacters = new();
			List<DnD5eCharacter> dnd5eCharacters = new();
			
			if (Directory.Exists(StarfinderResources.CharacterDataDir))
			{
				string[] starfinderCharacterEntries = Directory.GetFiles(StarfinderResources.CharacterDataDir);
				foreach (string characterEntry in starfinderCharacterEntries)
				{
					var item = ReadWriteJsonFile<StarfinderCharacter>.ReadFile(characterEntry);
					if (item != null) 
						starfinderCharacters.Add(item);
				}
			}

			if (Directory.Exists(DarkSoulsResources.CharacterDataDir))
			{
				string[] darkSoulsCharacterEntries = Directory.GetFiles(DarkSoulsResources.CharacterDataDir);
				foreach (string characterEntry in darkSoulsCharacterEntries)
				{
					var character = ReadWriteJsonFile<DarkSoulsCharacter>.ReadFile(characterEntry);
					if (character != null)
						darkSoulsCharacters.Add(character);
				}
			}
			
			if (Directory.Exists(DnD5eResources.CharacterDataDir))
			{
				string[] dnd5eCharacterEntries = Directory.GetFiles(DnD5eResources.CharacterDataDir);
				foreach (string characterEntry in dnd5eCharacterEntries)
				{
					var character = ReadWriteJsonFile<DnD5eCharacter>.ReadFile(characterEntry);
					if (character != null) 
						dnd5eCharacters.Add(character);
				}
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
				Save(item);
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
			else if (character is DnD5eCharacter dnd5e)
			{
				_dnD5echaracterDataService.Save(dnd5e);
			}

			OnSave?.Invoke();
		}
	}
}
