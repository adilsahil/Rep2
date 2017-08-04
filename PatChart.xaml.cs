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
using Main;
using System.Data;

namespace UIControls.UserControls
{
    /// <summary>
    /// Interaction logic for PatChart.xaml
    /// </summary>
    public partial class PatChart : UserControl
    {
        public PatChart()
        {
            InitializeComponent();
            FillAppointmentDetails();
        }
        private void FillAppointmentDetails()
        {
            MainRegistration objMain = new MainRegistration();
            DataTable dattble = objMain.FetchAppointmentDet(DateTime.Today);
            List<ColumnDet> ColumnList = new List<ColumnDet>();
            foreach (DataRow row in dattble.Rows)
            {
                ColumnDet clmndata = new ColumnDet();              
                clmndata.UserId = Convert.ToInt32(row[2]);
                clmndata.UserName = Convert.ToString(row[1]);
                ColumnList.Add(clmndata);
            }
           
        }
        public class ColumnDet
        {
            public int UserId { get; set; }
            public string UserName { get; set; }

        }
    }
}
