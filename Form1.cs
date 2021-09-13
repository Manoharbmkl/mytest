using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace PressKyLicense
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime cDate = DateTime.Now;
            DateTime edDate = dtpEndDate.Value;
            DBAccess db = null;
            DataTable dt = null;
            

            string query = string.Empty;
            string appName = "PressKy";
            db = new DBAccess();
            object rowIdentifier;
        //    query = "Select * from AppDetails";
            dt = db.ExecuteDataTable("GetAppDetails", false);
            int appno = 1;
            if (dt.Rows.Count > 0)
            {
                appno = Convert.ToInt32(dt.Rows[0][0].ToString());
                appno = appno + 1;

                //string query1 = "insert into AppDetails(AppName,StartDate,EndDate) values(' ColourGraphics ','" + cDate + "','" + edDate + "')";
                string squery = "Delete from AppDetails";
                db.ExecuteNonQuery(squery);

            }
            db.AddParameter("@Appno", appno );
            db.AddParameter("@AppName", appName);
            db.AddParameter("@StartDate", DateTime.Now);
            db.AddParameter("@EndDate", Convert.ToDateTime(dtpEndDate.Value));

            string query1 = "insert into AppDetails(AppName,StartDate,EndDate) values('PressKy','" + DateTime.Now + "','" + Convert.ToDateTime(dtpEndDate.Value) + "')"; 
            if (db.ExecuteNonQuery(query1,out rowIdentifier)>0)
                MessageBox.Show("Success");
            
            //}
        }
    }
}
