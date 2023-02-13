﻿using Newtonsoft.Json;
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
		private readonly CharacterStore characterStore;

		public JsonDnD5eCharacterDataService(CharacterStore characterStore)
		{
			this.characterStore = characterStore;
		}

		public void Add(DnD5eCharacter newCharacter)
		{
			Save(newCharacter);
		}

		public IEnumerable<DnD5eCharacter> GetCharacters()
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

		public IEnumerable<string> GetCharacterFilePaths()
		{
			return Directory.GetFiles(DnD5eResources.CharacterDataDir);
		}

		public void Save(IEnumerable<DnD5eCharacter> characters)
		{
			foreach (DnD5eCharacter character in characters)
			{
				ReadWriteJsonFile<DnD5eCharacter>.WriteFile(DnD5eResources.CharacterDataDir + "/" + character.Name + ".json", character);
			}
		}

		public void Save(DnD5eCharacter character)
		{
			// character data folder does not exist
			if (!Directory.Exists(DnD5eResources.CharacterDataDir))
			{
				Directory.CreateDirectory(DnD5eResources.CharacterDataDir);
			}

			if (character == null) return;

			character.DateModified = DateTime.Now.ToString();

			ReadWriteJsonFile<DnD5eCharacter>.WriteFile(DnD5eResources.CharacterDataDir + "/" + character.Name + ".json", character);
		}

		public bool Delete(DnD5eCharacter character)
		{
			if (File.Exists(DnD5eResources.CharacterDataDir + "/" + character.Name + ".json"))
			{
				File.Delete(DnD5eResources.CharacterDataDir + "/" + character.Name + ".json");
				return true;
			}

			return false;
		}
	}
}