using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PCCharacterManager.Views
{
	/// <summary>
	/// Interaction logic for PropertyListView.xaml
	/// </summary>
	public partial class PropertyListView : UserControl
	{
		public static readonly DependencyProperty ListViewDropCommandProperty =
			DependencyProperty.Register("ListViewDropCommand", typeof(ICommand), typeof(PropertyListView),
				new PropertyMetadata(null));

		public PropertyListView()
		{
			InitializeComponent();
		}

		private void PropertyDisplayView_MouseMove(object sender, MouseEventArgs e)
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

		}
	}
}
