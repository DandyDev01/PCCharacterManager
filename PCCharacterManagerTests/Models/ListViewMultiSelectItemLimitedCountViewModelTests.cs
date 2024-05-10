using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{
	[TestClass]
	public class ListViewMultiSelectItemLimitedCountViewModelTests
	{
		[TestMethod]
		public void EmptyOptionsTest()
		{
			List<string> options = new List<string>();
			int amountToSelect = 1;
			var list = new ListViewMultiSelectItemsLimitedCountViewModel(amountToSelect, options);

			Assert.AreEqual(list.Items.Count, 0);
		}

		[TestMethod]
		public void CorrectInitTest()
		{
			List<string> options = new List<string>()
			{
				"test", "test 1", "test 2", "test 3"
			};
			int amountToSelect = 1;
			var list = new ListViewMultiSelectItemsLimitedCountViewModel(amountToSelect, options);


			foreach ( var item in list.Items)
			{
				Assert.IsTrue(options.Contains(item.BoundItem));
			}

			Assert.AreEqual(amountToSelect, list.AmountToBeSelected);
		}

		[TestMethod]
		public void RepopulateTest()
		{
			List<string> options = new List<string>()
			{
				"test", "test 1", "test 2", "test 3"
			};
			List<string> moreOptions = new List<string>()
			{
				"take", "take 1", "take 2"
			};
			int amountToSelect = 1;
			var list = new ListViewMultiSelectItemsLimitedCountViewModel(amountToSelect, options);

			list.PopulateItems(1, moreOptions);

			foreach (var item in list.Items)
			{
				Assert.IsTrue(options.Contains(item.BoundItem) == false);
			}

			foreach (var item in list.Items)
			{
				Assert.IsTrue(moreOptions.Contains(item.BoundItem));
			}
		}

		[TestMethod]
		public void SelectItemCapTest()
		{
			List<string> options = new List<string>()
			{
				"test", "test 1", "test 2", "test 3"
			};
			int amountToSelect = 1;
			var list = new ListViewMultiSelectItemsLimitedCountViewModel(amountToSelect, options);

			foreach (var item in list.Items)
			{
				item.Toggle();
			}

			Assert.AreEqual(amountToSelect, list.SelectedItems.Count());
			Assert.AreEqual(amountToSelect, list.AmountSelected);
		}

		[TestMethod]
		public void RemoveSelectedItemTest()
		{
			List<string> options = new List<string>()
			{
				"test", "test 1", "test 2", "test 3"
			};
			int amountToSelect = 2;
			var list = new ListViewMultiSelectItemsLimitedCountViewModel(amountToSelect, options);

			foreach (var item in list.Items)
			{
				item.Toggle();
			}

			list.Items.Where(x => x.IsSelected).First().Toggle();

			Assert.AreEqual(amountToSelect - 1, list.SelectedItems.Count());
			Assert.AreEqual(amountToSelect - 1, list.AmountSelected);
		}

	}
}
