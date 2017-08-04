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
using System.Windows.Media.Animation;

namespace UIControls.UserControls
{
    /// <summary>
    /// Interaction logic for HIsticky.xaml
    /// </summary>
    public partial class HIsticky : UserControl
    {
        public HIsticky()
        {
            InitializeComponent();
        }

        private void BtnExpand_Click(object sender, RoutedEventArgs e)
        {
            int butUid = Convert.ToInt32(BtnExpand.Uid);
            if (butUid == 1)
            {
                Storyboard sb = (Storyboard)FindResource("ExpandStory");
                sb.Begin();
                BtnExpand.Uid = 2.ToString();
                BtnExpand.Content = "-";
            }
            else if (butUid == 2)
            {
                Storyboard sb = (Storyboard)FindResource("CollapseStory");
                sb.Begin();
                BtnExpand.Content = "+";
                BtnExpand.Uid = 1.ToString();
            }
        }
        private void BtnExpand_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void BtnExpand_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}
