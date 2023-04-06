using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
//для работы с БД
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QueCRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid(); // для загрузки таблицы
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) // for move window
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //Сюда вставляем скопированную строку подключения
        SqlConnection con = new SqlConnection("Data Source=ARS;Initial Catalog=QueCRUD;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        //создаем метод для очистки textbox - ов
        public void clearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            email_txt.Clear();
            id_txt.Clear();
        }

        //метод для открытия таблицы на DataGrid - е
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[UsersForm]", con);
            DataTable dataTable = new DataTable();
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);
            con.Close(); //обязательно нужно закрыть после оканчания действии
            datagrid.ItemsSource = dataTable.DefaultView;
        }

        public bool isValid() //метод для проверки на пустоту текстбоксов
        {
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
            {
                MessageBox.Show("Age is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("Gender is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (email_txt.Text == string.Empty)
            {
                MessageBox.Show("Email is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (id_txt.Text == string.Empty)
            {
                MessageBox.Show("ID is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        //кнопка - добавить данные в таблицу
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            //исключение - чтобы программа не перестала работать даже если появится ошибка/исключение
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[UsersForm] VALUES (@Name,@Age, @Gender, @Email, @ID)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender_txt.Text);
                    cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                    cmd.Parameters.AddWithValue("@ID", id_txt.Text);

                    con.Open();
                    cmd.ExecuteNonQuery(); //выполнение команд
                    con.Close();
                    LoadGrid();
                    clearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //удаление данных по ID
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Delete from [dbo].[UsersForm] where ID = " + id_txt.Text + "", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record has been deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not Deleted" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        //кнопка для обновления данных
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new("update [dbo].[UsersForm] set [Name] = '" + name_txt.Text + "',[Age] = '" + age_txt.Text + "',[Gender] = '" + gender_txt.Text + "',[Email] = '" + email_txt.Text + "' WHERE [ID] = '" + id_txt.Text + "'", con);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 
        }
    }
}
