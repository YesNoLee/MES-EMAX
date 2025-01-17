﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using System.Security.Cryptography;
using System.Drawing.Text;
using System.IO;
using EMAX_Monitoring.Properties;
using System.Net;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Configuration;
using System.Threading;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace EMAX_Monitoring
{
    public partial class Change_Config : DevExpress.XtraEditors.XtraForm
    {
        private static SqlConnection conn = null;

        public Change_Config()
        {
            InitializeComponent();
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(BaseWaitForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption("주소 변경");
            SplashScreenManager.Default.SetWaitFormDescription("변경 중...");

            try
            {
                string sDBConnString = "Server=" + txt_IP.Text + "," + txt_Port.Text + ";Integrated Security=false;Initial ";
                sDBConnString += "Catalog=" + txt_DB.Text + ";";
                sDBConnString += "User ID=" + txt_ID.Text + ";";
                sDBConnString += "Password=" + txt_PW.Text;

                conn = new SqlConnection(sDBConnString);

                conn.Open();

                conn.Close();
                SplashScreenManager.CloseForm(false);

                Configurations.SetConfig("DBConnstring", sDBConnString);

                DbHelp.Clear();
                XtraMessageBox.Show("데이터베이스 접속 주소가 변경되었습니다");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("데이터베이스 접속 정보가 잘못 되었습니다");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Change_Config_Load(object sender, EventArgs e)
        {
            //데이터베이스 접속 주소
            string sDBConnString = Configurations.GetConfig("DBConnstring");

            string[] DBConn = sDBConnString.Split(';');

            string sUrl = DBConn[0].Substring(DBConn[0].IndexOf("=") + 1);
            string sIP = sUrl.Substring(0, sUrl.IndexOf(","));
            string sPort = sUrl.Substring(sUrl.IndexOf(",") + 1);

            string sDB = DBConn[2].Substring(DBConn[2].IndexOf("=") + 1);
            string sID = DBConn[3].Substring(DBConn[3].IndexOf("=") + 1);
            string sPW = DBConn[4].Substring(DBConn[4].IndexOf("=") + 1);

            txt_IP.Text = sIP;
            txt_Port.Text = sPort;
            txt_ID.Text = sID;
            txt_PW.Text = sPW;
            txt_DB.Text = sDB;
        }
    }
}