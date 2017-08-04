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
using System.Data;

namespace UIContainer.SampleGICont
{
    /// <summary>
    /// Interaction logic for SampleGIContainer.xaml
    /// </summary>
    public partial class SampleGIContainer : UserControl, INotifyPropertyChanged
    {
        Main.MainRegistration objMain = null;        
        public SampleGIContainer()
        {
            InitializeComponent();
            XSD.RegDetails objXSD =new XSD.RegDetails();
            regDetails = objXSD.TEMP_MAST_REGISTRATION_GRID.Clone();                
            FillRegDetails();
            this.DataContext = this;
        }

        private DataTable RegDetails;
        public DataTable regDetails
        {
            get { return RegDetails; }
            set
            {
                RegDetails = value;
                OnPropertyChanged("regDetails");
            }
        }

        string _regName;
        public string RegName
        {
            get
            {
                return _regName;
            }
            set
            {
                _regName = value;
                OnPropertyChanged("RegName");
                regDetails.DefaultView.RowFilter = string.Format("REG_USER_NAME LIKE '%{0}%'", RegName);

            }
        }
        private void FillRegDetails()
        {
            objMain = new Main.MainRegistration();
            DataTable dt = objMain.FetchRegDetails();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                DataRow drs = regDetails.NewRow();
                regDetails.Rows.Add(drs);
                regDetails.Rows[i]["PK_REG_ID"] = row[0];
                regDetails.Rows[i]["REG_USER_NAME"] = row[1];
                regDetails.Rows[i]["REG_GENDER"] = Gender(Convert.ToInt32(row[2]));
                regDetails.Rows[i]["REG_MOB_NUM"] = row[3];
                regDetails.Rows[i]["REG_BLOOD_GROUP"] = BloodGroup(Convert.ToInt32(row[4]));
                i++;
            }
        }


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
    }
}
