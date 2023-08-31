using PCCharacterManager.Commands;
using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public class CharacterSpellBookViewModel : ObservableObject
	{
		private readonly SpellSearch spellSearch;
		private readonly SpellItemEditableVMPool spellItemPool;

		public Array SearchFilters { get; private set; } = Enum.GetValues(typeof(SpellType));
		public Array OrderByOptions { get; private set; } = Enum.GetValues(typeof(OrderByOption));	
		public SpellSchool[] Filters { get; private set; } = (SpellSchool[])Enum.GetValues(typeof(SpellSchool));

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChanged(ref isEditMode, value);
				OnPropertyChanged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

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

		private Spell? selectedPreparedSpell;
		public Spell? SelectedPreparedSpell
		{
			get { return selectedPreparedSpell; }
			set
			{
				if (value == null) return;

				if (value.Name.Equals(searchTerm))
				{
					SearchTerm = "";
					selectedPreparedSpell = null;
					return;
				}

				selectedPreparedSpell = value;
				SearchTerm = value.Name;
				PreparedSpellText = "Unprepare " + selectedPreparedSpell.Name;
			}
		}

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChanged(ref searchTerm, value);
				Search(searchTerm);
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
				PopulateSpellsToDisplay(FilteredSpells[selectedFilter]);
			}
		}

		private SpellSchool selectedFilter;
		public SpellSchool SelectedFilter
		{
			get { return selectedFilter; }
			set
			{
				OnPropertyChanged(ref selectedFilter, value);
				PopulateSpellsToDisplay(FilteredSpells[selectedFilter]);
			}
		}

		public ICommand AddSpellCommand { get; private set; }
		public ICommand AddCantripCommand { get; private set; }
		public ICommand ClearPreparedSpellsCommand { get; private set; }
		public ICommand DeleteSpellCommand { get; private set; }
		public ICommand DeleteCantripCommand { get; private set; }
		public ICommand UnprepareSpellCommand { get; private set; }
		public ICommand NextFilterCommand { get; private set; }

		private SpellBook spellBook;
		public SpellBook SpellBook
		{
			get { return spellBook; }
			set { OnPropertyChanged(ref spellBook, value); }
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

		public SpellItemEditableViewModel? PrevSelectedSpell { get; private set; }
		public SpellItemEditableViewModel? PrevSelectedCantrip { get;private set; }

		public readonly List<SpellItemEditableViewModel> CantripItems; // used to store all cantrips wrapped
		public Dictionary<SpellSchool, ObservableCollection<SpellItemEditableViewModel>> FilteredSpells { get; private set; } // used to store all spells

		public ObservableCollection<SpellItemEditableViewModel> SpellsToDisplay { get; }
		public ObservableCollection<SpellItemEditableViewModel> CantripsToDisplay { get; }

		public CharacterSpellBookViewModel(CharacterStore _characterStore)
		{
			_characterStore.SelectedCharacterChange += OnCharacterChanged;

			spellSearch = new SpellSearch();
			spellItemPool = new SpellItemEditableVMPool(20);

			FilteredSpells = new Dictionary<SpellSchool, ObservableCollection<SpellItemEditableViewModel>>();
			SpellsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			CantripsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			CantripItems = new List<SpellItemEditableViewModel>();

			spellBook = new SpellBook();
			spellBookNote = spellBook.Note;

			searchTerm = string.Empty;
			preparedSpellText = string.Empty;

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
			ReleaseSpellItems();
			CantripsToDisplay.Clear();
			SpellBook = newCharacter.SpellBook;
			SpellBookNote = SpellBook.Note;

			FilteredSpells = PopulateFilteredSpells();
			SelectedFilter = SpellSchool.ALL;
			CantripItems.Clear();

			PopulateCantripsToShow();
		}

		/// <summary>
		/// returns spell items back to their pool and unsubscribs any methods
		/// </summary>
		private void ReleaseSpellItems()
		{
			foreach (SpellSchool school in Filters)
			{
				if (!FilteredSpells.ContainsKey(school)) continue;
				foreach (SpellItemEditableViewModel spellItemVM in FilteredSpells[school])
				{
					spellItemVM.Prepare -= spellBook.PrepareSpell;
					spellItemPool.Return(spellItemVM);
				}
			}
		}

		/// <summary>
		/// sets the items in spells to display
		/// </summary>
		/// <param name="items">the items to display</param>
		private void PopulateSpellsToDisplay(IEnumerable<SpellItemEditableViewModel> items)
		{
			SpellsToDisplay.Clear();
			switch (selectedOrderByOption)
			{
				case OrderByOption.ALPHABETICAL:
					foreach (var spell in items) SpellsToDisplay.Add(spell);
					break;
				case OrderByOption.LEVEL:
					foreach (var spell in items.OrderBy(x => x.Spell.Level)) SpellsToDisplay.Add(spell);
					break;
				case OrderByOption.DURATION:
					foreach (var spell in items.OrderBy(x => x.Spell.Duration)) SpellsToDisplay.Add(spell);
					break;
				case OrderByOption.SCHOOL:
					foreach (var spell in items.OrderBy(x => x.Spell.School)) SpellsToDisplay.Add(spell);
					break;
				case OrderByOption.PREPARED:
					foreach (var spell in items.OrderBy(x => !x.Spell.IsPrepared)) SpellsToDisplay.Add(spell);
					break;
			}

		}

		/// <summary>
		/// used for initilization when the character is changed
		/// </summary>
		private void PopulateCantripsToShow()
		{
			foreach (var item in spellBook.CantripsKnown.OrderBy(x => x.Name))
			{
				SpellItemEditableViewModel temp = new SpellItemEditableViewModel(item);
				CantripItems.Add(temp);

				item.IsPrepared = true;
				temp.IsPrepared = true;
				CantripsToDisplay.Add(temp);
			}
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
		/// in the view and in the spellbook model
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

		/// <summary>
		/// used for initilization when character is changed
		/// </summary>
		/// <returns></returns>
		private Dictionary<SpellSchool, ObservableCollection<SpellItemEditableViewModel>> PopulateFilteredSpells()
		{
			Dictionary<SpellSchool, ObservableCollection<SpellItemEditableViewModel>> results = new Dictionary<SpellSchool, ObservableCollection<SpellItemEditableViewModel>>();
			foreach (SpellSchool school in Filters)
			{
				results.Add(school, new ObservableCollection<SpellItemEditableViewModel>());
				foreach (Spell spell in spellBook.SpellsKnown[school])
				{
					SpellItemEditableViewModel spellItemViewModel = spellItemPool.GetItem();
					spellItemViewModel.Bind(spell);
					spellItemViewModel.Prepare += spellBook.PrepareSpell;
					spellItemViewModel.IsPrepared = spell.IsPrepared;
					results[school].Add(spellItemViewModel);
					results[SpellSchool.ALL].Add(spellItemViewModel);
				}
			}

			foreach (SpellSchool school in Filters)
			{
				results[school] = new ObservableCollection<SpellItemEditableViewModel>(results[school].OrderBy(x => x.Spell.Name));
			}

			return results;
		}

		private void Search(string searchTerm)
		{
			switch (SelectedSearchFilter)
			{
				case SpellType.SPELL:
					SpellSearch(searchTerm);
					break;
				case SpellType.CANTRIP:
					CantripSearth(searchTerm);
					break;
				case SpellType.BOTH:
					SpellSearch(searchTerm);
					CantripSearth(searchTerm);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Populate the spellsToDisplay with spells whose name contrain the search term
		/// </summary>
		/// <param name="term">the search term</param>
		private void SpellSearch(string term)
		{
			SpellsToDisplay.Clear();
			term = term.ToLower();

			IEnumerable<SpellItemEditableViewModel> results = spellSearch.Search(term, FilteredSpells[selectedFilter]);

			foreach (SpellItemEditableViewModel spellVM in results)
			{
				SpellsToDisplay.Add(spellVM);
			}
		}

		/// <summary>
		/// Populate cantripsToDisplay with cantrips that contain the search term in their name
		/// </summary>
		/// <param name="term">the search term</param>
		private void CantripSearth(string term)
		{
			CantripsToDisplay.Clear();
			foreach (var item in CantripItems.OrderBy(x => x.Spell.Name))
			{
				if (item.Spell.Name.ToLower().Contains(term.ToLower()))
				{
					CantripsToDisplay.Add(item);
				}
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
