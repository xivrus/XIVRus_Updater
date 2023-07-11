using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace XIVRUS_Updater
{
	/// <summary>
	/// Логика взаимодействия для SettingsPage.xaml
	/// </summary>
	public partial class SettingsPage : Page
	{
		public MainWindow mainWindow = null;
		Config config = null;
		public SettingsPage()
		{
			InitializeComponent();
			AppVersionTB.Text = String.Format("Version: {0}", Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
		}

		public void Init()
		{
			config = mainWindow.config;
		}

		private void CancelSettingsButton_Click(object sender, RoutedEventArgs e)
		{
			mainWindow.LoadCofig();
			mainWindow.SettingsPageFrame.Visibility = Visibility.Collapsed;
        }
    }
}
