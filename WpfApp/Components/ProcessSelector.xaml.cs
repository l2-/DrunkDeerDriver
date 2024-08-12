using Driver;
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

namespace WpfApp.Components
{
    /// <summary>
    /// Interaction logic for ProcessSelector.xaml
    /// </summary>
    public partial class ProcessSelector : Window
    {
        public ProfileItem? ProfileItem { get; set; }
        public List<string> ActiveProcesses { get; set; } = [];

        public ProcessSelector()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            //storedProcesses.Items = 
            base.OnSourceInitialized(e);
        }

        private void AddProcessManually_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
