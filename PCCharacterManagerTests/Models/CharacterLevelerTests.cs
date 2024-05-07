using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManagerTests.Models
{

	[TestClass()]
	public class CharacterLevelerTests
	{
		[TestMethod()]
		public void CancelLevelupTest()
		{
			 DialogServiceBase dialogService = new DialogService();
			 CharacterLeveler dndCharacterLeveler = new DnD5eDialogStreamCharacterLeveler(dialogService);


		}

	}
}
