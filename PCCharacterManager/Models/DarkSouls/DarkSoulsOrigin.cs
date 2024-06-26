﻿using Newtonsoft.Json;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models.DarkSouls
{
    public class DarkSoulsOrigin : ObservableObject
    {
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
			}
		}

		private HitDie _hitDie;
		public HitDie HitDie
		{
			get
			{
				return _hitDie;
			}
			set
			{
				OnPropertyChanged(ref _hitDie, value);
			}
		}

		private Property _bloodiedEffect;
		public Property BloodiedEffect { get => _bloodiedEffect; set => _bloodiedEffect = value; }	

		private string[] _baseStatistics;
		public string[] BaseStatistics { get => _baseStatistics; set => _baseStatistics = value; }

		public DarkSoulsOrigin()
		{
			_name = string.Empty;
			_hitDie = HitDie.D10;
			_bloodiedEffect = new();
			_baseStatistics = Array.Empty<string>();
		}
	}
}
