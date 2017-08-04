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
using System.Data;
using System.ComponentModel;
using Main;
using UIControls;
using UIControls.Common;
using UIContainer.Enum;


namespace UIContainer.Registration
{
    /// <summary>
    /// Interaction logic for UCRegistrationContainer.xaml
    /// </summary>
    public partial class UCRegistrationContainer : UserControl, INotifyPropertyChanged
    {
        MainRegistration objMain = null;      
        DataSet dsToSave = new DataSet();
        DataRow dr;
        public UCRegistrationContainer()
        {
            try
            {
                InitializeComponent();
                FillBloodGroup();
                FillRegDetails();
                XSD.RegDetails objXSD = new XSD.RegDetails();
                DataToSave = objXSD.TEMP_MAST_REGISTRATION.Clone(); 
                dr = DataToSave.NewRow();
                DataToSave.Rows.Add(dr);
                DataToSave.AcceptChanges();
                //For Understand the Concept
                Commands com = new Commands();
                TypeSelectionCriteria = new DataTable();
                TypeSelectionCriteria = com.EnumToDataTable(typeof(GenderTest), "KEY", "VALUE");
                this.DataContext = this;
                ClearData();
            }
            catch (Exception)
            {
                throw;
            }

        }

        int _selectedValueHeader;
        public int SelectedValueHeader
        {
            get
            {
                return _selectedValueHeader;
            }
            set
            {
                _selectedValueHeader = value;
                OnPropertyChanged("SelectedValueHeader");

            }
        }
        private DataRowView _selectedbloodgroupHeader;
        public DataRowView SelectedBloodGroupHeader
        {
            get { return _selectedbloodgroupHeader; }
            set
            {
                _selectedbloodgroupHeader = value;
                if (_selectedValueHeader != null)
                {
                    if (SelectedBloodGroupHeader != null)
                    {
                        object[] arrayObj = SelectedBloodGroupHeader.Row.ItemArray;
                        SelectedValueHeader = Convert.ToInt32(arrayObj[0]);

                        if (DataToSave != null)
                        {
                            DataToSave.Rows[0]["REG_BLOOD_GROUP"] = SelectedValueHeader.ToString();
                        }

                    }
                }
            }
        }
        DataTable _dtbldgrpHeader;
        public DataTable BloodGroupHeader
        {
            get
            {
                return _dtbldgrpHeader;
            }
            set
            {
                _dtbldgrpHeader = value;
                OnPropertyChanged("BloodGroupHeader");

            }
        }
        DataTable DtValidateSave = new DataTable();
        public DataTable dtForValidateSave
        {
            get { return DtValidateSave; }
            set
            {
                DtValidateSave = value;
                OnPropertyChanged("dtForValidateSave");
            }
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
        private int REGID;
        public int PK_REG_ID
        {
            get { return REGID; }
            set
            {
                REGID = value;
                OnPropertyChanged("PK_REG_ID");
              
            }
        }
        private DataRowView REGNAME;
        public DataRowView REG_USER_NAME
        {
            get { return REGNAME; }
            set
            {
                REGNAME = value;
                OnPropertyChanged("REG_USER_NAME");
                if (REGNAME != null)
                {
                    object[] arrayObj = REG_USER_NAME.Row.ItemArray;
                    int id = Convert.ToInt32(arrayObj[0]);
                    MainRegistration objMain = new MainRegistration();
                    DataTable dtDetails = objMain.FetchRegDetailsById(id);
                    DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;
                    foreach (DataRow row in dtDetails.Rows)
                    {
                        SelectedValueHeader = Convert.ToInt32(row[4]);
                        int Gend = Convert.ToInt32(row[2]);
                        if (Gend == 1)
                        {
                            IsMale = true;
                        }
                        else if (Gend == 2)
                        {
                            IsFeMale = true;
                        }
                        TbiRegEntrySelected = true;
                    }
                    if (btnContent.ToLower() == "add")
                    {
                        btnContent = "Update";
                    }
                    DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;
                    DataToSave.AcceptChanges();
                    DataToSave.Rows[0].SetModified();
                    DataRow dr = DataToSave.Rows[0];

                }
            }
        }
        bool _isVisibleBGName=false;
        public bool IsVisibleBGName
        {
            get
            {
                return _isVisibleBGName;
            }
            set
            {
                _isVisibleBGName = value;
                OnPropertyChanged("IsVisibleBGName");

            }
        }


        private bool suppress;
        private GenderTest _yt;
        public GenderTest MaleOrFemale
        {
            get
            {
                return _yt;
            }
            set
            {
                if (_yt != value && !suppress)
                {
                    _yt = value;
                    suppress = true;
                    OnPropertyChanged("MaleOrFemale");
                    suppress = false;
                }
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
        //btnContent
        string _btnContent = "Add";
        public string btnContent
        {
            get
            {
                return _btnContent;
            }
            set
            {
                _btnContent = value;
                OnPropertyChanged("btnContent");

            }
        }
        string _mobNum;
        public string MobNum
        {
            get
            {
                return _mobNum;
            }
            set
            {
                _mobNum = value;
                OnPropertyChanged("MobNum");

            }
        }
        bool _isMale = false;
        public bool IsMale
        {
            get
            {
                return _isMale;
            }
            set
            {
                _isMale = value;
                OnPropertyChanged("IsMale");
                if (_isMale)
                {
                    if (DataToSave != null && Convert.ToString(DataToSave.Rows[0]["REG_GENDER"]) == string.Empty)
                    {
                        DataToSave.Rows[0]["REG_GENDER"] = 1.ToString();

                    }

                    IsFeMale = false;
                }

            }
        }
        bool _isFemale = false;
        public bool IsFeMale
        {
            get
            {
                return _isFemale;
            }
            set
            {
                _isFemale = value;
                OnPropertyChanged("IsFeMale");
                if (_isFemale)
                {
                    if (DataToSave != null && Convert.ToString(DataToSave.Rows[0]["REG_GENDER"].ToString()) == string.Empty)
                    {
                        DataToSave.Rows[0]["REG_GENDER"] = 2.ToString();
                    }
                    IsMale = false;
                }

            }
        }
        bool _tbiRegEntrySelected = true;
        public bool TbiRegEntrySelected
        {
            get
            {
                return _tbiRegEntrySelected;
            }
            set
            {
                _tbiRegEntrySelected = value;
                OnPropertyChanged("TbiRegEntrySelected");

            }
        }
        DataTable _dtdataTosave;
        public DataTable DataToSave
        {
            get
            {
                return _dtdataTosave;
            }
            set
            {
                _dtdataTosave = value;
                OnPropertyChanged("DataToSave");

            }
        }
        DataRow _dr;
        public DataRow dataRow
        {
            get
            {
                return _dr;
            }
            set
            {
                _dr = value;
                OnPropertyChanged("dataRow");

            }
        }
        DateTime _dateTime;
        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
                OnPropertyChanged("DateTime");

            }
        }
        private DataTable typeSelectionCriteria;
        public DataTable TypeSelectionCriteria
        {
            get { return typeSelectionCriteria; }
            set
            {
                typeSelectionCriteria = value;
                OnPropertyChanged("TypeSelectionCriteria");
            }
        }
        #region methods
        private void FillBloodGroup()
        {
            objMain = new MainRegistration();
            BloodGroupHeader = objMain.FetchAccountInitial(1);

        }
        private void FillRegDetails()
        {
            objMain = new MainRegistration();
            regDetails = objMain.FetchRegDetails();
           

        }
        #endregion
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
        private void CmdFormatSaveDetails_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataTable dt=new DataTable();
                CommanMethod(dt);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Set_Data()
        {
            try
            {
                ClearData();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        private void CmdClearExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ClearData();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void CmdDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(DataToSave.Rows[0]["PK_REG_ID"]).Trim()))
                {
                    string res = HisMessageBox.ShowMessage("Are you sure want to delete these details ??");
                    if (res == 1.ToString())
                    {
                        DataTable dt = DataToSave.Copy();
                        DataToSave.Rows[0].Delete();
                        CommanMethod(dt);
                    }
                }
                else
                {
                    HisMessageBox.ShowErrorMessage("PLEASE select Registration deatils to DELETE");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool Validate()
        {
            try
            {
                bool IsSuccess = true;
                if (IsSuccess && string.IsNullOrEmpty(Convert.ToString(DataToSave.Rows[0]["REG_USER_NAME"]).Trim()))
                {
                    HisMessageBox.ShowErrorMessage("Please enter Register Name");
                    IsSuccess = false;
                }
                if (IsSuccess && string.IsNullOrEmpty(Convert.ToString(DataToSave.Rows[0]["REG_MOB_NUM"]).Trim()))
                {
                    HisMessageBox.ShowErrorMessage("Please provide mobile number");
                    IsSuccess = false;
                }
                if (IsSuccess && SelectedValueHeader == 0)
                {
                    HisMessageBox.ShowErrorMessage("Please Select Blood Group");
                    IsSuccess = false;
                }
                return IsSuccess;
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        private void ClearData()
        {
            try
            {
                SelectedValueHeader = 0;
                IsMale = IsFeMale = false;
                RegName = string.Empty;
                if (btnContent.ToLower() == "update")
                {
                    btnContent = "Add";
                }
                DataTable dtDetails = objMain.FetchRegDetailsById(0);
                DataRow dr = dtDetails.NewRow();
                dtDetails.Rows.Add(dr);
                DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;
                DataToSave.AcceptChanges();
                DataToSave.Rows[0].SetAdded();
            }
            catch (Exception)
            {
                throw;
            }
           
        }
        private void CommanMethod(DataTable dt)
        {
            MainRegistration objForSaveUpdate = new MainRegistration();
            try
            {
                DataRow row = DataToSave.Rows[0];
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        if (Validate())
                        {
                            dsToSave.Clear();
                            dsToSave.Merge(DataToSave);
                            DataRow rw = regDetails.AsEnumerable().FirstOrDefault(tt => (tt.Field<string>("REG_MOB_NUM") == DataToSave.Rows[0]["REG_MOB_NUM"].ToString()) && (tt.Field<string>("REG_USER_NAME") == DataToSave.Rows[0]["REG_USER_NAME"].ToString()));
                            if (rw != null)
                            {
                                string message = HisMessageBox.ShowErrorMessage("The Record already exist");
                            }
                            else
                            {
                                if (objForSaveUpdate.CommanMethodForSaveUpdate(dsToSave, row.RowState))
                                {
                                    HisMessageBox.ShowErrorMessage("The Record inserted successfully");
                                    Set_Data();
                                    FillRegDetails();
                                }
                                else
                                {
                                    HisMessageBox.ShowErrorMessage("Error occured while inserting the Records");
                                }
                            }
                        }
                        break;
                    case DataRowState.Deleted:
                        if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["PK_REG_ID"]).Trim()))
                        {
                            dsToSave.Clear();
                            dsToSave.Merge(dt);
                            if (objForSaveUpdate.CommanMethodForSaveUpdate(dsToSave, row.RowState))
                            {
                                HisMessageBox.ShowErrorMessage("The Record Deleted successfully");
                                DataToSave.RejectChanges();
                                Set_Data();
                                FillRegDetails();
                            }
                            else
                            {
                                HisMessageBox.ShowErrorMessage("Error occured while deleting the Records");
                            }
                        }
                        break;
                    case DataRowState.Detached:
                        break;
                    case DataRowState.Modified:
                        if (Validate())
                        {
                            dsToSave.Clear();
                            dsToSave.Merge(DataToSave);
                            //create the instance of MainRegistration class (in MAIN) and pass the parameter through the instance of method CommanMethodForSaveUpdate()
                            if (objForSaveUpdate.CommanMethodForSaveUpdate(dsToSave, row.RowState))
                            {
                                HisMessageBox.ShowErrorMessage("The Record Updated successfully");
                                Set_Data();
                                FillRegDetails();
                            }
                            else
                            {
                                HisMessageBox.ShowErrorMessage("Error occured while updating the Records");
                            }
                        }
                        break;
                    case DataRowState.Unchanged:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}
