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

		private readonly SpellSearch spellSearch;
		private readonly SpellSearch cantripSearch;
		private readonly CollectionViewPropertySort spellPropertySort;
		private readonly CollectionViewPropertySort cantripPropertySort;
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

		private SpellBook spellBook;
		public SpellBook SpellBook
		{
			get { return spellBook; }
			set { OnPropertyChanged(ref spellBook, value); }
		}

		private Spell? selectedPreparedSpell;
		public Spell? SelectedPreparedSpell
		{
			get { return selectedPreparedSpell; }
			set
			{
				if (value == null) return;

				if (value.Name.Equals(SearchTerm))
				{
					SearchTerm = "";
					selectedPreparedSpell = null;
					return;
				}

				selectedPreparedSpell = value;
				SearchTerm = value.Name;
				PreparedSpellText = unprepared + selectedPreparedSpell.Name;
			}
		}

		private Note spellBookNote;
		public Note SpellBookNote
		{
			get { return spellBookNote; }
			set
			{
				OnPropertyChanged(ref spellBookNote, value);
			}
		}

		public string SearchTerm
		{
			get => spellSearch.SearchTerm;
			set
			{
				spellSearch.SearchTerm = value;
				cantripSearch.SearchTerm = value;
				Search();
			}
		}

		private string preparedSpellText;
		public string PreparedSpellText
		{
			get { return preparedSpellText; }
			set
			{
				OnPropertyChanged(ref preparedSpellText, value);
			}
		}

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChanged(nameof(IsDisplayMode));
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		private SpellType selectedSearchFilter;
		public SpellType SelectedSearchFilter
		{
			get { return selectedSearchFilter; }
			set { OnPropertyChanged(ref selectedSearchFilter, value); }
		}

		private OrderByOption selectedOrderByOption;
		public OrderByOption SelectedOrderByOption
		{
			get { return selectedOrderByOption; }
			set 
			{ 
				OnPropertyChanged(ref selectedOrderByOption, value);
				SpellSortFilter();
			}
		}

		private SpellSchool selectedFilter;
		public SpellSchool SelectedFilter
		{
			get { return selectedFilter; }
			set
			{
				OnPropertyChanged(ref selectedFilter, value);
				switch (value)
				{
					case SpellSchool.ALL:
						SpellsCollectionView.Filter = spellSearch.Search;
						break;
					default:
						SpellsCollectionView.Filter = DisplayFilter;
						break;
				}
				
				SpellsCollectionView.Refresh();
			}
		}

		public ICommand AddSpellCommand { get; private set; }
		public ICommand AddCantripCommand { get; private set; }
		public ICommand ClearPreparedSpellsCommand { get; private set; }
		public ICommand DeleteSpellCommand { get; private set; }
		public ICommand DeleteCantripCommand { get; private set; }
		public ICommand UnprepareSpellCommand { get; private set; }
		public ICommand NextFilterCommand { get; private set; }

		public CharacterSpellBookViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			spellSearch = new SpellSearch();
			cantripSearch = new SpellSearch();

			SpellsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			CantripsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();

			spellBook = new SpellBook();
			spellBookNote = spellBook.Note;

			preparedSpellText = string.Empty;

			SpellsCollectionView = CollectionViewSource.GetDefaultView(SpellsToDisplay);
			CantripsCollectionView = CollectionViewSource.GetDefaultView(CantripsToDisplay);
			spellPropertySort = new CollectionViewPropertySort(SpellsCollectionView);
			cantripPropertySort = new CollectionViewPropertySort(CantripsCollectionView);

			SpellsCollectionView.Filter = spellSearch.Search;
			CantripsCollectionView.Filter = cantripSearch.Search;

			AddSpellCommand = new AddItemToSpellBookCommand(this, SpellType.SPELL);
			AddCantripCommand = new AddItemToSpellBookCommand(this, SpellType.CANTRIP);
			DeleteSpellCommand = new RemoveItemFromSpellBookCommand(this, SpellType.SPELL);
			DeleteCantripCommand = new RemoveItemFromSpellBookCommand(this, SpellType.CANTRIP);
			UnprepareSpellCommand = new RelayCommand(RemovePreparedSpell);
			ClearPreparedSpellsCommand = new RelayCommand(ClearPreparedSpells);
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
				foreach (Spell spell in spellBook.SpellsKnown[school])
				{
					SpellsToDisplay.Add(new SpellItemEditableViewModel(spell));
				}
			}

			// populate cantrips
			foreach (Spell spell in spellBook.CantripsKnown)
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
			switch (selectedOrderByOption)
			{
				case OrderByOption.ALPHABETICAL:
					spellPropertySort.Sort(nameof(Spell.Name));
					break;
				case OrderByOption.LEVEL:
					spellPropertySort.Sort(nameof(Spell.Level));
					break;
				case OrderByOption.DURATION:
					spellPropertySort.Sort(nameof(Spell.Duration));
					break;
				case OrderByOption.SCHOOL:
					spellPropertySort.Sort(nameof(Spell.School));
					break;
				case OrderByOption.PREPARED:
					spellPropertySort.Sort(nameof(Spell.IsPrepared));;
					break;
			}
		}

		private bool DisplayFilter(object obj)
		{
			if (obj is SpellItemEditableViewModel spellItem)
			{
				if (spellItem.School == selectedFilter)
					return true;
			}

			return false;
		}

		/// <summary>
		/// used by the clear prepared spells command
		/// </summary>
		private void ClearPreparedSpells()
		{
			spellBook.ClearPreparedSpells();

			foreach (var spellItemView in SpellsToDisplay)
			{
				spellItemView.IsPrepared = false;
			}
		}

		/// <summary>
		/// remove the selected prepared spell from the prepared spells list
		/// in the view and in the spell book model
		/// </summary>
		private void RemovePreparedSpell()
		{
			if (selectedPreparedSpell == null) return;

			if (spellBook.PreparedSpells.Contains(selectedPreparedSpell))
			{
				selectedPreparedSpell.IsPrepared = false;
				spellBook.PreparedSpells.Remove(selectedPreparedSpell);
				List<SpellItemEditableViewModel> spells = SpellsToDisplay.ToList();
				SpellItemEditableViewModel? s = spells.Find(x => x.Spell.Name.Equals(selectedPreparedSpell.Name));
				if (s == null) return;
				s.IsPrepared = false;
				selectedPreparedSpell = null;
			}
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
			int currentIndex = (int)selectedFilter;
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
