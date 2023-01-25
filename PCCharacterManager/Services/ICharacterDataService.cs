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
		IEnumerable<DnD5eCharacter> GetCharacters();

		IEnumerable<string> GetCharacterFilePaths();

		void Save(IEnumerable<DnD5eCharacter> characters);

		void Save(DnD5eCharacter character);

		void Add(DnD5eCharacter newCharacter);

		bool Delete(DnD5eCharacter character);
	}
}
