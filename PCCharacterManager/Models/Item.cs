using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public enum ItemType { Weapon, Armor, Item, Ammunition }

	public class Item : ObservableObject, ICloneable
	{
		private string name;
		private string desc;
		private string cost;
		private string weight;
		private int quantity;

		public string Name
		{
			get { return name; }
			set { OnPropertyChanged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChanged(ref desc, value); }
		}
		public string Cost
		{
			get { return cost; }
			set { OnPropertyChanged(ref cost, value); }
		}
		public string Weight
		{
			get { return weight; }
			set { OnPropertyChanged(ref weight, value); }
		}
		public int Quantity
		{
			get { return quantity; }
			set { OnPropertyChanged(ref quantity, value); }
		}
		public IEnumerable<Property> Properties
		{
			get { return properties; }
		}

		private readonly ObservableCollection<Property> properties;

		[JsonProperty("Tag")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemType Tag { get; set; }

		public Item()
		{
			name = string.Empty;
			desc = string.Empty;
			cost = string.Empty;
			weight = string.Empty;
			quantity = 1;
			properties = new ObservableCollection<Property>();
		}

		private Item(IEnumerable<Property> _properties)
		{
			name = string.Empty;
			desc = string.Empty;
			cost = string.Empty;
			weight = string.Empty;
			quantity = 1;
			properties = new ObservableCollection<Property>();
			foreach (var property in _properties)
			{
				properties.Add((Property)property.Clone());
			}
		}

		/// <summary>
		/// adds a property to an item
		/// </summary>
		/// <param name="property">property to add</param>
		public void AddProperty(Property property)
		{
			properties.Add(property);
		}

		/// <summary>
		/// removes a proeprty from an item
		/// </summary>
		/// <param name="property">property to remove</param>
		public void RemoveProperty(Property property)
		{
			properties.Remove(property);
		}

		public object Clone()
		{
			return new Item(properties)
			{
				name = name,
				desc = desc,
				cost = cost,
				weight = weight,
				quantity = quantity
			};
		}
	}
}
