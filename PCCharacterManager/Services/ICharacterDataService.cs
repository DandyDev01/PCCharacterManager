using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public interface ICharacterDataService
	{
		IEnumerable<Character> GetCharacters();

		IEnumerable<string> GetCharacterFilePaths();

		void Save(IEnumerable<Character> characters);

		void Save(Character character);

		void Add(Character newCharacter);

		bool Delete(Character character);
	}
}
