using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading;
namespace Assignment1
{
    public partial class Form1 : Form
    {

        static string connectionString = @"Data Source = (local); DataBase = simple; Integrated Security=true";
        private delegate void SafeCallDelegate(string text);
        private delegate void SafeCallDelegatedvg(object dgv);
         //string connectionString = string.Format(Ms, " ");
        //CallToChildThread
    /*    public string LableText
        { 
        get
            {
                return this.L1.Text;
            }
            set
            {
                this.L1.Text = value;
            }
        }

        */


        private void WriteTextSafe(string text)
        {
            if (L1.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteTextSafe);
                L1.Invoke(d, new object[] { text });
            }
            else
            {
                L1.Text = text;
            }
        }

        private void WritedgvSafe(object ob)
        {
            if (dgv1.InvokeRequired)
            {
                var d = new SafeCallDelegatedvg(WritedgvSafe);
                dgv1.Invoke(d, new object[] { ob });
            }
            else
            {
                dgv1.DataSource = ob;
            }
        }

        public void SecondThread()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Second Thread Call");
                    using (SqlConnection scon = new SqlConnection(connectionString))
                    {
                        scon.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Tb", scon);
                        DataTable dt = new DataTable();
                        sqlDa.Fill(dt);
                        WritedgvSafe(dt);


                    }
                    // the thread is paused for 5000 milliseconds
                    int sleepfor = 60000;

                    Console.WriteLine("Child Thread Paused for {0} seconds", sleepfor / 1000);
                    WriteTextSafe("loading");
                    Thread.Sleep(sleepfor);
                    Console.WriteLine(" Thread resumes");
                    WriteTextSafe("Done");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Thread Abort Exception" + e.ToString());
            }
            finally
            {
                Console.WriteLine("Couldn't catch the Thread Exception");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ThreadStart childref = new ThreadStart(SecondThread);
                Console.WriteLine("In Main: Creating the Second thread");


             /*   using (SqlConnection scon = new SqlConnection(connectionString))
                {
                    scon.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("Select * from Tb", scon);
                    DataTable dt = new DataTable();
                    sqlDa.Fill(dt);
                    dgv1.DataSource = dt;

                    dgv1.Refresh();
                }*/


                Thread Second = new Thread(childref);
                Second.Start();
                //Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(" Exception" + ex.ToString());
            }
        }
    }
}
