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
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        private string connStr;
        public SqlConnection Con { get; set; }

        public SearchPage()
        {
            InitializeComponent();
            connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aarma\OneDrive\Рабочий стол\Проект_рпм\Проектик\Project\DBProject.mdf;Integrated Security=True";
        }

        private void DonePerson_Click(object sender, RoutedEventArgs e)
        {
            string lastname = PersonTB.Text;
            Con = new SqlConnection(connStr);
            Con.Open();
            string queryString = $"SELECT People.[ID_человека], [Фамилия], [Имя], [Отчество], [Дата_рождения], Org.[Название_организации]" +
                $"FROM [People], [Org], [Employee]" +
                $"WHERE [Фамилия] = @surname AND Employee.[ID_человека] = People.[ID_человека] AND Employee.[ID_организации] = Org.[ID_организации]";
            SqlCommand command = new SqlCommand(queryString, Con);
            command.Parameters.AddWithValue("@surname", lastname);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count >= 1)
            {
                SearchPeople.ItemsSource = table.AsDataView();
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте введенные данные!", "Мастер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Con.Close();
            PersonTB.Text = "";
        }

        private void DoneOrg_Click(object sender, RoutedEventArgs e)
        {
            string orgname = OrgTB.Text;
            Con = new SqlConnection(connStr);
            Con.Open();
            string queryString = $"SELECT [ID_организации], [Название_организации], [Род_деятельности]" +
                $"FROM [Org]" +
                $"WHERE [Название_организации] = @orgname";
            SqlCommand command = new SqlCommand(queryString, Con);
            command.Parameters.AddWithValue("@orgname", orgname);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            if (table.Rows.Count >= 1)
            {
                SearchOrg.ItemsSource = table.AsDataView();
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте введенные данные!", "Мастер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            Con.Close();
            OrgTB.Text = "";
        }

        private void Backbt_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Некуда возвращаться");
            }
        }
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BDSpravkaPage());
        }
    }
}
