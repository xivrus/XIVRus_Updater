using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Config config = null;
		public MainWindow()
		{
			InitializeComponent();
			Init();
		}

		public void Init()
		{
			FirstStartPageFrame.Visibility = Visibility.Collapsed;
			if (!File.Exists(ConfigManager.CONFIGFILE))
			{
				FirstStartPageFrame.Visibility = Visibility.Visible;
				return;
			}
			config = ConfigManager.LoadConfig();
		}

		private void FirstStartPageFrame_LoadCompleted(object sender, NavigationEventArgs e)
		{
			FirstStartPage fsp = (FirstStartPage)FirstStartPageFrame.Content;
			fsp.mainWindow = this;
		}
	}
}
