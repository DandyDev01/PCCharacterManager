using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models.Tests
{
	[TestClass()]
	public class StringFormatterTests
	{
		[TestMethod()]
		public void FindQuantityNegTest()
		{
			int s = StringFormater.FindQuantity("Dexterity x-1");
			Assert.AreEqual(-1, s);
		}
		
		[TestMethod()]
		public void FindQuantityPosTest()
		{
			int s = StringFormater.FindQuantity("Dexterity x1");
			Assert.AreEqual(1, s);
		}

		[TestMethod()]
		public void RemoveQuantityNegTest()
		{
			string s = StringFormater.RemoveQuantity("Dexterity x-1");
			Assert.AreEqual("Dexterity", s);
		}

		[TestMethod()]
		public void RemoveQuantityPosTest()
		{
			string s = StringFormater.RemoveQuantity("Dexterity x1");
			Assert.AreEqual("Dexterity", s);
		}

		[TestMethod()]
		public void FindAllOccurrencesOfCharTest()
		{
			int[] s = StringFormater.FindAllOcurancesOfChar("Dexterity x1", 'x');
			Assert.AreEqual(2, s[0]);
			Assert.AreEqual(10, s[1]);
		}
		
		[TestMethod()]
		public void FindAllOccurrencesOfCharNoneTest()
		{
			int[] s = StringFormater.FindAllOcurancesOfChar("Dexterity x1", 'p');
			Assert.AreEqual(0, s.Length);
		}
	}
}