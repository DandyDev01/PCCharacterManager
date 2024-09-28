using System.Collections.Generic;

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
