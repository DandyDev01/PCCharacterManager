using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for StringListView.xaml
	/// </summary>
	public partial class StringListView : UserControl
	{
		public static readonly DependencyProperty ListViewDropCommand =
			DependencyProperty.Register("ListViewDropCommand", typeof(ICommand), typeof(StringListView),
				new PropertyMetadata(null));

		public StringListView()
		{
			InitializeComponent();
		}

		private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed &&
					sender is FrameworkElement frameworkElement)
			{
				DragDrop.DoDragDrop(frameworkElement,
					new DataObject(DataFormats.Serializable, frameworkElement.DataContext), DragDropEffects.Move);
			}
		}

		private void ListView_Drop(object sender, DragEventArgs e)
		{
			//if (ListViewDropCommand?.CanExecute(null) ?? false)
			//{
			//	ListViewDropCommand?.Execute(null);
			//}
		}
	}
}
