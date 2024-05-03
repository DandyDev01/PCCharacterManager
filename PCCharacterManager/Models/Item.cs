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
		Shield, LightArmor, MediumArmor, HeavyArmor,
		Potion, Poison, Ingredient, Component,
		Misc, Book, Scroll, Note, Map, Tool
	}

	public class Item : ObservableObject
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set { OnPropertyChanged(ref _name, value); }
		}

		private string _desc;
		public string Desc
		{
			get { return _desc; }
			set { OnPropertyChanged(ref _desc, value); }
		}
		
		private string _cost;
		public string Cost
		{
			get { return _cost; }
			set { OnPropertyChanged(ref _cost, value); }
		}
		
		private string _weight;
		public string Weight
		{
			get { return _weight; }
			set { OnPropertyChanged(ref _weight, value); }
		}
		
		private int _quantity;
		public int Quantity
		{
			get { return _quantity; }
			set { OnPropertyChanged(ref _quantity, value); }
		}
		
		private readonly ObservableCollection<Property> _properties;
		public IEnumerable<Property> Properties
		{
			get { return _properties; }
		}

		private ItemCategory _category;
		private ItemType type;

		[JsonProperty("Category")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ItemCategory Category 
		{
			get => _category; 
			set => OnPropertyChanged(ref _category, value); 
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
			_name = string.Empty;
			_desc = string.Empty;
			_cost = string.Empty;
			_weight = string.Empty;
			_quantity = 1;
			type = ItemType.Sword;
			_category = ItemCategory.Weapon;
			_properties = new ObservableCollection<Property>();
		}

		/// <summary>
		/// adds a property to an item
		/// </summary>
		/// <param name="property">property to add</param>
		public void AddProperty(Property property)
		{
			_properties.Add(property);
		}

		/// <summary>
		/// removes a proeprty from an item
		/// </summary>
		/// <param name="property">property to remove</param>
		public void RemoveProperty(Property property)
		{
			_properties.Remove(property);
		}
	}
}
