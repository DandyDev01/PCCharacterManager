using PCCharacterManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PCCharacterManager.ViewModels;
using PCCharacterManager.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for CharacterNoteBookView.xaml
	/// </summary>
	public partial class CharacterNoteBookView : UserControl
	{
		private ICommand focusSearchCommand;


		public CharacterNoteBookView()
		{
			InitializeComponent();
			
			focusSearchCommand = new RelayCommand(FocusSearch);
			findButton.Command = new RelayCommand(Find);
			this.InputBindings.Add(new KeyBinding(focusSearchCommand, Key.Q, ModifierKeys.Control));
			DataContextChanged += Sub;
		}

		private void UpdateDocument(Note note)
		{
			richTextBox.Document.Blocks.Clear();
			richTextBox.Document.Blocks.Add(new Paragraph(new Run(note.Notes)));
		}

		private void Find()
		{
			TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);

			//clear up highlighted text before starting a new search
			textRange.ClearAllProperties();
			//lbl_Status.Content = "";

			//get the richtextbox text
			string textBoxText = textRange.Text;

			//get search text
			string searchText = findTextBox.Text;

			if (string.IsNullOrWhiteSpace(textBoxText) || string.IsNullOrWhiteSpace(searchText))
			{
				//lbl_Status.Content = "Please provide search text or source text to search from";
				return;
			}

			//using regex to get the search count
			//this will include search word even it is part of another word
			//say we are searching "hi" in "hi, how are you Mahi?" --> match count will be 2 (hi in 'Mahi' also)
			Regex regex = new Regex(searchText);
			int count_MatchFound = Regex.Matches(textBoxText, regex.ToString(), RegexOptions.IgnoreCase).Count;

			for (TextPointer startPointer = richTextBox.Document.ContentStart;
						startPointer.CompareTo(richTextBox.Document.ContentEnd) <= 0;
							startPointer = startPointer.GetNextContextPosition(LogicalDirection.Forward))
			{
				//check if end of text
				if (startPointer.CompareTo(richTextBox.Document.ContentEnd) == 0) break;

				//get the adjacent string
				string parsedString = startPointer.GetTextInRun(LogicalDirection.Forward);

				//check if the search string present here
				int indexOfParseString = parsedString.IndexOf(searchText);

				if (indexOfParseString >= 0) //present
				{
					//setting up the pointer here at this matched index
					startPointer = startPointer.GetPositionAtOffset(indexOfParseString);

					if (startPointer == null) break;
					
					//next pointer will be the length of the search string
					TextPointer nextPointer = startPointer.GetPositionAtOffset(searchText.Length);

					//create the text range
					TextRange searchedTextRange = new TextRange(startPointer, nextPointer);

					//color up 
					searchedTextRange.ApplyPropertyValue(TextElement.BackgroundProperty,
						new SolidColorBrush(Colors.Yellow));

					//add other setting property

				}
			}

			//update the label text with count
			if (count_MatchFound > 0)
			{
				//lbl_Status.Content = "Total Match Found : " + count_MatchFound;
			}
			else
			{
				//lbl_Status.Content = "No Match Found !";
			}
		}

		public void FocusSearch()
		{
			this.searchBox.Focus();
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			CharacterNoteBookViewModel? viewModel = DataContext as CharacterNoteBookViewModel;
			
			if (viewModel == null) return;

			if(treeView.SelectedItem is NoteSection)
			{
				viewModel.SelectedSection = treeView.SelectedItem as NoteSection;
				return;
			}

			if (treeView.SelectedItem is not Note) return;

			viewModel.SelectedNote = treeView.SelectedItem as Note;
			viewModel.SelectedSection = null;
		}

		/// <summary>
		/// used to link the vm so that the view updates to display the note contents
		/// </summary>
		private void Sub(object sender, DependencyPropertyChangedEventArgs e)
		{
			CharacterNoteBookViewModel? viewModel = DataContext as CharacterNoteBookViewModel;
			if (viewModel == null) return;
			viewModel.selectedNoteChange += UpdateDocument;
			viewModel.characterChange += ExpandAllHelper;
			richTextBox.TextChanged += UpdateNote;
		}

		private void UpdateNote(object sender, TextChangedEventArgs e)
		{
			CharacterNoteBookViewModel? viewModel = DataContext as CharacterNoteBookViewModel;
			string s = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

			if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)) return;
			viewModel.SelectedNote.Notes = s;
		}

		private void ExpandAllHelper()
		{
			ExpandAll(treeView, true);
		}

		private void ExpandAll(ItemsControl treeView, bool expand)
		{
			foreach (object obj in treeView.Items)
			{
				ItemsControl childControl = treeView.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
				if (childControl != null)
				{
					ExpandAll(childControl, expand);
				}
				TreeViewItem item = childControl as TreeViewItem;
				if (item != null)
					item.IsExpanded = true;
			}
		}
	}
}
