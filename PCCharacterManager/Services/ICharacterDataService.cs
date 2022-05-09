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
		void Save(IEnumerable<Character> characters);

		void Add(Character newCharacter);
	}
}
