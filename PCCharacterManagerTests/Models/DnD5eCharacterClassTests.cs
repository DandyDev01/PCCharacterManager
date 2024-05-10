using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class DnD5eCharacterClassTests
	{
		[TestMethod]
		public void UpdateCharacterClassNameTest()
		{
			var characterClass = new DnD5eCharacterClass();
			characterClass.Name = "test 1";

			characterClass.UpdateCharacterClassName("test", 1);

			Assert.AreEqual(characterClass.Name, "test 2");
		}

		[TestMethod]
		public void UpdateCharacterClassNameTest1()
		{
			string name = "test 1 / the 3";
			var characterClass = new DnD5eCharacterClass();
			characterClass.Name = name;

			characterClass.UpdateCharacterClassName("test", 1);

			Assert.AreEqual(characterClass.Name, "test 2 / the 3");
		}

		[TestMethod]
		public void GetNamesAndLevels()
		{
			var characterClass = new DnD5eCharacterClass();
			characterClass.Name = "test 1";

			var results = characterClass.GetClassNamesAndLevels();

			Assert.AreEqual(results[0].Key, "test");
			Assert.AreEqual(results[0].Value, 1);
		}

		[TestMethod]
		public void GetNamesAndLevels1()
		{
			var characterClass = new DnD5eCharacterClass();
			characterClass.Name = "test 1 / the 3";

			var results = characterClass.GetClassNamesAndLevels();

			Assert.AreEqual(results[0].Key, "test");
			Assert.AreEqual(results[0].Value, 1);

			Assert.AreEqual(results[1].Key, "the");
			Assert.AreEqual(results[1].Value, 3);
		}

	}
}
