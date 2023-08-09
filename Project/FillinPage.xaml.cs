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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;


namespace Project
{
    /// <summary>
    /// Логика взаимодействия для FillinPage.xaml
    /// </summary>
    public partial class FillinPage : Page
    {
        private string connStr;

        public SqlConnection Con {get; set;}

        public FillinPage()
        {
            InitializeComponent();

            connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aarma\OneDrive\Рабочий стол\Проект_рпм\Проектик\Project\DBProject.mdf;Integrated Security=True";

        }

        
        private void FillPeople_Click(object sender, RoutedEventArgs e)
        {
            Con = new SqlConnection(connStr);
            var Lastname = LastnameTB.Text;
            var Name = NameTB.Text;
            var Fathername = FathernameTB.Text;
            var Sex = SexTB.Text;
            var LivPlace = LivPlaceTB.Text;
            var Adress = AdressTB.Text;
            var Telephone = TelepnoneTB.Text;
            var Email = EmailTB.Text;
            var Education = EducationTB.Text;

            Regex pattern1 = new Regex(@"[a-zA-Z0-9\.\^\$\*\+\?\{\}\[\]\=\|\(\)]+$");
            Regex pattern2 = new Regex(@"[М|Ж]");
            Regex pattern3 = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            Regex pattern4 = new Regex("[0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{4}");
            Regex pattern5 = new Regex("^(\\+7|8)[0-9]{10}$");

            bool checkSex = pattern2.IsMatch(SexTB.Text);
            bool checkMail = pattern3.IsMatch(EmailTB.Text);
            bool checkDate = pattern4.IsMatch(BirthdateTB.Text);
            bool checkNumber = pattern5.IsMatch(TelepnoneTB.Text);

            // Есть ли пустые поля
            if ((LastnameTB.Text == string.Empty)|| (NameTB.Text == string.Empty)|| (FathernameTB.Text == string.Empty)|| (SexTB.Text == string.Empty)|| (BirthdateTB.Text == string.Empty) || (LivPlaceTB.Text == string.Empty)
                || (AdressTB.Text == string.Empty)||(TelepnoneTB.Text == string.Empty)||(EmailTB.Text == string.Empty)||(EducationTB.Text == string.Empty))
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Есть ли неккоректные значения в имени, фамилии, отчестве и городе
            else if (pattern1.IsMatch(LastnameTB.Text) || pattern1.IsMatch(NameTB.Text) || pattern1.IsMatch(FathernameTB.Text) || pattern1.IsMatch(LivPlaceTB.Text))
            {
                MessageBox.Show("Введено неккоректное значение! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Проверка значения в поле пол
            else if (checkSex == false)
            {
                MessageBox.Show("Поле ''Пол'' принмает только значения в формате ''М'' и ''Ж''!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Проверка e-mail
            else if (checkMail == false)
            {
                MessageBox.Show("Введен неккоректный e-mail!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Проверка ввода формата даты
            else if (checkDate == false)
            {
                MessageBox.Show("Дата рождения введена в неверном формате!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (checkNumber == false)
            {
                MessageBox.Show("Номер телефона введен в неверном формате!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Проверка корректности введенной даты
               if (DateTime.TryParse(BirthdateTB.Text, out DateTime Birthdate))
               {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        Con.Open();

                        string queryString = $"INSERT [People] VALUES (N'{Lastname}',N'{Name}',N'{Fathername}',N'{Sex}', @Birthdate, N'{LivPlace}',N'{Adress}','{Telephone}','{Email}', N'{Education}')";
                        SqlCommand command = new SqlCommand(queryString, Con);

                        command.Parameters.AddWithValue("@Birthdate", SqlDbType.Date);
                        command.Parameters["@Birthdate"].Value = Birthdate;
                        adapter.SelectCommand = command;

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Запись успешно сохранена!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Запись не сохранена! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    finally
                    {
                       Con.Close();
                    }
               }
               else
               {
                    MessageBox.Show("Введена неккоректная дата рождения! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
               }
                
            }
        }

        private void FillEmployee_Click(object sender, RoutedEventArgs e)
        {
            Con = new SqlConnection(connStr);
            var IDPeople = IDPeopleTB.Text;
            var IDOrg = IDOrgTB.Text;
            Int32.TryParse(IDOrg, out Int32 IdOrg);
            var Position = PositionTB.Text;
            DateTime.TryParse(JobDateTB.Text, out DateTime checkJobDate);

            SqlDataAdapter adp = new SqlDataAdapter();
            Con.Open();
            string query = $"SELECT [Дата_создания] FROM [Org] WHERE [ID_организации] = @IdOrg";
            SqlCommand cmd = new SqlCommand(query, Con);
            
            cmd.Parameters.AddWithValue("@IdOrg", IdOrg);
            adp.SelectCommand = cmd;
            DateTime checkDate = (DateTime)cmd.ExecuteScalar();
            

            Regex patternID = new Regex("^[0-9]");
            Regex patternPos = new Regex(@"[a-zA-Z0-9\.\^\$\*\+\?\{\}\[\]\=\|\(\)]+$"); 
            Regex patternSalary = new Regex("^[0-9]");
            Regex patternStartDate = new Regex("[0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{4}");

            bool checkIDpeople = patternID.IsMatch(IDPeopleTB.Text);
            bool checkIDorg = patternID.IsMatch(IDPeopleTB.Text);
            bool checkPosition = patternPos.IsMatch(PositionTB.Text);
            bool checkSalary = patternSalary.IsMatch(SalaryTB.Text);
            bool checkStartDate = patternStartDate.IsMatch(JobDateTB.Text);
            // Есть ли пустые поля
            if ((IDPeopleTB.Text == string.Empty) || (IDOrgTB.Text == string.Empty) || (PositionTB.Text == string.Empty) || (JobDateTB.Text == string.Empty))
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((checkIDpeople == false) || (checkIDorg == false))
            {
                MessageBox.Show("ID введен в неправильном формате! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Есть ли неккоректные значения в должности
            else if (checkPosition == true)
            {
                MessageBox.Show("Введено неккоректное значение! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // Есть ли неккоректные значения в зарплате
            else if (checkSalary == false)
            {
                MessageBox.Show("Заработная плата введена неккоректно! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (checkStartDate == false)
            {
                MessageBox.Show("Дата начала работы введена в неверном формате! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (checkDate >= checkJobDate == true)
            {
                MessageBox.Show("Сотрудник начал работать в компании раньше, чем она была создана!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (DateTime.TryParse(JobDateTB.Text, out DateTime Jobdate))
                {
                    try
                    {
                        int Salary = int.Parse(SalaryTB.Text);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        Con.Open();

                        string queryString = $"INSERT INTO [Employee] ([ID_человека], [ID_организации], [Должность], [Ежемесячная_заработная_плата], [Дата_начала_работы]) " +
                            $"VALUES ('{IDPeople}', '{IDOrg}',N'{Position}','{Salary}', @Jobdate)";
                        SqlCommand command = new SqlCommand(queryString, Con);

                        command.Parameters.Add("@Jobdate", SqlDbType.Date);
                        command.Parameters["@Jobdate"].Value = Jobdate;
                        adapter.SelectCommand = command;
                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBoxResult result = MessageBox.Show("Запись успешно сохранена!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Запись не сохранена! Проверьте введенные ID!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    finally
                    {
                        Con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Введена неккоректная дата начала работы! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void FillOrg_Click(object sender, RoutedEventArgs e)
        {
            Con = new SqlConnection(connStr);
            var OrgName = OrgNameTB.Text;
            var Sphere = SphereTB.Text;

            Regex patternOrgdate = new Regex("[0-9]{1,2}\\.[0-9]{1,2}\\.[0-9]{4}");

            bool checkStartDate = patternOrgdate.IsMatch(CreateDateTB.Text);

            if ((OrgNameTB.Text == string.Empty) || (SphereTB.Text == string.Empty) || (CreateDateTB.Text == string.Empty))
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (checkStartDate == false)
            {
                MessageBox.Show("Дата создания введена в неверном формате! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (DateTime.TryParse(CreateDateTB.Text, out DateTime Orgdate))
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        Con.Open();

                        string queryString = $"INSERT [Org] VALUES (N'{OrgName}', N'{Sphere}', @Orgdate)";
                        SqlCommand command = new SqlCommand(queryString, Con);

                        command.Parameters.Add("@Orgdate", SqlDbType.Date);
                        command.Parameters["@Orgdate"].Value = Orgdate;
                        adapter.SelectCommand = command;

                        if (command.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Запись успешно сохранена!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Запись не сохранена! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    finally
                    {
                        Con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Введена неккоректная дата создания! Ошибка!", "Менеджер БД", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Backbt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BDpage());
        }
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BDSpravkaPage());
        }
        private void Searchbt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage());
        }
    }
}
