using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace UIControls.UserControls
{
    /// <summary>
    /// Interaction logic for RegistrationMain.xaml
    /// </summary>
    public partial class RegistrationMain : UserControl
    {
        public RegistrationMain()
        {
            InitializeComponent();
        }
        private void txtMob_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
