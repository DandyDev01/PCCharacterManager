using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Utilitys
	{
		public static T GetPropertyValue<T>(object src, string propName)
		{
			return (T)src.GetType().GetProperty(propName).GetValue(src, null);
		}

		public static T[] GetPropertyValue<T>(object[] src, string propName)
		{
			T[] result = new T[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				result[i] = (T)src[i].GetType().GetProperty(propName).GetValue(src[i], null);
			}

			return result;
		}

	}
}
