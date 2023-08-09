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
    /// Логика взаимодействия для BDpage.xaml
    /// </summary>
    public partial class BDpage : Page
    {
        private string connStr;
        public SqlConnection Con { get; set; }

        public BDpage()
        {
            InitializeComponent();
            connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aarma\OneDrive\Рабочий стол\Проект_рпм\Проектик\Project\DBProject.mdf;Integrated Security=True";
        }
        static DataTable DBProject(string sql)
        {
            DataTable table = new DataTable();
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aarma\OneDrive\Рабочий стол\Проект_рпм\Проектик\Project\DBProject.mdf;Integrated Security=True");

            using (conn)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                { 
                    MessageBox.Show(ex.Message);
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader read = cmd.ExecuteReader();

                using (read)
                {
                    table.Load(read);
                }
            }
            return table;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            DataTable people = DBProject("SELECT * FROM People");
            TablePeople.ItemsSource = people.DefaultView;
            DataTable employee = DBProject("SELECT * FROM Employee");
            TableEmployee.ItemsSource = employee.DefaultView;
            DataTable org = DBProject("SELECT * FROM Org");
            TableOrg.ItemsSource = org.DefaultView;
        }

        private void FillIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FillinPage());
        }
        private void Backbt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BDSpravkaPage());
        }
    }
}
