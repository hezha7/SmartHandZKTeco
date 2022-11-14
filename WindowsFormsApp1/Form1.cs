using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zkemkeeper;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            myTimer();
           // deleteData();
        }

        public void deleteData()
        {
            zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();
            var isconnected = axCZKEM1.Connect_Net("192.168.1.201", 4370);
            for (int j = 0; j < 5000; j++)
            {
                axCZKEM1.SSR_DeleteEnrollData(0, j.ToString(), 12);
            }
            axCZKEM1.RefreshData(0);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(0, true);
        }


        public static void myTimer()
        {
            zkemkeeper.CZKEM axCZKEM1 = new zkemkeeper.CZKEM();
            var isconnected = axCZKEM1.Connect_Net("192.168.1.201", 4370);

            for (int j = 0; j < 5000; j++)
            {
                axCZKEM1.SSR_DeleteEnrollData(0, j.ToString(), 12);

            }
            axCZKEM1.RefreshData(0);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(0, true);

            try
            {
                MySql.Data.MySqlClient.MySqlConnection conn;
                string myConnectionString;
                myConnectionString = "server=localhost;uid=root;" + "pwd='';database=smartgym";
                string sql = "SELECT PLAYERINFO.PLAYERID as id,PLAYERINFO.NAME as name, PLAYERINFO.PLAYERCODE as cardnumber from PLAYERINFO left join PLAYER_PACKAGES on PLAYERINFO.PLAYERID=PLAYER_PACKAGES.PLAYERID WHERE player_packages.EXPIREDATE>CURRENT_DATE group by playerinfo.PLAYERCODE;";
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                var stm = "SELECT VERSION()";
                var cmd = new MySqlCommand(sql, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();
                int i = 0;
                axCZKEM1.RefreshData(0);
                axCZKEM1.EnableDevice(0, false);
                while (rdr.Read())
                {
                    i++;
                    bool Enabled = true;
                    string EnrollNumber = rdr.GetString("id");
                    string Name = rdr.GetString("name");
                    int Privilege = 1;
                    axCZKEM1.SetStrCardNumber(rdr.GetString("cardnumber"));
                    var a = axCZKEM1.SSR_SetUserInfo(0, i.ToString(), Name, "smart2017", 1, true);
                }
                axCZKEM1.RefreshData(0);//the data in the device should be refreshed
                axCZKEM1.EnableDevice(0, true);
                Console.WriteLine(i+" data is insserted");

                throw new Exception();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {


        }


    }

}




