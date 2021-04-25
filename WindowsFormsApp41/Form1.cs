using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp41
{
    public partial class Form1 : Form
    {
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string connectionString = @"Data Source=SQL5103.site4now.net;Initial Catalog=DB_A71192_kolia4237;User Id=DB_A71192_kolia4237_admin;Password=r21423247;";
        string sql = "SELECT * FROM Beverages";

        public Form1()
        {
            InitializeComponent();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);

                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                // делаем недоступным столбец id для изменения
                dataGridView1.Columns["Id"].ReadOnly = false;
            }

        }
        // кнопка добавления
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
            ds.Tables[0].Rows.Add(row);
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            
            
            Form2 newForm = new Form2();
            newForm.Show();
        }
        // кнопка сохранения
        private void button3_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sql, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = new SqlCommand("__Beverages", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 15, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@manufacturer", SqlDbType.NVarChar, 20, "Manufacturer"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@prace", SqlDbType.Money, 0, "Prace"));
                

                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 0, "Id");
                parameter.Direction = ParameterDirection.Output;
                
                adapter.Update(ds);
            }
        }

        
    }
}
