using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;
namespace Controls.RegHedBL
{
    public class BLRegistrationHead
    {
       
        //Methods for Fething the Records
        public DataTable FetchInitialData(int valid, IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_SELECT_BLOOD_GROUP");
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_SELECT_BLOOD_GROUP", dataSet, new string[] { "TEMP_MAST_BLOOD" }, spParameterSet);
                table = dataSet.Tables["TEMP_MAST_BLOOD"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
        public DataTable FetchUserRegDetails(int valid, IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_SELECT_USERDET");
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_SELECT_USERDET", dataSet, new string[] { "TEMP_MAST_USER" }, spParameterSet);
                table = dataSet.Tables["TEMP_MAST_USER"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
        public DataTable FetchRegData(IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_SELECT_REGDETAILS");
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_SELECT_REGDETAILS", dataSet, new string[] { "TEMP_MAST_REGISTRATION" }, spParameterSet);
                table = dataSet.Tables["TEMP_MAST_REGISTRATION"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
        public DataTable FetchStickyData(IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_GET_STICKYNOTES");
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_GET_STICKYNOTES", dataSet, new string[] { "TEMP_STICKYNOTES" }, spParameterSet);
                table = dataSet.Tables["TEMP_STICKYNOTES"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }

        public DataTable FetchStickyDatabyDate(IDbConnection cn,DateTime dt)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_GET_STICKY_DATE");
                spParameterSet[2].Value = dt;
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_GET_STICKY_DATE", dataSet, new string[] { "TEMP_STICKYNOTES" }, spParameterSet);
                table = dataSet.Tables["TEMP_STICKYNOTES"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
        public DataTable FetchAppointmentData(IDbConnection cn, DateTime dt)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_SELECT_REG_DATE");
                spParameterSet[5].Value = dt;
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_SELECT_REG_DATE", dataSet, new string[] { "TEMP_MAST_REGISTRATION" }, spParameterSet);
                table = dataSet.Tables["TEMP_MAST_REGISTRATION"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
        public DataTable FetchRegDataByID(int ID, IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_GET_REGDETAILS");
                spParameterSet[0].Value = ID;
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_GET_REGDETAILS", dataSet, new string[] { "TEMP_MAST_REGISTRATION" }, spParameterSet);
                table = dataSet.Tables["TEMP_MAST_REGISTRATION"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }

        public DataTable FetchStickyDet(int ID, IDbConnection cn)
        {
            DataTable table;
            try
            {
                DataSet dataSet = new DataSet();
                IDbDataParameter[] spParameterSet = DAL.DALHelperParameterCache.GetSpParameterSet(cn, "TEMP_GET_STICKYNOTES_ID");
                spParameterSet[0].Value = ID;
                DALHelper.FillDataset(cn, CommandType.StoredProcedure, "TEMP_GET_STICKYNOTES_ID", dataSet, new string[] { "TEMP_STICKYNOTES" }, spParameterSet);
                table = dataSet.Tables["TEMP_STICKYNOTES"];
            }
            catch (Exception)
            {
                throw;
            }
            return table;
        }
     
      
    
      
      
      

        //Comman Method For Saving,Updating and Deleting the Registration Details
        public bool CommanMethod(DataSet dsSave, IDbTransaction trans, DataRowState rowState)
        {
            bool IsSuccess = false;
            switch (rowState)
            {
                case DataRowState.Added:
                    IsSuccess = Save(dsSave, trans);
                    break;
                case DataRowState.Modified:
                    IsSuccess = Update(dsSave, trans);
                    break;
                case DataRowState.Deleted:
                    IsSuccess = Delete(dsSave, trans);
                    break;               
                default:
                    break;
            }

            return IsSuccess;
        }



        public bool CommanMethodSticky(DataSet dsSave, IDbTransaction trans, DataRowState rowState)
        {
            bool IsSuccess = false;
            switch (rowState)
            {
                case DataRowState.Added:
                    IsSuccess = SaveSticky(dsSave, trans);
                    break;
                case DataRowState.Modified:
                    IsSuccess = UpdateSticky(dsSave, trans);
                    break;
                case DataRowState.Deleted:
                   
                    break;
                default:
                    break;
            }

            return IsSuccess;
        }

        //Methods for Save the Registration Details
        public bool Save(DataSet ds, IDbTransaction trans)
        {
            bool isSuccuss = false;
            try
            {
                if (ds.Tables.Contains("TEMP_MAST_REGISTRATION") && ds.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    isSuccuss = this.SaveAccHead(ds, trans);
                }
                return isSuccuss;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveSticky(DataSet ds, IDbTransaction trans)
        {
            bool isSuccuss = false;
            try
            {
                if (ds.Tables.Contains("TEMP_STICKYNOTES") && ds.Tables["TEMP_STICKYNOTES"].Rows.Count > 0)
                {
                    isSuccuss = this.SaveDetSticky(ds, trans);
                }
                return isSuccuss;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool SaveAccHead(DataSet dsScheme, IDbTransaction trans)
        {
            bool IsInsert = false;
            try
            {
                if (dsScheme.Tables.Contains("TEMP_MAST_REGISTRATION") && dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows)
                    {
                        IsInsert = InsertDtls(item, trans);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsInsert;
        }



        private bool SaveDetSticky(DataSet dsScheme, IDbTransaction trans)
        {
            bool IsInsert = false;
            try
            {
                if (dsScheme.Tables.Contains("TEMP_STICKYNOTES") && dsScheme.Tables["TEMP_STICKYNOTES"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsScheme.Tables["TEMP_STICKYNOTES"].Rows)
                    {
                        IsInsert = InsertSticky(item, trans);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsInsert;
        }
        private bool InsertDtls(DataRow dr, IDbTransaction trans)
        {
            try
            {
                IDbDataParameter[] paramMst = SetParameters(dr, trans, "TEMP_INSERT_REG_DETAILS");
                return DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TEMP_INSERT_REG_DETAILS", paramMst) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool InsertSticky(DataRow dr, IDbTransaction trans)
        {
            try
            {
                IDbDataParameter[] paramMst = SetParametersSticky(dr, trans, "TEMP_INSERT_STICKY");
                return DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TEMP_INSERT_STICKY", paramMst) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Methods for Update the Registration Details
        public bool Update(DataSet ds, IDbTransaction trans)//DataSet ds, DBOperations DBop, IDbTransaction trans
        {
            bool isSuccuss = false;
            try
            {
                if (ds.Tables.Contains("TEMP_MAST_REGISTRATION") && ds.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    isSuccuss = this.UpdateRegDetails(ds, trans);
                }
                return isSuccuss;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool UpdateRegDetails(DataSet dsScheme, IDbTransaction trans)
        {
            bool IsUpdate = false;
            try
            {
                if (dsScheme.Tables.Contains("TEMP_MAST_REGISTRATION") && dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows)
                    {
                        IsUpdate = UpdateDtls(item, trans);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsUpdate;
        }
        private bool UpdateDtls(DataRow dr, IDbTransaction trans)
        {
            try
            {
                IDbDataParameter[] paramMst = SetParameters(dr, trans, "TEMP_UPDATE_REG_DETAILS");
                return DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TEMP_UPDATE_REG_DETAILS", paramMst) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public bool UpdateSticky(DataSet ds, IDbTransaction trans)//DataSet ds, DBOperations DBop, IDbTransaction trans
        {
            bool isSuccuss = false;
            try
            {
                if (ds.Tables.Contains("TEMP_STICKYNOTES") && ds.Tables["TEMP_STICKYNOTES"].Rows.Count > 0)
                {
                    isSuccuss = this.UpdateStickyDetails(ds, trans);
                }
                return isSuccuss;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool UpdateStickyDetails(DataSet dsScheme, IDbTransaction trans)
        {
            bool IsUpdate = false;
            try
            {
                if (dsScheme.Tables.Contains("TEMP_STICKYNOTES") && dsScheme.Tables["TEMP_STICKYNOTES"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsScheme.Tables["TEMP_STICKYNOTES"].Rows)
                    {
                        IsUpdate = UpdateStckDtls(item, trans);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsUpdate;
        }


        private bool UpdateStckDtls(DataRow dr, IDbTransaction trans)
        {
            try
            {
                IDbDataParameter[] paramMst = SetParametersSticky(dr, trans, "TEMP_UPDATE_STICKY");
                return DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TEMP_UPDATE_STICKY", paramMst) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Methods for Delete the Registration Details
        public bool Delete(DataSet ds, IDbTransaction trans)
        {
            bool isSuccuss = false;
            try
            {
                if (ds.Tables.Contains("TEMP_MAST_REGISTRATION") && ds.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    isSuccuss = this.DeleteRegDetails(ds, trans);
                }
                return isSuccuss;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool DeleteRegDetails(DataSet dsScheme, IDbTransaction trans)
        {
            bool IsUpdate = false;
            try
            {
                if (dsScheme.Tables.Contains("TEMP_MAST_REGISTRATION") && dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows.Count > 0)
                {
                    foreach (DataRow item in dsScheme.Tables["TEMP_MAST_REGISTRATION"].Rows)
                    {
                        IsUpdate = DeleteDtls(item, trans);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return IsUpdate;
        }
        private bool DeleteDtls(DataRow dr, IDbTransaction trans)
        {
            try
            {
                IDbDataParameter[] paramMst = SetParameters(dr, trans, "TEMP_DELETE_REGDETILS");
                return DALHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "TEMP_DELETE_REGDETILS", paramMst) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Set The Parameters
        private IDbDataParameter[] SetParameters(DataRow dr, IDbTransaction trans, string procName)
        {
            try
            {
                IDbDataParameter[] paramMst = DALHelperParameterCache.GetSpParameterSet(trans, procName);
                if (paramMst != null)
                {
                    foreach (IDbDataParameter eachObject in paramMst)
                    {
                        switch (eachObject.ParameterName.ToUpper())
                        {
                            case "P_PK_REG_ID":
                                eachObject.Value = dr["PK_REG_ID"];
                                break;
                            case "P_REG_USER_NAME":
                                eachObject.Value = dr["REG_USER_NAME"];
                                break;
                            case "P_REG_GENDER":
                                eachObject.Value = dr["REG_GENDER"];
                                break;
                            case "P_REG_MOB_NUM":
                                eachObject.Value = dr["REG_MOB_NUM"];
                                break;
                            case "P_REG_BLOOD_GROUP":
                                eachObject.Value = dr["REG_BLOOD_GROUP"];
                                break;
                            case "P_REG_DATE":
                                eachObject.Value = dr["REG_DATE"];
                                break;

                            default:
                                break;
                        }
                    }
                }
                return paramMst;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private IDbDataParameter[] SetParametersSticky(DataRow dr, IDbTransaction trans, string procName)
        {
            try
            {
                IDbDataParameter[] paramMst = DALHelperParameterCache.GetSpParameterSet(trans, procName);
                if (paramMst != null)
                {
                    foreach (IDbDataParameter eachObject in paramMst)
                    {
                        switch (eachObject.ParameterName.ToUpper())
                        {
                            case "P_STICKYNOTES_ID":
                                eachObject.Value = dr["STICKYNOTES_ID"];
                                break;
                            case "P_STICKY_NOTES":
                                eachObject.Value = dr["STICKY_NOTES"];
                                break;
                            case "P_STICKY_DATE":
                                eachObject.Value = dr["STICKY_DATE"];
                                break;
                            default:
                                break;
                        }
                    }
                }
                return paramMst;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
