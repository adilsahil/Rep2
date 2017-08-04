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
namespace UIControls.UserControls
{
    /// <summary>
    /// Interaction logic for SampleGI.xaml
    /// </summary>
    public partial class SampleGI : UserControl
    {
        public SampleGI()
        {
            InitializeComponent();
        }

        private void EventTrigger_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            Border br = sender as Border;
        }

        private void AnimatedColorButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AnimatedColorButton_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
           
        }
    }
}
