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
	public class CreateCharacterViewModelTests
	{

		[TestMethod()]
		public void CreateCharacterTest()
		{
			DialogServiceBase dialogService = new SelectDialogService();
			CharacterCreatorViewModel creator = new(dialogService);

			creator.SelectedCharacterClass = ReadWriteJsonCollection<DnD5eCharacterClassData>
				.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray().GetRandom();

			creator.SelectedBackground = ReadWriteJsonCollection<DnD5eBackgroundData>
				.ReadCollection(DnD5eResources.BackgroundDataJson).ToArray().GetRandom();

			creator.SelectedRace = ReadWriteJsonCollection<DnD5eCharacterRaceData>
				.ReadCollection(DnD5eResources.RaceDataJson).ToArray().GetRandom();
			creator.SelectedRace.RaceVariant = creator.SelectedRace.Variants.ToArray().GetRandom();

			creator.SelectedBackground.OtherProfs = new string[1] { "tool ^ not tool" };
			creator.SelectedBackground.SkillProfs = new string[3] { "Your choice", "Religion ^ Stealth", "Athletics" };
			creator.SelectedClassSkillProfs.SelectedItems.Append("Athletics");
			creator.SelectedRace.AbilityScoreIncreases = new string[1] { "Your Choice" };

			var character = creator.Create();

			Assert.IsNotNull(character);
		}

	}

	/// <summary>
	/// Use this dialog service when a selectStringValueViewModel needs to have a selected item
	/// </summary>
	internal class SelectDialogService : DialogServiceBase
	{
		public override void ShowDialog<TView, TViewModel>(TViewModel dataContext, Action<string> callBack)
		{
			if (typeof(TViewModel) == typeof(DialogWindowSelectStingValueViewModel))
			{
				var d = dataContext as DialogWindowSelectStingValueViewModel;
				d.LimitedMultiSelectVM.Items.ElementAt(0).IsSelected = true;
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
