using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public interface ISearch<T>
	{
		public IEnumerable<T> Search(string searchTerm, IEnumerable<T> itemsToSearch);
	}
}
