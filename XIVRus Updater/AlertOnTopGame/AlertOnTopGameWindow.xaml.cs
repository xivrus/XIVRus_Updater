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
using System.Windows.Shapes;

namespace XIVRUS_Updater.AlertOnTopGame
{
	/// <summary>
	/// Логика взаимодействия для AlertOnTopGameWindow.xaml
	/// </summary>
	public partial class AlertOnTopGameWindow : Window
	{
		public AlertOnTopGameWindow(Config config, MainWindow mainWindow)
		{
			InitializeComponent();
			ModDisabledAlertPage modDisabledAlertPage = new ModDisabledAlertPage();
			MainFrame.Content = modDisabledAlertPage;
		}

		private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			// Always On Top
			var window = (Window)sender;
			window.Topmost = true;
		}
	}
}
