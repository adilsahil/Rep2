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
using System.Windows.Shapes;
using UIContainer.Registration;
using UIControls;
using UIContainer.SampleGICont;
using UIControls.UserControls;
using UIContainer.Dash;
using UIContainer.ListviewCont;
using System.Windows.Threading;

namespace UIDummy
{
    /// <summary>
    /// Interaction logic for RegistrationMWindow.xaml
    /// </summary>
    public partial class RegistrationMWindow : Window
    {
        int validId = 1;
        System.Windows.Forms.Timer tmr = null;
        public delegate void ReadyToShowDelegate(object sender, EventArgs args);
        public event ReadyToShowDelegate ReadyToShow;
        private DispatcherTimer timer;     
        public RegistrationMWindow(string UName)
        {
            InitializeComponent();
            lblUserName.Content = UName;
            StartTimer();
            tbItemMain.Content = new DashBoardContainer();
            this.KeyDown += new KeyEventHandler(MainReg_KeyDown);
            _box.Text = "WELCOME" +"   " + UName +"  "+ " : " + " TO Hospital Management System";
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(7);
            timer.Tick += new EventHandler(timer_Tick);           
            timer.Start();
        }
        private void MainReg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string message = HisMessageBox.ShowMessage("Are you sure want to Logout??");
                if (message != null)
                {
                    if (message.ToString() == "1")
                    {
                        MainWindow objAdmin = new MainWindow();
                        this.Close();
                        objAdmin.ShowDialog();
                    }
                }
            }
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (validId == 2)
                {
                    string message = HisMessageBox.ShowMessage("You are in Registration screen are you sure go to DashBoard??");
                    if (message != null)
                    {
                        if (message.ToString() == "1")
                        {
                            tbItemMain.Content = new DashBoardContainer();
                            validId = 1;
                        }
                    }
                }
                else if (validId == 3)
                {
                    string message = HisMessageBox.ShowMessage("You are in Reg Details screen are you sure go to DashBoard??");
                    if (message != null)
                    {
                        if (message.ToString() == "1")
                        {
                            tbItemMain.Content = new DashBoardContainer();
                            validId = 1;
                        }
                    }
                }
            }
            if (e.Key == Key.R && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (validId == 1)
                {
                    string message = HisMessageBox.ShowMessage("You are in DashBoard screen are you sure go to Registration??");
                    if (message != null)
                    {
                        if (message.ToString() == "1")
                        {
                            tbItemMain.Content = new UCRegistrationContainer();
                            validId = 2;
                        }

                    }
                }
                else if (validId == 3)
                {
                    string message = HisMessageBox.ShowMessage("You are in Reg Details screen are you sure go to Registration Details??");
                    if (message != null)
                    {
                        if (message.ToString() == "1")
                        {
                            tbItemMain.Content = new UCRegistrationContainer();
                            validId = 2;
                        }
                    }
                }
            }
        }
        private void exp3_Collapsed(object sender, RoutedEventArgs e)
        {
            //Expander exp = sender as Expander;
            //Border brdr = (Border)exp.Parent;
            //foreach (object obj in stkMain.Children)
            //{
            //    if (obj.GetType() == typeof(Border))
            //    {
            //        Border brdrObj = (Border)obj;
            //        Expander exp1 = (Expander)brdrObj.Child;
            //        if (exp1.Name != exp.Name)
            //        {                      
            //            exp1.IsExpanded = false;
            //        }
            //    }
            //}
        }       
        private void StartTimer()
        {
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;
        }
        void tmr_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            lblDateTime.Content = date.ToString("dd-MMMM-yyyy").ToUpper() + " " + "\n" + date.ToLongTimeString();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            // This timer imitates a time-consuming operation (or a whole bunch of
            // such operations).
            // When done, it raises a custom event to let the splash screen know that its time is up.

            timer.Stop();

            if (ReadyToShow != null)
            {
                ReadyToShow(this, null);
            }
        }
        private void tvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
        }
        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = sender as TreeViewItem;
            TreeView tv = (TreeView)tvi.Parent;
            int Uid = Convert.ToInt32(tvi.Uid);
            foreach (TreeViewItem item in tv.Items)
            {
                if (Uid == Convert.ToInt32(item.Uid))
                {
                    item.IsExpanded = true;
                }
                else
                {
                    item.IsExpanded = false;
                }
            }
        }
        private void tviRegDetails_Selected(object sender, RoutedEventArgs e)
        {
            
        }       
        private void tviLv_Selected(object sender, RoutedEventArgs e)
        {
            
        }
        private void tviregistration_Selected(object sender, RoutedEventArgs e)
        {
           
        }
        private void tviRegDetails_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tv = sender as TreeViewItem;
            tv.Background = Brushes.Transparent;
            if (validId == 1)
            {
                string message = HisMessageBox.ShowMessage("You are in DashBoard screen are you sure go to Registration Details??");
                if (message != null)
                {
                    if (message.ToString() == "1")
                    {
                        tbItemMain.Content = new SampleGIContainer();
                        validId = 3;
                    }

                }
            }
            else if (validId == 2)
            {
                string message = HisMessageBox.ShowMessage("You are in Registration screen are you sure go to Registration Details??");
                if (message != null)
                {
                    if (message.ToString() == "1")
                    {
                        tbItemMain.Content = new SampleGIContainer();
                        validId = 3;
                    }
                }
            }
        }
        private void tviregistration_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tv = sender as TreeViewItem;
            tv.Background = Brushes.Transparent;
            if (validId == 1)
            {
                string message = HisMessageBox.ShowMessage("You are in DashBoard screen are you sure go to Registration??");
                if (message != null)
                {
                    if (message.ToString() == "1")
                    {
                        tbItemMain.Content = new UCRegistrationContainer();
                        validId = 2;
                    }

                }
            }
            else if (validId == 3)
            {
                string message = HisMessageBox.ShowMessage("You are in Reg Det screen are you sure go to Registration??");
                if (message != null)
                {
                    if (message.ToString() == "1")
                    {
                        tbItemMain.Content = new UCRegistrationContainer();
                        validId = 2;
                    }
                }
            }
        }

        private void tviLv_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tv = sender as TreeViewItem;
            tv.Background = Brushes.Transparent;
            tbItemMain.Content = new ListviewContainer();
        }
    }
}
