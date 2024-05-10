using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.ViewModels;
using PCCharacterManager.ViewModels.DialogWindowViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class CharacterInfoViewModelTests
	{

		[TestMethod]
		public void EndEncouterTest()
		{
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new PassDialogService();
			CharacterInfoViewModel vm = new(store, service);

			vm.EndEncounterCommand.Execute(this);

			Assert.IsTrue(vm.SelectedCharacter.Status == PCCharacterManager.Models.CharacterStatus.IDLE);
			Assert.IsTrue(vm.SelectedCharacter.IsInCombat == false);
		}

		[TestMethod]
		public void StartEncouterTest()
		{
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new PassDialogService();
			CharacterInfoViewModel vm = new(store, service);

			vm.StartEncounterCommand.Execute(this);

			Assert.IsTrue(vm.SelectedCharacter.Status == PCCharacterManager.Models.CharacterStatus.COMBAT);
			Assert.IsTrue(vm.SelectedCharacter.IsInCombat);
		}

		[TestMethod]
		public void NextComabtRoundEncouterTest()
		{
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new PassDialogService();
			CharacterInfoViewModel vm = new(store, service);

			vm.SelectedCharacter.Conditions.Add(new PCCharacterManager.Models.Condition("", "", 1));
			vm.NextCombatRoundCommand.Execute(this);

			Assert.IsTrue(vm.SelectedCharacter.CombatRound == 1);
			Assert.IsTrue(vm.SelectedCharacter.Conditions.Count == 0);
		}

		[TestMethod]
		public void AddToCurrentHealthTest()
		{
			int amount = 10;
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new ChangeHealthDialogService(amount, false);
			CharacterInfoViewModel vm = new(store, service);

			vm.SelectedCharacter.Health.CurrHealth = 0;
			vm.SelectedCharacter.Health.MaxHealth = 20;
			int currentHealth = vm.SelectedCharacter.Health.CurrHealth;
			vm.AdjustHealthCommand.Execute(this);

			Assert.IsTrue(currentHealth + amount == vm.SelectedCharacter.Health.CurrHealth);
		}

		[TestMethod]
		public void RemoveFromCurrentHealthTest()
		{
			int amount = -10;
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new ChangeHealthDialogService(amount, false);
			CharacterInfoViewModel vm = new(store, service);

			vm.SelectedCharacter.Health.CurrHealth = 20;
			vm.SelectedCharacter.Health.MaxHealth = 20;
			int currentHealth = vm.SelectedCharacter.Health.CurrHealth;
			vm.AdjustHealthCommand.Execute(this);

			Assert.IsTrue(currentHealth + amount == vm.SelectedCharacter.Health.CurrHealth);
		}

		[TestMethod]
		public void AddToTempHealthTest()
		{
			int amount = 10;
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new ChangeHealthDialogService(amount, true);
			CharacterInfoViewModel vm = new(store, service);

			vm.SelectedCharacter.Health.TempHitPoints = 0;
			int currentTemp = vm.SelectedCharacter.Health.TempHitPoints;
			vm.AdjustHealthCommand.Execute(this);

			Assert.IsTrue(currentTemp + amount == vm.SelectedCharacter.Health.TempHitPoints);
		}

		[TestMethod]
		public void RemoveFromTempHealthTest()
		{
			int amount = -10;
			CharacterStore store = new CharacterStore();
			DialogServiceBase service = new ChangeHealthDialogService(amount, true);
			CharacterInfoViewModel vm = new(store, service);

			vm.SelectedCharacter.Health.TempHitPoints = 20;
			int currentTemp = vm.SelectedCharacter.Health.TempHitPoints;
			vm.AdjustHealthCommand.Execute(this);

			Assert.IsTrue(currentTemp + amount == vm.SelectedCharacter.Health.TempHitPoints);
		}

	}

	internal class ChangeHealthDialogService : DialogServiceBase
	{
		private int _changeAmount;
		private bool _tempHealth;
		internal ChangeHealthDialogService(int changeAmount, bool tempHealth)
		{
			_changeAmount = changeAmount;
			_tempHealth	= tempHealth;
		}

		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			if (typeof(TViewModel) == typeof(DialogWindowChangeHealthViewModel))
			{
				var d = dataContext as DialogWindowChangeHealthViewModel;
				d.IsTempHealth = _tempHealth;
				d.Amount = _changeAmount;
			}
			EventHandler closeEventhandler = null;
			closeEventhandler = (s, e) =>
			{
				callBack(true.ToString());
			};
		}

		public override MessageBoxResult ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			return MessageBoxResult.OK;
		}
	}
}
