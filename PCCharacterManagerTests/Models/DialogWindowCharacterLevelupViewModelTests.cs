using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class DialogWindowCharacterLevelupViewModelTests
	{
		[TestMethod]
		public void AddClassCharacterClassNameTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			string className = character.CharacterClass.Name;
			vm.AddClassCommand.Execute(character);
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];
			Assert.AreEqual(character.CharacterClass.Name, className + "/" + vm.SelectedCharacterClass.Name + " 0");
		}

		[TestMethod]
		public void AddClassClassesToDisplayTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			Assert.AreEqual(vm.ClassesToDisplay.Count, 1);
			vm.AddClassCommand.Execute(character);
			Assert.AreEqual(vm.ClassesToDisplay.Count, 2);
		}

		[TestMethod]
		public void FeaturesToDisplayTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			var features = vm.SelectedCharacterClass.Features.Where(x => x.Level == vm.SelectedCharacterClass.Level.Level+1).ToArray();

			Assert.AreEqual(features.Length, vm.FeaturesToDisplay.Count);
			for (int i = 0; i < vm.FeaturesToDisplay.Count; i++)
			{
				Assert.IsTrue(features.Contains(x => x.Name == vm.FeaturesToDisplay[i].Name));
			}

			vm.AddClassCommand.Execute(character);
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];

			features = vm.SelectedCharacterClass.Features.Where(x => x.Level == vm.SelectedCharacterClass.Level.Level+1).ToArray();

			Assert.AreEqual(features.Length, vm.FeaturesToDisplay.Count);
			for (int i = 0; i < vm.FeaturesToDisplay.Count; i++)
			{
				Assert.IsTrue(features.Contains(x => x.Name == vm.FeaturesToDisplay[i].Name));
			}

		}

		[TestMethod]
		public void RollForHealthTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			Assert.AreEqual(vm.MaxHealth, character.Health.MaxHealth);
			vm.RollHitdieCommand.Execute(character);
			Assert.IsTrue(vm.MaxHealth > character.Health.MaxHealth);
		}

		[TestMethod]
		public void ProrocessLevelupOnlyOneClassTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			int currentLevel = character.Level.Level;
			vm.ProcessLevelup();
			Assert.AreEqual(currentLevel + 1, character.Level.Level);
		}

		[TestMethod]
		public void ProrocessLevelupMultipleClassesTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);

			vm.AddClassCommand.Execute(character);

			int currentLevel = character.Level.Level;
			vm.ProcessLevelup();
			Assert.AreEqual(currentLevel + 1, character.Level.Level);
		}

		[TestMethod]
		public void LevelupToolProfsCheckTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);
			var multiClassData = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray().Where(x => x.Name 
				== vm.SelectedCharacterClass.Name).First();

			vm.AddClassCommand.Execute(character);

			int currentLevel = character.Level.Level;
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];
			vm.ProcessLevelup();

			for (int i = 0; i < vm.ToolProfsToDisplay.Count; i++)
			{
				if (character.ToolProficiences.Contains(vm.ToolProfsToDisplay[i]) == false)
					Assert.Fail();
			}

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void LevelupWeaponProfsCheckTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);
			var multiClassData = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray().Where(x => x.Name
				== vm.SelectedCharacterClass.Name).First();

			vm.AddClassCommand.Execute(character);

			int currentLevel = character.Level.Level;
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];
			vm.ProcessLevelup();

			for (int i = 0; i < vm.WeaponProfsToDisplay.Count; i++)
			{
				if (character.WeaponProficiencies.Contains(vm.WeaponProfsToDisplay[i]) == false)
					Assert.Fail();
			}

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void LevelupArmorProfsCheckTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);
			var multiClassData = ReadWriteJsonCollection<CharacterMultiClassData>
				.ReadCollection(DnD5eResources.MultiClassDataJson).ToArray().Where(x => x.Name
				== vm.SelectedCharacterClass.Name).First();

			vm.AddClassCommand.Execute(character);

			int currentLevel = character.Level.Level;
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];
			vm.ProcessLevelup();

			for (int i = 0; i < vm.ToolProfsToDisplay.Count; i++)
			{
				if (character.ArmorProficiencies.Contains(vm.ArmorProfsToDisplay[i]) == false)
					Assert.Fail();
			}

			Assert.IsTrue(true);
		}


		[TestMethod]
		public void LeveupNewClassFeaturesCheckTest()
		{
			var dialogService = new SelectDialogService();
			var character = CharacterCreatorViewModel.CreateRamdonCharacter();
			var vm = new DialogWindowDnD5eCharacterLevelupViewModel(dialogService, character);
			var classData = ReadWriteJsonCollection<DnD5eCharacterClassData>
				.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray().Where(x => x.Name
				== vm.SelectedCharacterClass.Name).First();

			vm.AddClassCommand.Execute(character);

			int currentLevel = character.Level.Level;
			vm.SelectedCharacterClass = vm.ClassesToDisplay[1];
			vm.ProcessLevelup();

			for (int i = 0; i < vm.FeaturesToDisplay.Count; i++)
			{
				if (character.CharacterClass.Features.Contains(x => x.Name == vm.FeaturesToDisplay[i].Name) == false)
					Assert.Fail();
			}

			Assert.IsTrue(true);
		}

	}
}
