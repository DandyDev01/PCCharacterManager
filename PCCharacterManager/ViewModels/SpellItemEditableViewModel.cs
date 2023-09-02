using PCCharacterManager.Models;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class SpellItemEditableViewModel : ObservableObject
	{
		private Spell spell;
		public Spell Spell
		{
			get { return spell; }
			set { OnPropertyChanged(ref spell, value); }
		}

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				OnPropertyChanged(ref name, value);
				spell.Name = value;
			}
		}

		private string desc;
		public string Desc
		{
			get
			{
				return desc;
			}
			set
			{
				OnPropertyChanged(ref desc, value);
				spell.Desc = value;
			}
		}

		private string level;
		public string Level
		{
			get
			{
				return level;
			}
			set
			{
				OnPropertyChanged(ref level, value);
				spell.Level = value;
			}
		}

		private string castingTime;
		public string CastingTime
		{
			get
			{
				return castingTime;
			}
			set
			{
				OnPropertyChanged(ref castingTime, value);
				spell.CastingTime = value;	
			}
		}

		private string range_area;
		public string Range_Area
		{
			get
			{
				return range_area;
			}
			set
			{
				OnPropertyChanged(ref range_area, value);
				spell.Range_Area = value;
			}
		}

		private string damage_effect;
		public string Damage_Effect
		{
			get
			{
				return damage_effect;
			}
			set
			{
				OnPropertyChanged(ref damage_effect, value);
				spell.Damage_Effect = value;
			}
		}

		private string attack_save;
		public string Attack_Save
		{
			get
			{
				return attack_save;
			}
			set
			{
				OnPropertyChanged(ref attack_save, value);
				spell.Attack_Save = value;
			}
		}

		private string duration;
		public string Duration
		{
			get
			{
				return duration;
			}
			set
			{
				OnPropertyChanged(ref duration, value);
				spell.Duration = value;
			}
		}

		private string stringComponents;
		public string StringComponents
		{
			get
			{
				return stringComponents;
			}
			set
			{
				OnPropertyChanged(ref stringComponents, value);
			}
		}

		private SpellSchool school;
		public SpellSchool School
		{
			get
			{
				return school;
			}
			set
			{
				OnPropertyChanged(ref school, value);
				spell.School = value;
			}
		}

		private bool isPrepared;
		public bool IsPrepared
		{
			get { return isPrepared; }
			set 
			{ 
				OnPropertyChanged(ref isPrepared, value);
				spell.IsPrepared = value;
			}
		}

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			private set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChanged(nameof(IsDisplayMode));
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		public ObservableCollection<char> Components { get; }

		public ICommand PrepareCommand { get; set; }
		public Action<Spell>? Prepare;

		public SpellItemEditableViewModel(Spell _spell)
		{
			spell = _spell;

			Components = new(spell.Components);

			PrepareCommand = new RelayCommand(InvokePrepare);
			isEditMode = false;

			name = spell.Name;
			desc = spell.Desc;
			level = spell.Level;
			stringComponents = spell.StringComponents;
			castingTime = spell.CastingTime;
			range_area = spell.Range_Area;
			damage_effect = spell.Damage_Effect;
			attack_save = spell.Attack_Save;
			duration = spell.Duration;
			school = spell.School;
			isPrepared = spell.IsPrepared;
		}

		private void InvokePrepare()
		{
			IsPrepared = !IsPrepared;
			Prepare?.Invoke(spell);
		}

		public void Edit()
		{
			IsEditMode = !isEditMode;
		}
	}
}
