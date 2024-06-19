using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterSpellBookViewModel : ObservableObject
	{
		private const string unprepared = "un-prepare ";

		private readonly SpellSearch _spellSearch;
		private readonly SpellSearch _cantripSearch;
		private readonly CollectionViewPropertySort _spellPropertySort;
		private readonly CollectionViewPropertySort _cantripPropertySort;
		private readonly DialogServiceBase _dialogService;
		private readonly RecoveryBase _recovery;

		public ICollectionView SpellsCollectionView { get; }
		public ICollectionView CantripsCollectionView { get; }

		public ObservableCollection<SpellItemEditableViewModel> SpellsToDisplay { get; }
		public ObservableCollection<SpellItemEditableViewModel> CantripsToDisplay { get; }

		public Array SearchFilters { get; } = Enum.GetValues(typeof(SpellType));
		public Array OrderByOptions { get; } = Enum.GetValues(typeof(OrderByOption));	
		public SpellSchool[] Filters { get; } = (SpellSchool[])Enum.GetValues(typeof(SpellSchool));

		private SpellItemEditableViewModel? selectedSpell;
		public SpellItemEditableViewModel? SelectedSpell
		{
			get { return selectedSpell; }
			set
			{
				OnPropertyChanged(ref selectedSpell, value);
				selectedSpell?.Edit();
				PrevSelectedSpell = selectedSpell;
				selectedSpell = null;
			}
		}

		private SpellItemEditableViewModel? selectedCantrip;
		public SpellItemEditableViewModel? SelectedCantrip
		{
			get { return selectedCantrip; }
			set
			{
				OnPropertyChanged(ref selectedCantrip, value);
				selectedCantrip?.Edit();
				PrevSelectedCantrip = selectedCantrip;
				selectedCantrip = null;
			}
		}

		public SpellItemEditableViewModel? PrevSelectedSpell { get; private set; }
		public SpellItemEditableViewModel? PrevSelectedCantrip { get; private set; }

		private SpellBook _spellBook;
		public SpellBook SpellBook
		{
			get { return _spellBook; }
			set { OnPropertyChanged(ref _spellBook, value); }
		}

		private Spell? _selectedPreparedSpell;
		public Spell? SelectedPreparedSpell
		{
			get { return _selectedPreparedSpell; }
			set
			{
				if (value == null) return;

				if (value.Name.Equals(SearchTerm))
				{
					SearchTerm = "";
					_selectedPreparedSpell = null;
					return;
				}

				_selectedPreparedSpell = value;
				SearchTerm = value.Name;
				PreparedSpellText = unprepared + _selectedPreparedSpell.Name;
			}
		}

		private Note _spellBookNote;
		public Note SpellBookNote
		{
			get { return _spellBookNote; }
			set
			{
				OnPropertyChanged(ref _spellBookNote, value);
			}
		}

		public string SearchTerm
		{
			get => _spellSearch.SearchTerm;
			set
			{
				_spellSearch.SearchTerm = value;
				_cantripSearch.SearchTerm = value;
				Search();
			}
		}

		private string _preparedSpellText;
		public string PreparedSpellText
		{
			get { return _preparedSpellText; }
			set
			{
				OnPropertyChanged(ref _preparedSpellText, value);
			}
		}

		private bool _isEditMode;
		public bool IsEditMode
		{
			get { return _isEditMode; }
			set
			{
				OnPropertyChanged(ref _isEditMode, value);
				OnPropertyChanged(nameof(IsDisplayMode));
			}
		}

		public bool IsDisplayMode
		{
			get { return !_isEditMode; }
		}

		private SpellType _selectedSearchFilter;
		public SpellType SelectedSearchFilter
		{
			get { return _selectedSearchFilter; }
			set { OnPropertyChanged(ref _selectedSearchFilter, value); }
		}

		private OrderByOption _selectedOrderByOption;
		public OrderByOption SelectedOrderByOption
		{
			get { return _selectedOrderByOption; }
			set 
			{ 
				OnPropertyChanged(ref _selectedOrderByOption, value);
				SpellSortFilter();
			}
		}

		private SpellSchool _selectedFilter;
		public SpellSchool SelectedFilter
		{
			get { return _selectedFilter; }
			set
			{
				OnPropertyChanged(ref _selectedFilter, value);

				if (value == SpellSchool.ALL)
				{
					SpellsCollectionView.Filter = _spellSearch.Search;
				}
				else
				{
					SpellsCollectionView.Filter = DisplayFilter;
				}
				
				SpellsCollectionView.Refresh();
			}
		}

		public ICommand AddSpellCommand { get; }
		public ICommand AddCantripCommand { get; }
		public ICommand ClearPreparedSpellsCommand { get; }
		public ICommand DeleteSpellCommand { get; }
		public ICommand DeleteCantripCommand { get; }
		public ICommand UnprepareSpellCommand { get; }
		public ICommand NextFilterCommand { get; }

		public CharacterSpellBookViewModel(CharacterStore characterStore, DialogServiceBase dialogService, RecoveryBase recovery)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			_recovery = recovery;
			_dialogService = dialogService;
			_spellSearch = new SpellSearch();
			_cantripSearch = new SpellSearch();

			SpellsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			CantripsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();

			_spellBook = new SpellBook();
			_spellBookNote = _spellBook.Note;

			_preparedSpellText = string.Empty;

			SpellsCollectionView = CollectionViewSource.GetDefaultView(SpellsToDisplay);
			CantripsCollectionView = CollectionViewSource.GetDefaultView(CantripsToDisplay);
			_spellPropertySort = new CollectionViewPropertySort(SpellsCollectionView);
			_cantripPropertySort = new CollectionViewPropertySort(CantripsCollectionView);

			SpellsCollectionView.Filter = _spellSearch.Search;
			CantripsCollectionView.Filter = _cantripSearch.Search;

			AddSpellCommand = new AddItemToSpellBookCommand(this, SpellType.SPELL, dialogService);
			AddCantripCommand = new AddItemToSpellBookCommand(this, SpellType.CANTRIP, dialogService);
			DeleteSpellCommand = new RemoveItemFromSpellBookCommand(this, dialogService, SpellType.SPELL);
			DeleteCantripCommand = new RemoveItemFromSpellBookCommand(this, dialogService, SpellType.CANTRIP);
			UnprepareSpellCommand = new RemovePreparedSpellCommand(this);
			ClearPreparedSpellsCommand = new ClearPreparedSpellsCommand(this);
			NextFilterCommand = new RelayCommand(NextFilter);
		}

		private void OnCharacterChanged(DnD5eCharacter newCharacter)
		{
			CantripsToDisplay.Clear();
			SpellsToDisplay.Clear();

			SpellBook = newCharacter.SpellBook;
			SpellBookNote = SpellBook.Note;

			SelectedFilter = SpellSchool.ALL;
			SelectedOrderByOption = OrderByOption.ALPHABETICAL;

			// populate spells
			foreach (SpellSchool school in Filters)
			{
				foreach (Spell spell in _spellBook.SpellsKnown[school])
				{
					var spellItem = new SpellItemEditableViewModel(spell);
					spellItem.Prepare += _spellBook.PrepareSpell;
					SpellsToDisplay.Add(spellItem);
				}
			}

			// populate cantrips
			foreach (Spell spell in _spellBook.CantripsKnown)
			{
				CantripsToDisplay.Add(new SpellItemEditableViewModel(spell));
			}
		}

		/// <summary>
		/// sets the items in spells to display
		/// </summary>
		/// <param name="items">the items to display</param>
		private void SpellSortFilter()
		{
			switch (_selectedOrderByOption)
			{
				case OrderByOption.ALPHABETICAL:
					_spellPropertySort.Sort(nameof(Spell.Name));
					break;
				case OrderByOption.LEVEL:
					_spellPropertySort.Sort(nameof(Spell.Level));
					break;
				case OrderByOption.DURATION:
					_spellPropertySort.Sort(nameof(Spell.Duration));
					break;
				case OrderByOption.SCHOOL:
					_spellPropertySort.Sort(nameof(Spell.School));
					break;
				case OrderByOption.PREPARED:
					_spellPropertySort.Sort(nameof(Spell.IsPrepared));;
					break;
			}
		}

		private bool DisplayFilter(object obj)
		{
			if (obj is SpellItemEditableViewModel spellItem)
			{
				if (spellItem.School == _selectedFilter)
					return true;
			}

			return false;
		}

		private void Search()
		{
			switch (SelectedSearchFilter)
			{
				case SpellType.SPELL:
					SpellsCollectionView.Refresh();
					break;
				case SpellType.CANTRIP:
					CantripsCollectionView.Refresh();
					break;
				case SpellType.BOTH:
					SpellsCollectionView.Refresh();
					CantripsCollectionView.Refresh();
					break;
				default:
					SpellsCollectionView.Refresh();
					CantripsCollectionView.Refresh();
					break;
			}
		}

		private void NextFilter()
		{
			int currentIndex = (int)_selectedFilter;
			int nextIndex = currentIndex + 1;
			if (nextIndex > Filters.Length - 1)
			{
				currentIndex = 0;
				SelectedFilter = Filters[currentIndex];
				return;
			}

			SelectedFilter = Filters[nextIndex];
		}
	} // end class
}
