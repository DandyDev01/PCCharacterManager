using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class DnD5eCharacterDialogSeriseLevelerTests
	{
		[TestMethod()]
		public void CancelLevelupTest()
		{
			 DialogServiceBase dialogService = new CancelDialogService();
			 CharacterLeveler dndCharacterLeveler = new DnD5eDialogStreamCharacterLeveler(dialogService);

			Assert.IsFalse(dndCharacterLeveler.LevelCharacter(CharacterCreatorViewModel.CreateRamdonCharacter()));
		}

		[TestMethod()]
		public void AddClassTest()
		{
			DialogServiceBase dialogService = new PassDialogService();
			CharacterLeveler dndCharacterLeveler = new DnD5eDialogStreamCharacterLeveler(dialogService);

			Assert.IsTrue(dndCharacterLeveler.LevelCharacter(CharacterCreatorViewModel.CreateRamdonCharacter()));
		}

		[TestMethod()]
		public void DontAddClassTest()
		{
			DialogServiceBase dialogService = new PassDialogNoMessageDialogService();
			CharacterLeveler dndCharacterLeveler = new DnD5eDialogStreamCharacterLeveler(dialogService);

			Assert.IsTrue(dndCharacterLeveler.LevelCharacter(CharacterCreatorViewModel.CreateRamdonCharacter()));
		}

		[TestMethod()]
		public void NullCharacterLevelupTest()
		{
			DialogServiceBase dialogService = new CancelDialogService();
			CharacterLeveler dndCharacterLeveler = new DnD5eDialogStreamCharacterLeveler(dialogService);

			try {
				dndCharacterLeveler.LevelCharacter(null);
				Assert.Fail();
			}
			catch (NullReferenceException ex)
			{
				Assert.IsTrue(true);
				return;
			}

			Assert.Fail();
		}
	}

	internal class CancelDialogService : DialogServiceBase
	{
		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			EventHandler closeEventhandler = null;
			closeEventhandler = (s, e) =>
			{
				callBack(false.ToString());
			};
		}

		public override MessageBoxResult ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			return MessageBoxResult.Cancel;
		}
	}

	internal class PassDialogService : DialogServiceBase
	{
		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
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

	internal class PassDialogNoMessageDialogService : DialogServiceBase
	{
		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			EventHandler closeEventhandler = null;
			closeEventhandler = (s, e) =>
			{
				callBack(true.ToString());
			};
		}

		public override MessageBoxResult ShowMessage(string message, string caption, MessageBoxButton button, MessageBoxImage image)
		{
			return MessageBoxResult.No;
		}
	}
}
