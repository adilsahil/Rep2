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
using System.Windows.Threading;
using UIControls.UserControls;

namespace UIDummy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> nameList;
        public string passWord = string.Empty;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        TimeSpan adtime = new TimeSpan(0, 0, 1);
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            crcProgress.Visibility = Visibility.Hidden;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0);
            nameList = new List<string> 
            {
                "KAMEDA-01","ADMIN","ADMIN2","BENZA","ROOPAN","RUBEL",
                "KAMEDA-02","KAMEDA-03","KAMEDA-04",
                "HR-SATHMA","HR-02",
                "HR-03", "AKHIL", "ARUN", "SHIJEESH", "SUBRU", "ANISH", "SIJO", "ANSHAD", "SINDHU KUMAR", "HARISH", "SANDEEP", "MADHU", "PRINCE", "YATHEESH",
            };
            txtUserName.TextChanged += new TextChangedEventHandler(txtUserName_TextChanged);
        } 
        private void passwrdEntry_PasswordChanged(object sender, RoutedEventArgs e)
        {           
            if (passwrdEntry.Password.Length <= 0)
            {
                passwrdEntry.Password = "";
                txtPasswtr.Text = "";
                txtPasswtr.Text = "Enter Password";
                passwrdEntry.Opacity = 0.5;
            }
            else
            {
                txtPasswtr.Text = "";
                passwrdEntry.Opacity = 1;
            }
        }
        private void passwrdEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void passwrdEntry_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Tab)
            {
                btnLogin.Focus();
                btnLogin.Background = Brushes.Orange;
                Login();
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.Focus();
            btnLogin.Background = Brushes.Orange;
            Login();
           // dispatcherTimer.Start();
        }
        private void Login()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(txtUserName.Text).Trim()))
            {
                if (txtUserName.MaxLength >= 4)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(passwrdEntry.Password).Trim()))
                    {
                        if (passwrdEntry.Password.Length >= 4)
                        {                          
                            passWord = txtUserName.Text.ToLower().Trim().Replace(" ", String.Empty);
                            string txtPassWord = Convert.ToString(passwrdEntry.Password);
                            if (passWord == txtPassWord.ToLower().Trim())
                            {
                                dispatcherTimer.Start();

                            }
                            else
                            {
                                lblError.Content = "Password Missmatch";
                            }
                        }
                    }
                }
            }
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
              
                btnLogin.Focus();
                btnLogin.Background = Brushes.Orange;
                Login();
            }
           
        }
        private void text_GotFocus(object sender, RoutedEventArgs e)
        {
            lblError.Content = "";
            GocusEvent();
        }
        private void GocusEvent()
        {
            if (btnLogin.Background == Brushes.Orange)
            {
                btnLogin.Background = Brushes.Black;
            }
        }
        void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = txtUserName.Text.ToUpper();
            List<string> autoList = new List<string>();
            autoList.Clear();

            foreach (string item in nameList)
            {
                if (!string.IsNullOrEmpty(txtUserName.Text))
                {
                    if (item.StartsWith(typedString))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                lbSuggestion.ItemsSource = autoList;
                lbSuggestion.Visibility = Visibility.Visible;
            }
            else if (txtUserName.Text.Equals(""))
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                lbSuggestion.ItemsSource = null;
            }
            else
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                lbSuggestion.ItemsSource = null;
            }
        }
        #region ListBox-SelectionChanged-lbSuggestion
        private void lbSuggestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbSuggestion.ItemsSource != null)
            {
                lbSuggestion.Visibility = Visibility.Collapsed;
                txtUserName.TextChanged -= new TextChangedEventHandler(txtUserName_TextChanged);
                if (lbSuggestion.SelectedIndex != -1)
                {
                    txtUserName.Text = lbSuggestion.SelectedItem.ToString();
                }
                txtUserName.TextChanged += new TextChangedEventHandler(txtUserName_TextChanged);
            }
        }
        #endregion
        private void txtUserName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            crcProgress.Visibility = Visibility.Visible;          
            dispatcherTimer.Interval = dispatcherTimer.Interval + adtime;
            if (dispatcherTimer.Interval == new TimeSpan(0, 0, 4))
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0);
                crcProgress.Visibility = Visibility.Hidden;
                dispatcherTimer.Stop();
                SplashWindow sp = new SplashWindow(txtUserName.Text);
            
                this.Close();
                sp.ShowDialog();
            }
        }
     
    }
}
