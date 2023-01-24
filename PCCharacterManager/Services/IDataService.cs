using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Services
{
	public interface IDataService<T>
	{
		IEnumerable<T> GetItems();

		IEnumerable<string> GetByFilePaths();

		void Save(IEnumerable<T> items);

		void Save(T item);

		void Add(T item);

		bool Delete(T item);
	}
}
