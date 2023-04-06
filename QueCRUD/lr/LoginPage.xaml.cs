using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
//для работы с БД
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace QueCRUD.lr
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) // for move window
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public static DataTable Select(string selectSQL) // функция для подключение к БД и обработка запросов
        {
            DataTable dataTable = new DataTable("UsersQ"); // создаем таблицу в приложении
            SqlConnection SqlConnection = new SqlConnection("Data Source=ARS;Initial Catalog=QueCRUD;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            SqlConnection.Open(); // открываем подключение
            SqlCommand sqlCommand = SqlConnection.CreateCommand(); // создать команду
            sqlCommand.CommandText = selectSQL; // присваеваем команде текст
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создает адаптер
            sqlDataAdapter.Fill(dataTable); // возвращаем таблицу с результатом
            return dataTable;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUser.Text.Length > 3 || txtPass.Password.Length > 3) // проверяем введён ли логин и пароль     
            {
                DataTable cmd = LoginPage.Select("SELECT * FROM [dbo].[Users] WHERE [user] = '" + txtUser.Text + "' AND [pass] = '" + txtPass.Password + "'");
                if (cmd.Rows.Count > 0) // если такая запись существует       
                {
                    // говорим, что авторизовался  // ищем в базе данных пользователя с такими данным
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    txtUser.ToolTip = "This field is entered incorrectly"; // hover hint
                    txtUser.Foreground = Brushes.Red; // change styles
                    txtUser.CaretBrush = Brushes.Red;

                    txtPass.ToolTip = "This field is entered incorrectly"; // hover hint
                    txtPass.Foreground = Brushes.Red; // change styles
                    txtPass.CaretBrush = Brushes.Red;
                }
            }
            
        }
    }
}
