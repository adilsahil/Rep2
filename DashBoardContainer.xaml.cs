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
using System.ComponentModel;
using Main;
using System.Data;
using System.Collections.ObjectModel;
using UIContainer.Enum;

namespace UIContainer.Dash
{
    /// <summary>
    /// Interaction logic for DashBoardContainer.xaml
    /// </summary>
    public partial class DashBoardContainer : UserControl, INotifyPropertyChanged
    {
        MainRegistration objMain = null;

        public DashBoardContainer()
        {

            DataContext = this;
            InitializeComponent();           
            FillRegDetails();
            FillAppointmentDetails();
        }
        private DataTable RegDetails;
        public List<ChartDetails> _ChartValues = new List<ChartDetails>();
        public List<ChartDetails> ChartValues
        {
            get { return _ChartValues; }
            set
            {
                _ChartValues = value;
                OnPropertyChanged("ChartValues");

            }
        }



        public DataTable regDetails
        {
            get { return RegDetails; }
            set
            {
                RegDetails = value;
                OnPropertyChanged("regDetails");

            }
        }


        private DataTable _dtAppointment;
        public DataTable dtAppointment
        {
            get { return _dtAppointment; }
            set
            {
                _dtAppointment = value;
                OnPropertyChanged("dtAppointment");

            }
        }

        private bool _blnkSelect = false;
        public bool tbBlankSelect
        {
            get { return _blnkSelect; }
            set
            {
                _blnkSelect = value;
                OnPropertyChanged("tbBlankSelect");
            }
        }



        private bool _detSelect = false;
        public bool tbDetSelect
        {
            get { return _detSelect; }
            set
            {
                _detSelect = value;
                OnPropertyChanged("tbDetSelect");
            }
        }


        private int _regCount;
        public int regCount
        {
            get { return _regCount; }
            set
            {
                _regCount = value;
                OnPropertyChanged("regCount");
            }
        }

        private string regDate;
        public string Reg_Date
        {
            get { return regDate; }
            set
            {
                regDate = value;
                OnPropertyChanged("Reg_Date");

            }
        }

        private void FillRegDetails()
        {
            objMain = new MainRegistration();
            regDetails = objMain.FetchRegDetails();
            regCount = regDetails.Rows.Count;

        }
        private void FillAppointmentDetails()
        {
            objMain = new MainRegistration();
            dtAppointment = objMain.FetchAppointmentDet(DateTime.Today);
            ChartValues = new List<ChartDetails>();
            if (dtAppointment.Rows.Count > 0)
            {
                tbBlankSelect = false;
                tbDetSelect = true;
                int i = 1;
                foreach (DataRow row in dtAppointment.Rows)
                {
                    try
                    {
                        ChartDetails chrtDet = new ChartDetails();
                        chrtDet.UserId = i;
                        chrtDet.UserName = Convert.ToString(row[1]);
                        chrtDet.Gender = Gender(Convert.ToInt32(row[2]));
                        chrtDet.bloodgroup = BloodGroup(Convert.ToInt32(row[4]));
                        chrtDet.RegDate = Convert.ToDateTime(row[5]);
                        ChartValues.Add(chrtDet);
                        i++;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                tbBlankSelect = true; ;
                tbDetSelect = false;
            }
            DateTime dt = DateTime.Today.Date;
            regDate = dt.ToString("dd-MMMM-yyyy");

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

        private string Gender(int id)
        {
            string Gender = string.Empty;
            switch (id)
            {
                case 1:
                    Gender = "Male";
                    break;
                case 2:
                    Gender = "Female";
                    break;
                default:
                    break;
            }
            return Gender;
        }
        private string BloodGroup(int id)
        {
            string BloodGroup = string.Empty;
            switch (id)
            {
                case 1:
                    BloodGroup = "O-";
                    break;
                case 2:
                    BloodGroup = "O+";
                    break;
                case 3:
                    BloodGroup = "A-";
                    break;
                case 4:
                    BloodGroup = "A+";
                    break;
                case 5:
                    BloodGroup = "B-";
                    break;
                case 6:
                    BloodGroup = "B+";
                    break;
                case 7:
                    BloodGroup = "AB-";
                    break;
                case 8:
                    BloodGroup = "AB+";
                    break;
            }
            return BloodGroup;
        }
    }
}
