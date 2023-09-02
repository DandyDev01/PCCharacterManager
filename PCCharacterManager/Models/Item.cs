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
	public enum ItemCategory { Weapon, Armor, Item, Ammunition }
	public enum ItemType 
	{
		Sword, GreatSword, Axe, GreatAxe, Hammer, GreatHammer, Mace, GreatMace, Spear, Bow, Firearm, Versatile,
		Shild, LightArmor, MediumnArmor, HeavyArmor,
		Potion, Poision, Ingredient, Component,
		Misc, Book, Scroll, Note, Map, Tool
	}

	public class Item : ObservableObject
	{
		private string name;
		public string Name
		{
			get { return name; }
			set { OnPropertyChanged(ref name, value); }
		}

		private string desc;
		public string Desc
		{
			get { return desc; }
			set { OnPropertyChanged(ref desc, value); }
		}
		
		private string cost;
		public string Cost
		{
			get { return cost; }
			set { OnPropertyChanged(ref cost, value); }
		}
		
		private string weight;
		public string Weight
		{
			get { return weight; }
			set { OnPropertyChanged(ref weight, value); }
		}
		
		private int quantity;
		public int Quantity
		{
			get { return quantity; }
			set { OnPropertyChanged(ref quantity, value); }
		}
		
		private readonly ObservableCollection<Property> properties;
		public IEnumerable<Property> Properties
		{
			get { return properties; }
		}

		private ItemCategory category;
		private ItemType type;

		[JsonProperty("Category")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemCategory Category 
		{
			get => category; 
			set => OnPropertyChanged(ref category, value); 
		}

		[JsonProperty("Type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemType Type 
		{ 
			get => type;
			set => OnPropertyChanged(ref type, value); 
		}

		public Item()
		{
			name = string.Empty;
			desc = string.Empty;
			cost = string.Empty;
			weight = string.Empty;
			quantity = 1;
			type = ItemType.Sword;
			category = ItemCategory.Weapon;
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
