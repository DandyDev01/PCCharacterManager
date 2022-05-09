using PCCharacterManager.DialogWindows;
using PCCharacterManager.Models;
using PCCharacterManager.Services;
using PCCharacterManager.Stores;
using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCCharacterManager.ViewModels
{
	public enum SpellFilterType { SPELL, CANTRIP, BOTH }

	public class CharacterSpellBookViewModel : TabItemViewModel
	{
		public Array Filters { get; private set; } = Enum.GetValues(typeof(SpellFilterType));

		private bool isEditMode;
		public bool IsEditMode
		{
			get { return isEditMode; }
			set
			{
				OnPropertyChaged(ref isEditMode, value);
				OnPropertyChaged("IsDisplayMode");
			}
		}

		public bool IsDisplayMode
		{
			get { return !isEditMode; }
		}

		private SpellItemEditableViewModel selectedSpell;
		public SpellItemEditableViewModel SelectedSpell
		{
			get { return selectedSpell; }
			set
			{
				OnPropertyChaged(ref selectedSpell, value);
				selectedSpell?.Edit();
				prevSelectedSpell = selectedSpell;
				selectedSpell = null;
			}
		}

		private SpellItemEditableViewModel selectedCantrip;
		public SpellItemEditableViewModel SelectedCantrip
		{
			get { return selectedCantrip; }
			set
			{
				OnPropertyChaged(ref selectedCantrip, value);
				selectedCantrip.Edit();
				prevSelectedCantrip = selectedCantrip;
				selectedCantrip = null;
			}
		}

		private string searchTerm;
		public string SearchTerm
		{
			get { return searchTerm; }
			set
			{
				OnPropertyChaged(ref searchTerm, value);
				Search(searchTerm);
			}
		}

		private SpellFilterType selectedFilter;
		public SpellFilterType SelectedFilter
		{
			get { return selectedFilter; }
			set { OnPropertyChaged(ref selectedFilter, value); }
		}

		public ICommand AddSpellCommand { get; private set; }
		public ICommand AddCantripCommand { get; private set; }
		public ICommand ClearPreparedSpellsCommand { get; private set; }
		public ICommand DeleteSpellCommand { get; private set; }
		public ICommand DeleteCantripCommand { get; private set; }

		private SpellBook spellBook;
		public SpellBook SpellBook
		{
			get { return spellBook; }
			set { OnPropertyChaged(ref spellBook, value); }
		}

		private Note spellBookNote;
		public Note SpellBookNote
		{
			get { return spellBookNote; }
			set
			{
				OnPropertyChaged(ref spellBookNote, value);
			}
		}

		private SpellItemEditableViewModel prevSelectedSpell;
		private SpellItemEditableViewModel prevSelectedCantrip;

		private readonly List<SpellItemEditableViewModel> spellItems;
		private readonly List<SpellItemEditableViewModel> cantripItems;

		public ObservableCollection<SpellItemEditableViewModel> SpellsToDisplay { get; private set; }
		public ObservableCollection<SpellItemEditableViewModel> CantripsToDisplay { get; private set; }

		public CharacterSpellBookViewModel(CharacterStore _characterStore, ICharacterDataService _dataService)
			: base(_characterStore, _dataService)
		{
			characterStore.SelectedCharacterChange += OnCharacterChanged;

			SpellsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			CantripsToDisplay = new ObservableCollection<SpellItemEditableViewModel>();
			spellItems = new List<SpellItemEditableViewModel>();
			cantripItems = new List<SpellItemEditableViewModel>();

			AddSpellCommand = new RelayCommand(AddSpellWindow);
			AddCantripCommand = new RelayCommand(AddCantripWindow);
			ClearPreparedSpellsCommand = new RelayCommand(ClearPreparedSpells);
			DeleteSpellCommand = new RelayCommand(DeleteSpell);
			DeleteCantripCommand = new RelayCommand(DeleteCantrip);
		}

		protected override void OnCharacterChanged(Character newCharacter)
		{
			base.OnCharacterChanged(newCharacter);
			SpellsToDisplay.Clear();
			CantripsToDisplay.Clear();
			spellItems.Clear();
			cantripItems.Clear();

			SpellBook = selectedCharacter.SpellBook;

			PopulateSpellsToShow();
			PopulateCantripsToShow();

			SpellBookNote = spellBook.Note;
		}

		private void ClearPreparedSpells()
		{
			spellBook.ClearPreparedSpells();

			foreach (var spellItemView in SpellsToDisplay)
			{
				spellItemView.IsPrepared = false;
			}
		}

		private void PopulateSpellsToShow()
		{
			foreach (var item in spellBook.SpellsKnown.OrderBy(x => x.Name))
			{
				SpellItemEditableViewModel temp = new SpellItemEditableViewModel(item);
				temp.IsPrepared = item.IsPrepared;
				// this is how the spell is added the spellBooks prepared spells
				temp.Prepare += spellBook.PrepareSpell;

				spellItems.Add(temp);
				SpellsToDisplay.Add(temp);
			}
		}

		private void PopulateCantripsToShow()
		{
			foreach (var item in spellBook.CantripsKnown.OrderBy(x => x.Name))
			{
				SpellItemEditableViewModel temp = new SpellItemEditableViewModel(item);
				temp.IsPrepared = item.IsPrepared;
				cantripItems.Add(temp);

				item.IsPrepared = true;
				CantripsToDisplay.Add(temp);
			}
		}

		private void Search(string searchTerm)
		{
			switch (SelectedFilter)
			{
				case SpellFilterType.SPELL:
					SpellSearch(searchTerm);
					break;
				case SpellFilterType.CANTRIP:
					CantripSearth(searchTerm);
					break;
				case SpellFilterType.BOTH:
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

			foreach (var item in spellItems.OrderBy(x => x.Spell.Name))
			{
				if (item.Spell.Name.ToLower().Contains(term.ToLower()))
				{
					SpellsToDisplay.Add(item);
				}
			}
		}

		/// <summary>
		/// Populate cantripsToDisplay with cantrips that contain the search term in their name
		/// </summary>
		/// <param name="term">the search term</param>
		private void CantripSearth(string term)
		{
			foreach (var item in cantripItems.OrderBy(x => x.Spell.Name))
			{
				if (item.Spell.Name.ToLower().Contains(term.ToLower()))
				{
					CantripsToDisplay.Add(item);
				}
			}
		}

		/// <summary>
		/// Open window to add new Spell
		/// </summary>
		private void AddSpellWindow()
		{
			Window window = new AddSpellDialogWindow();
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, characterStore, SpellFilterType.SPELL,
				dataService, characterStore.SelectedCharacter);

			window.DataContext = data;

			window.ShowDialog();
			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			spellItems.Add(temp);
			SpellsToDisplay.Add(temp);
		}

		private void AddCantripWindow()
		{
			Window window = new AddSpellDialogWindow();
			DialogWindowAddSpellViewModel data = new DialogWindowAddSpellViewModel(window, characterStore, SpellFilterType.CANTRIP,
				dataService, characterStore.SelectedCharacter);

			window.DataContext = data;

			window.ShowDialog();
			SpellItemEditableViewModel temp = new SpellItemEditableViewModel(data.NewSpell);
			cantripItems.Add(temp);
			CantripsToDisplay.Add(temp);
		}

		private void DeleteSpell()
		{
			var messageBox = MessageBox.Show("Are you sure you want to delete " + prevSelectedSpell.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
			if (messageBox == MessageBoxResult.No)
				return;

			spellBook.RemoveSpell(prevSelectedSpell.Spell);
			SpellsToDisplay.Remove(prevSelectedSpell);
		}

		private void DeleteCantrip()
		{
			var messageBox = MessageBox.Show("Are you sure you want to delete " + prevSelectedCantrip.Spell.Name, "Delete Spell", MessageBoxButton.YesNo);
			if (messageBox == MessageBoxResult.No)
				return;

			spellBook.RemoveCantrip(prevSelectedCantrip.Spell);
			CantripsToDisplay.Remove(prevSelectedCantrip);
		}

	} // end class
}
