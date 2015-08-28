using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

//Using Sql
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=.\\SQLEXPRESS;Integrated Security=True;";

            SqlConnection c = new SqlConnection(ConnectionString);
            c.Open();
            SqlCommand sql = new SqlCommand("EXEC sp_databases", c);

            try
            {
                using (SqlDataReader d = sql.ExecuteReader())
                {

                    listBox1.Items.Clear();

                    int records_pub = 0;

                    while (d.Read())
                    {
                        listBox1.Items.Add(string.Format("{0}",d.GetString(0)));
                        records_pub++;
                    }
                    
                    toolStripStatusLabel1.Text = "Databases: " + records_pub.ToString();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            
            string query = "SELECT ROUND(SUM(mf.size) * 8 / 1024, 0) Size_MBs FROM sys.master_files mf INNER JOIN sys.databases d ON d.database_id = mf.database_id GROUP BY d.name ORDER BY d.name";
            SqlCommand querycommand = new SqlCommand(query, c);
            SqlDataReader querycommandreader = querycommand.ExecuteReader();

            while (querycommandreader.Read())
            {
                chart1.Series[0].Points.AddY(querycommandreader.GetInt32(0));
            }

            Series thesqlresultsplease = new Series();

            chart1.Series[0].XValueMember = thesqlresultsplease.XValueMember;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=.\\SQLEXPRESS;Integrated Security=True;";

            SqlConnection c = new SqlConnection(ConnectionString);
            c.Open();

            //string aaa = String.Empty;

            string bbb = listBox1.SelectedItem.ToString();

            //aaa = bbb.Substring(bbb.Length - 5, 4);

            SqlCommand sql = new SqlCommand("SELECT TABLE_NAME FROM " + bbb + ".INFORMATION_SCHEMA.Tables;", c);

            SqlDataReader d = sql.ExecuteReader();
            
            listBox2.Items.Clear();
            
            int record_titles = 0;
            while (d.Read())
            {
                
                listBox2.Items.Add(d.GetString(0));
                record_titles++;
                
            }

            toolStripStatusLabel2.Text = "Tables: " + record_titles.ToString();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
