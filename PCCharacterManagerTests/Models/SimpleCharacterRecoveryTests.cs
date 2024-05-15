using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class SimpleCharacterRecoveryTests
	{

		[TestMethod]
		public void UndoTest()
		{
			DnD5eCharacter character = CharacterCreatorViewModel.CreateRandonCharacter();
			SimpleCharacterRecovery recovery = new();

			character.OnCharacterChangedAction += recovery.RegisterChange;

			string name = character.Name;

			character.Name = "test";

			Assert.AreEqual("test", character.Name);

			character = recovery.Undo();

			Assert.AreEqual(name, character.Name);
		}
	}
}
