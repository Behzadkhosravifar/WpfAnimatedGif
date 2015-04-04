using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWaitSplash
{
    public partial class Form1 : BaseForm
    {
        public Form1()
        {
            InitializeComponent();

            OnStartupAction = OnStartup;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await InvokeAsync(OnStartup);

        }

        public void OnStartup()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            ServerTransmitter.CreateDatabaseAsync().GetAwaiter().GetResult();

            using (var sqlConn = new SqlConnection("Data Source=.;Initial Catalog=TestDB;Integrated Security=True"))
            {
                using (var cmd = sqlConn.CreateCommand())
                {
                    for (var i = 0; i < 10000; i++)
                    {
                        var rand = new Random();
                        cmd.CommandText = string.Format(@"INSERT INTO [TestTable]
                                                           ([Name]
                                                           ,[Mobile])
                                                     VALUES
                                                           ('Test'
                                                           ,{0})", rand.Next());

                        sqlConn.Open();

                        cmd.ExecuteNonQuery();

                        sqlConn.Close();
                    }
                }
            }

            using (var sqlConn = new SqlConnection("Data Source=.;Initial Catalog=TestDB;Integrated Security=True"))
            {
                using (var cmd = new SqlCommand("Select * From TestTable", sqlConn))
                {
                    sqlConn.Open();

                    var dt = new DataTable();

                    var da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    sqlConn.Close();

                    //if (dataGridView1.IsHandleCreated)
                    //    dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = dt));
                    dataGridView1.DataSource = dt;
                }
            }


        }
    }
}
