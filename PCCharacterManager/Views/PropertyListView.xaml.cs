using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
