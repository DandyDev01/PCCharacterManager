using PCCharacterManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public static class CharacterIDGenerator
	{
		public static string GenerateID()
		{
			string[] characterFilePaths = new JsonCharacterDataService(null).GetCharacterFilePaths().ToArray();
			string[] ids = new string[characterFilePaths.Length];

			for (int i = 0; i < characterFilePaths.Length; i++)
			{
				ids[i] = characterFilePaths[i].Substring(characterFilePaths[i].IndexOf("#"));
			}

			int id;
			do
			{
				id = new Random().Next(1, 10000);
			}
			while (ids.Contains(x => x.Contains(id.ToString())));

			return StringConstants.ID_SEPERATOR + id.ToString();
		}
	}
}
