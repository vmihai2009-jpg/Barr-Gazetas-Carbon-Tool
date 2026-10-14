using System.Windows;

namespace CarbonEmissionTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var carbonEmissionToolPage = new CarbonEmissionToolPage(this);

            this.PageFrame.Content = carbonEmissionToolPage;
        }
    }
}
