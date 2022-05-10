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

	public class Item : ObservableObject
	{
		private string name;
		private string desc;
		private string cost;
		private string weight;
		private int quantity;

		public string Name
		{
			get { return name; }
			set { OnPropertyChaged(ref name, value); }
		}
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChaged(ref desc, value); }
		}
		public string Cost
		{
			get { return cost; }
			set { OnPropertyChaged(ref cost, value); }
		}
		public string Weight
		{
			get { return weight; }
			set { OnPropertyChaged(ref weight, value); }
		}
		public int Quantity
		{
			get { return quantity; }
			set { OnPropertyChaged(ref quantity, value); }
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
	}
}
