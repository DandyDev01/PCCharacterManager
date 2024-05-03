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
		private Spell _spell;
		public Spell Spell
		{
			get { return _spell; }
			set { OnPropertyChanged(ref _spell, value); }
		}

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				OnPropertyChanged(ref _name, value);
				_spell.Name = value;
			}
		}

		private string _desc;
		public string Desc
		{
			get
			{
				return _desc;
			}
			set
			{
				OnPropertyChanged(ref _desc, value);
				_spell.Desc = value;
			}
		}

		private string _level;
		public string Level
		{
			get
			{
				return _level;
			}
			set
			{
				OnPropertyChanged(ref _level, value);
				_spell.Level = value;
			}
		}

		private string _castingTime;
		public string CastingTime
		{
			get
			{
				return _castingTime;
			}
			set
			{
				OnPropertyChanged(ref _castingTime, value);
				_spell.CastingTime = value;	
			}
		}

		private string _range_area;
		public string Range_Area
		{
			get
			{
				return _range_area;
			}
			set
			{
				OnPropertyChanged(ref _range_area, value);
				_spell.Range_Area = value;
			}
		}

		private string _damage_effect;
		public string Damage_Effect
		{
			get
			{
				return _damage_effect;
			}
			set
			{
				OnPropertyChanged(ref _damage_effect, value);
				_spell.Damage_Effect = value;
			}
		}

		private string _attack_save;
		public string Attack_Save
		{
			get
			{
				return _attack_save;
			}
			set
			{
				OnPropertyChanged(ref _attack_save, value);
				_spell.Attack_Save = value;
			}
		}

		private string _duration;
		public string Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				OnPropertyChanged(ref _duration, value);
				_spell.Duration = value;
			}
		}

		private string _stringComponents;
		public string StringComponents
		{
			get
			{
				return _stringComponents;
			}
			set
			{
				OnPropertyChanged(ref _stringComponents, value);
			}
		}

		private SpellSchool _school;
		public SpellSchool School
		{
			get
			{
				return _school;
			}
			set
			{
				OnPropertyChanged(ref _school, value);
				_spell.School = value;
			}
		}

		private bool _isPrepared;
		public bool IsPrepared
		{
			get { return _isPrepared; }
			set 
			{ 
				OnPropertyChanged(ref _isPrepared, value);
				_spell.IsPrepared = value;
			}
		}

		private bool _isEditMode;
		public bool IsEditMode
		{
			get { return _isEditMode; }
			private set
			{
				OnPropertyChanged(ref _isEditMode, value);
				OnPropertyChanged(nameof(IsDisplayMode));
			}
		}

		public bool IsDisplayMode
		{
			get { return !_isEditMode; }
		}

		public ObservableCollection<char> Components { get; }

		public ICommand PrepareCommand { get; set; }
		public Action<Spell>? Prepare;

		public SpellItemEditableViewModel(Spell spell)
		{
			_spell = spell;

			Components = new(this._spell.Components);

			PrepareCommand = new RelayCommand(InvokePrepare);
			_isEditMode = false;

			_name = _spell.Name;
			_desc = _spell.Desc;
			_level = _spell.Level;
			_stringComponents = _spell.StringComponents;
			_castingTime = _spell.CastingTime;
			_range_area = _spell.Range_Area;
			_damage_effect = _spell.Damage_Effect;
			_attack_save = _spell.Attack_Save;
			_duration = _spell.Duration;
			_school = _spell.School;
			_isPrepared = _spell.IsPrepared;
		}

		private void InvokePrepare()
		{
			IsPrepared = !IsPrepared;
			Prepare?.Invoke(_spell);
		}

		public void Edit()
		{
			IsEditMode = !_isEditMode;
		}
	}
}
