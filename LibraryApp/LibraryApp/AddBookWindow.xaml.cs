using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryApp 
{
    public partial class AddBookWindow : Window
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryDB;Integrated Security=True;";

        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitle.Text) || string.IsNullOrWhiteSpace(TxtAuthor.Text))
            {
                MessageBox.Show("Название и Автор обязательны!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Books (Title, Author, Year, Genre) VALUES (@Title, @Author, @Year, @Genre)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", TxtTitle.Text);
                        cmd.Parameters.AddWithValue("@Author", TxtAuthor.Text);

                        int year;
                        if (int.TryParse(TxtYear.Text, out year))
                            cmd.Parameters.AddWithValue("@Year", year);
                        else
                            cmd.Parameters.AddWithValue("@Year", DBNull.Value);

                        cmd.Parameters.AddWithValue("@Genre", TxtGenre.Text);

                        cmd.ExecuteNonQuery();
                    }
                }
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления: " + ex.Message);
            }
        }
    }
}