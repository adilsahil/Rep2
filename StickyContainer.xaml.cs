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
using System.Data;
using System.ComponentModel;
using UIControls;
using Main;
using UIContainer.Enum;
using System.Collections.ObjectModel;

namespace UIContainer.Registration
{
    /// <summary>
    /// Interaction logic for StickyContainer.xaml
    /// </summary>
    public partial class StickyContainer : UserControl, INotifyPropertyChanged
    {
        MainRegistration objMain = null;
        DataSet dsToSave = new DataSet();
        DataRow dr;
        public StickyContainer()
        {

            InitializeComponent();
            FillStickyDetails();
            objMain = new MainRegistration();
            XSD.RegDetails objXSD = new XSD.RegDetails();
            DataToSave = objXSD.TEMP_STICKYNOTES.Clone();
            dr = DataToSave.NewRow();
            DataToSave.Rows.Add(dr);
            DataToSave.AcceptChanges();
            ClearData();
            this.DataContext = this;
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

        private DataTable StickDetails;
        public DataTable stickDetails
        {
            get { return StickDetails; }
            set
            {
                StickDetails = value;
                OnPropertyChanged("stickDetails");
            }
        }




       
        string _btnContent = "Save";
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
        private DataRowView stk_NOTES;
        public DataRowView STICKY_NOTES
        {
            get { return stk_NOTES; }
            set
            {
                stk_NOTES = value;
                OnPropertyChanged("STICKY_NOTES");
                if (STICKY_NOTES != null)
                {
                    object[] arrayObj = STICKY_NOTES.Row.ItemArray;
                    int id = Convert.ToInt32(arrayObj[0]);
                    MainRegistration objMain = new MainRegistration();
                    DataTable dtDetails = objMain.FetchStickyId(id);
                    DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;                   
                    if (btnContent.ToLower() == "save")
                    {
                        btnContent = "Update";
                    }
                    DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;
                    DataToSave.AcceptChanges();
                    DataToSave.Rows[0].SetModified();
                    DataRow dr = DataToSave.Rows[0];
                    TbiRegEntrySelected = true;

                }
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
        private void FillStickyDetails()
        {
            objMain = new MainRegistration();
            stickDetails = objMain.FetchStickyDetails();

        }
        private bool Validate()
        {
            try
            {
                bool IsSuccess = true;
                if (IsSuccess && string.IsNullOrEmpty(Convert.ToString(DataToSave.Rows[0]["STICKY_NOTES"]).Trim()))
                {
                    HisMessageBox.ShowErrorMessage("Please enter NOTES to Save");
                    IsSuccess = false;
                }               
                return IsSuccess;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void CmdFormatSaveDetails_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (Validate())
                {
                    MainRegistration objForSaveUpdate = new MainRegistration();
                    DataTable dt = new DataTable();
                    DataRow row = DataToSave.Rows[0];
                    switch (row.RowState)
                    {
                            case DataRowState.Added:
                            DataToSave.Rows[0]["STICKY_DATE"] = DateTime.Today;
                            dsToSave.Clear();
                            dsToSave.Merge(DataToSave);
                            if (objForSaveUpdate.CommanMethodForSaveUpdateSticky(dsToSave, row.RowState))
                            {
                                HisMessageBox.ShowErrorMessage("The Record inserted successfully");
                                FillStickyDetails();
                                ClearData();
                            }
                            else
                            {
                                HisMessageBox.ShowErrorMessage("Error occured while inserting the Records");
                            }
                            break;
                        case DataRowState.Deleted:
                            break;
                        case DataRowState.Detached:
                            break;
                        case DataRowState.Modified:                           
                            if (string.IsNullOrEmpty(Convert.ToString((DataToSave.Rows[0]["STICKY_DATE"])).Trim()))
                            {
                                DataToSave.Rows[0]["STICKY_DATE"] = DateTime.Today;
                            }
                            dsToSave.Clear();
                            dsToSave.Merge(DataToSave);
                            if (objForSaveUpdate.CommanMethodForSaveUpdateSticky(dsToSave, row.RowState))
                            {
                                HisMessageBox.ShowErrorMessage("The Record Updated successfully");
                                FillStickyDetails();
                                ClearData();
                            }
                            else
                            {
                                HisMessageBox.ShowErrorMessage("Error occured while updating the Records");
                            }
                            break;
                        case DataRowState.Unchanged:
                            break;
                        default:
                            break;
                    }
                    ClearData();
                }
                
               
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
        private void ClearData()
        {
            try
            {
                DataTable dtDetails = objMain.FetchStickyId(0);
                DataRow dr = dtDetails.NewRow();
                dtDetails.Rows.Add(dr);
                DataToSave.Rows[0].ItemArray = dtDetails.Rows[0].ItemArray;
                DataToSave.AcceptChanges();
                if (btnContent.ToLower() == "update")
                {
                    btnContent = "Save";
                }
                DataToSave.Rows[0].SetAdded();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
