using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class CharacterListViewModelTests
	{

		[TestMethod]
		public void DeleteCharacterTest()
		{
			var recovery = new SimpleCharacterRecovery();
			var dialogService = new PassDialogService();
			var characterStore = new CharacterStore(recovery);
			var dataService = new MockCharacterDataService();
			CharacterListViewModel vm = new(characterStore, dataService, dialogService);

			var character = CharacterCreatorViewModel.CreateRandonCharacter();
			characterStore.CreateCharacter(character);

			Assert.IsTrue(vm.CharacterItems.Count == 2);
			vm.DeleteCharacterCommand.Execute(character);
			Assert.IsTrue(vm.CharacterItems.Count == 1);
		}

		[TestMethod]
		public void LoadCharacterTest()
		{
			var recovery = new SimpleCharacterRecovery();
			var dialogService = new PassDialogService();
			var characterStore = new CharacterStore(recovery);
			var dataService = new MockCharacterDataService();
			CharacterListViewModel vm = new(characterStore, dataService, dialogService);

			var character = CharacterCreatorViewModel.CreateRandonCharacter();
			characterStore.CreateCharacter(character);

			Assert.IsTrue(vm.CharacterItems.Any());
		}

		[TestMethod]
		public void SaveCharacterTest()
		{
			//var dialogService = new PassDialogService();
			//var characterStore = new CharacterStore();
			//var dataService = new MockCharacterDataService();
			//CharacterListViewModel vm = new(characterStore, dataService, dialogService);

			//characterStore.BindSelectedCharacter(CharacterCreatorViewModel.CreateRamdonCharacter());

			//string createTime = characterStore.SelectedCharacter.DateModified;

			//vm.SaveCharacter();

			//Assert.IsTrue(characterStore.SelectedCharacter.DateModified.Equals(createTime) == false);

		}

	}

	internal class MockCharacterDataService : ICharacterDataService
	{
		public List<CharacterBase> characters = new List<CharacterBase>();
		private List<CharacterBase> _saved = new List<CharacterBase>();

		public override void Add(CharacterBase newCharacter)
		{
			characters.Add(newCharacter);
		}

		public override bool Delete(CharacterBase character)
		{
			return _saved.Remove(character);
		}

		public override IEnumerable<string> GetCharacterFilePaths()
		{
			string[] results= new string[characters.Count];
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = characters[i].Name;
			}

			return results;
		}

		public override IEnumerable<CharacterBase> GetCharacters()
		{
			if (_saved.Any() == false)
				Save(new DnD5eCharacter());

			return _saved;
		}

		public override void Save(IEnumerable<CharacterBase> characters)
		{
			_saved.AddRange(characters);
		}

		public override void Save(CharacterBase character)
		{
			_saved.Add(character);
			characters.Add(character);
		}
	}
}
