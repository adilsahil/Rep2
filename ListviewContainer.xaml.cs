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
using System.Data;
using System.ComponentModel;
using UIControls;
using Main;
using UIContainer.Enum;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
namespace UIContainer.ListviewCont
{
    /// <summary>
    /// Interaction logic for ListviewContainer.xaml
    /// </summary>
    public partial class ListviewContainer : UserControl, INotifyPropertyChanged
    {
        MainRegistration objMain = null;
        public DataTable dt = new DataTable();
        public ListviewContainer()
        {
            InitializeComponent();
            FillStickyDetails();
            this.DataContext=this;
        }
        private List<StickDetails> listDetails=new List<StickDetails>();
        public List<StickDetails> ListDet
        {
            get { return listDetails; }
            set
            {               
                listDetails = value;             
                OnPropertyChanged("ListDet");
            }
        }
        private DataTable dtDetails ;
        public DataTable DTDet
        {
            get { return dtDetails; }
            set
            {
                dtDetails = value;
                OnPropertyChanged("DTDet");
            }
        }
        private DataTable dtDetailsSecondlist;
        public DataTable DTDetSecond
        {
            get { return dtDetailsSecondlist; }
            set
            {
                dtDetailsSecondlist = value;
                OnPropertyChanged("DTDetSecond");
            }
        }
        //STICKY_NOTES
        private string _stkNotes;
        public string STCKNOTES
        {
            get
            {
                return _stkNotes;
            }
            set
            {
                _stkNotes = value;
                OnPropertyChanged("STCKNOTES");

            }
        }
        private int _stkID;
        public int STICKID
        {
            get
            {
                return _stkID;
            }
            set
            {
                _stkID = value;
                OnPropertyChanged("STICKID");

            }
        }
        private bool stckCheck;
        public bool SELECT
        {
            get
            {
                return stckCheck;
            }
            set
            {
                stckCheck = value;
                OnPropertyChanged("SELECT");

            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        public DateTime DatetimeProperty
        {
            get { return (DateTime)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register("DatetimeProperty", typeof(DateTime), typeof(ListviewContainer), new FrameworkPropertyMetadata(DateTime.Now, OnCurrentTimePropertyChanged,OnCoerceTimeProperty));
        public int STICKYNOTES_ID
        {
            get { return (int)GetValue(stickProperty); }
            set { SetValue(stickProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StickID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty stickProperty =
        DependencyProperty.Register("STICKYNOTES_ID", typeof(int), typeof(ListviewContainer), new FrameworkPropertyMetadata(0, OnCurrentTimePropertyChanged_SelectAll));
        public string STICKY_NOTES
        {
            get { return (string)GetValue(STICKY_NOTESProperty); }
            set { SetValue(STICKY_NOTESProperty, value); }
        }
        // Using a DependencyProperty as the backing store for STICKY_NOTES.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty STICKY_NOTESProperty =
        DependencyProperty.Register("STICKY_NOTES", typeof(string), typeof(ListviewContainer), new FrameworkPropertyMetadata("str"));
        /// <summary>
        /// value Changed Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>




        public bool SELECTALL
        {
            get { return (bool)GetValue(SELECTProperty); }
            set { SetValue(SELECTProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SELECT.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SELECTProperty =
            DependencyProperty.Register("SELECTALL", typeof(bool), typeof(ListviewContainer), new FrameworkPropertyMetadata(false, OnCurrentTimePropertyChanged_SelectAll));
         
        private static void OnCurrentTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ListviewContainer control = source as ListviewContainer;
            DateTime time = (DateTime)e.NewValue;
            if (time > DateTime.Today)
            {
                HisMessageBox.ShowErrorMessage("Value Changed Callback  :  Date is not greater than Today");
                control.DatetimeProperty = DateTime.Today;
            }
            else
            {
               // var result = control.ListDet.OfType<StickDetails>().Where(s => s.STCKDATE == time.Date);
                control.FillStickyDetails();
                control.ListDet = control.ListDet.FindAll(s => s.STCKDATE == time.Date);
            }

        }
        /// <summary>
        /// value Changed Callback For StickyList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static void OnCurrentTimePropertyChanged_SelectAll(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ListviewContainer control = source as ListviewContainer;
            bool Ischecked = (bool)e.NewValue;
            foreach (DataRow row in control.DTDet.Rows)
            {
                if (Ischecked == true)
                {
                    row["SELECT"] = true;
                }
                else
                {
                    row["SELECT"] = false;
                }
            }
            DataTable newDt = control.DTDet;
            control.DTDetSecond.Rows.Clear();
            DataTable insertDt = new DataTable();
            insertDt = newDt.Clone();
            foreach (DataRow row in newDt.Rows)
            {
                if (Convert.ToBoolean(row["SELECT"]) == true)
                {
                    insertDt.ImportRow(row);
                    control.DTDetSecond = insertDt.Copy();
                }
            }
            
        }
        /// <summary>
        /// Coerce Value Callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static object OnCoerceTimeProperty(DependencyObject sender, object data)
        {
            if ((DateTime)data > DateTime.Now)
            {
              //  data = DateTime.Now;
            }
            return data;
        }
        /// <summary>
        /// Validation Callback
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool OnValidateTimeProperty(object data)
        {
            return data is DateTime;
        }
        private void FillStickyDetails()
        {
            
            objMain = new MainRegistration();
           
            XSD.RegDetails objXSD = new XSD.RegDetails();
            DTDet = objXSD.TEMP_STICKYNOTES_TEST.Clone();          
            DTDetSecond = DTDet.Clone();
            DataTable dt = objMain.FetchStickyDetails();
            ListDet = new List<StickDetails>();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                StickDetails item = new Enum.StickDetails();
                item.STICKID = Convert.ToInt32(row[0]);
                item.STCKNOTES = Convert.ToString(row[1]);
                item.STCKDATE = Convert.ToDateTime(row[2]);
                ListDet.Add(item);
                DataRow drs = DTDet.NewRow();
                DTDet.Rows.Add(drs);
                DTDet.Rows[i]["SELECT"] = false;
                DTDet.Rows[i]["STICKYNOTES_ID"] = row[0];
                DTDet.Rows[i]["STICKY_NOTES"] = (Convert.ToString(row[1]));
                DTDet.Rows[i]["STICKY_DATE"] = row[2];              
                i++;
            }


           
           
            //ListView lv = new ListView();
            //lv.ItemsSource = ListDet;
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lv.ItemsSource);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("STCKNOTES");
            //view.GroupDescriptions.Add(groupDescription);

           


        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {  
            ListviewContainer control = sender as ListviewContainer;
            DataTable newDt = control.DTDet;
            DTDetSecond.Rows.Clear();
            DataTable insertDt = new DataTable();
            insertDt = newDt.Clone();           
            foreach (DataRow row in newDt.Rows)
            {
                if (Convert.ToBoolean(row["SELECT"])==true)
                {
                    insertDt.ImportRow(row);
                    DTDetSecond = insertDt.Copy();
                }
            }
        }

       
    }
}
