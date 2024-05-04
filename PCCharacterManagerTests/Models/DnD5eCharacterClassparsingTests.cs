using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass()]
	public class DnD5eCharacterClassparsingTests
	{
		DnD5eCharacterClassData[] classes = ReadWriteJsonCollection<DnD5eCharacterClassData>.ReadCollection(DnD5eResources.CharacterClassDataJson).ToArray();
		Ability[] abilities = ReadWriteJsonCollection<Ability>.ReadCollection(DnD5eResources.AbilitiesJson).ToArray();
		Item[] items = ReadWriteJsonCollection<Item>.ReadCollection(DnD5eResources.AllItemsJson).ToArray();

		[TestMethod()]
		public void SavingThrowTest()
		{
			bool anyFailed = false;

			foreach (var characterClass in classes)
			{
				string[] savingThrows = characterClass.SavingThrows;

				foreach (string savingThrow in savingThrows)
				{
					try
					{
						Ability skillOwner = abilities.Where(y => y.Name == savingThrow).First();
						Assert.IsNotNull(skillOwner);
					}
					catch (Exception ex)
					{
						anyFailed = true;
						Trace.WriteLine("Failed Background: " + characterClass.Name);
						Trace.WriteLine("Failed Skill: " + savingThrow);
					}
				}
			}

			Assert.IsFalse(anyFailed);
		}

		[TestMethod()]
		public void PossibleSkillProfsTest()
		{
			bool anyFailed = false;

			foreach (var characterClass in classes)
			{
				string[] skillProfs = characterClass.PossibleSkillProficiences;

				foreach (string skillName in skillProfs)
				{
					try
					{
						Ability skillOwner = abilities.Where(x => x.Skills.Any(y => y.Name == skillName)).First();
						Assert.IsNotNull(skillOwner);
					}
					catch (Exception ex)
					{
						anyFailed = true;
						Trace.WriteLine("Failed Background: " + characterClass.Name);
						Trace.WriteLine("Failed Skill: " + skillName);
					}
				}
			}

			Assert.IsFalse(anyFailed);
		}

		[TestMethod()]
		public void StartEquipmentTest()
		{
			bool anyFailed = false;

			foreach (var characterClass in classes)
			{
				string[] startItems = characterClass.StartEquipment;

				foreach (string itemName in startItems)
				{
					if (itemName == "1 of your choice" 
						|| itemName.Contains("any", StringComparison.OrdinalIgnoreCase))
						continue;

					if (itemName.Contains("^") || itemName.Contains("&"))
					{
						string[] otherItems = itemName.Split(new char[] { '^', '&'});
						foreach (string item in otherItems)
						{
							Regex regex = new Regex("x+[0-9]");
							int index = regex.Match(item).Index != 0 ? regex.Match(item).Index : item.Length;
							anyFailed = Check(anyFailed, characterClass, item.Substring(0, index).Trim());
						}
					}
					else
					{
						Regex regex = new Regex("x+[0-9]");
							int index = regex.Match(itemName).Index != 0 ? regex.Match(itemName).Index : itemName.Length;
							anyFailed = Check(anyFailed, characterClass, itemName.Substring(0, index).Trim());
					}
				}
			}

			Assert.IsFalse(anyFailed);
		}

		private bool Check(bool anyFailed, DnD5eCharacterClassData characterClass, string itemName)
		{
			try
			{
				Item item = items.Where(y => y.Name == itemName).First();
				Assert.IsNotNull(item);
			}
			catch (Exception ex)
			{
				anyFailed = true;
				Trace.WriteLine("Failed Class: " + characterClass.Name);
				Trace.WriteLine("Failed Item: " + itemName);
			}

			return anyFailed;
		}
	}
}
