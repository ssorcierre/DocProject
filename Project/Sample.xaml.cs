using System;
using System.IO;
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
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;



namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Sample.xaml
    /// </summary>
    public partial class Sample : Page
    {
        
        #region Поля
        private string connStr;
        public SqlConnection Con { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
        public string gender { get; set; }
        public string startdate { get; set; }
        public string jobplace { get; set; }
        public string position { get; set; }
        public string salary { get; set; }
        #endregion

        public Sample()
        {
            InitializeComponent();
            connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\aarma\OneDrive\Рабочий стол\Проект_рпм\Проектик\Project\DBProject.mdf;Integrated Security=True";
            this.OpenFiletxtbox.IsReadOnly = true;
        }

        private void Backbt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        #region Обзор шаблонов

        #region Склонение
        public string LastnameSkl(string lastname)
        {
            Regex patternOV = new Regex(@"\w+(ов|ев|ин)$");
            Regex patternIY = new Regex(@"\w+(ий)$");
            Regex patternA = new Regex(@"\w+(а)$");
            Regex patternOVA = new Regex(@"\w+(ова)$");
            Regex patternINA = new Regex(@"\w+(ина)$");
            Regex patternSogl = new Regex(@"\w+(к|т|з|ч|ц|г|б|л)");
            Regex patternAZ = new Regex(@"\w+(ая)$");
            Regex patternMz = new Regex(@"\w+(ь)$");

            if (patternOV.IsMatch(lastname))
                return (lastname += 'у');
            else if (patternIY.IsMatch(lastname))
            {
                string target = "ому";
                return Regex.Replace(lastname, "ий$", target);
            }
            else if (patternOVA.IsMatch(lastname))
            {
                string target = "овой";
                return Regex.Replace(lastname, "ова", target);
            }
            else if (patternINA.IsMatch(lastname))
            {
                string target = "ой";
                return Regex.Replace(lastname, "а$", target);
            }
            else if (patternAZ.IsMatch(lastname))
            {
                string target = "ой";
                return Regex.Replace(lastname, "ая$", target);
            }
            else if (patternA.IsMatch(lastname))
            {
                string target = "е";
                return Regex.Replace(lastname, "а$", target);
            }
            else if (patternMz.IsMatch(lastname))
            {
                string target = "ю";
                return Regex.Replace(lastname, "ь$", target);
            }
            else if (patternSogl.IsMatch(lastname))
            {
                return (lastname += 'у');
            }
            else
            {
                return (lastname);
            }
        }

        public string NameSkl(string name)
        {
            Regex patternTv = new Regex(@"\w+(к|т|з|ч|ц|г|б|л|м)$");
            Regex patternY = new Regex(@"\w+(й)$");
            Regex patternA = new Regex(@"\w+(а)$");
            Regex patternYA = new Regex(@"\w+(я|ья)$");
            Regex patternIYA = new Regex(@"\w+(ия|ея)$");

            if (patternIYA.IsMatch(name))
            {
                string target = "и";
                return Regex.Replace(name, "я$", target);
            }
            if (patternYA.IsMatch(name))
            {
                string target = "е";
                return Regex.Replace(name, "я$", target);
            }
            if (patternA.IsMatch(name))
            {
                string target = "е";
                return Regex.Replace(name, "а$", target);
            }
            if (patternY.IsMatch(name))
            {
                string target = "ю";
                return Regex.Replace(name, "й$", target);
            }
            if (patternTv.IsMatch(name))
            {
                return (name += 'у');
            }
            else 
            { return name; }
        }

        public string FatherNameSKL(string name)
        {
            Regex patternCh = new Regex(@"\w+(ч)$");
            Regex patternVna = new Regex(@"\w+(вна)$");
            if (patternCh.IsMatch(name))
            {
                return (name += 'у');
            }
            if (patternVna.IsMatch(name))
            {
                string target = "е";
                return Regex.Replace(name, "а$", target);
            }
            else { return name; }
        }

        #endregion
        private void OpenDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            

            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT Files (*.txt)|*.txt";
            dlg.Multiselect = false;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    OpenFiletxtbox.Text = File.ReadAllText(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Шаблон спрвка с места работы
        [Obsolete]
        private void SpravkaPlaceJob()
        {
            string idPerson = Lastnametxtbox.Text;
            string nameOrg = OrgMainTB.Text;
            Con = new SqlConnection(connStr);
            Con.Open();
            string queryString = $"SELECT [Фамилия], [Имя], [Отчество], [Пол], [Дата_начала_работы], [Название_организации], [Должность], [Ежемесячная_заработная_плата]" +
                $"FROM People, Employee, Org " +
                $"WHERE Employee.[ID_организации]=Org.[ID_организации] AND Employee.[ID_человека]=People.[ID_человека] AND People.[ID_человека] = @idPerson AND [Название_организации] = @nameOrg";
            SqlCommand command = new SqlCommand(queryString, Con);
            command.Parameters.Add("@idPerson", idPerson);
            command.Parameters.Add("@nameOrg", nameOrg);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            
            List<string> list = new List<string>();
         
            foreach (DataRow row in table.Rows)
            {
                surname = row[0].ToString();
                name = row[1].ToString();
                fathername = row[2].ToString();
                gender = row[3].ToString();
                startdate = DateTime.Parse(Convert.ToString(row[4])).ToString("dd.MM.yyyy");
                jobplace = row[5].ToString();
                position = row[6].ToString();
                int salary1 = Convert.ToInt32(row[7]);
                salary = salary1.ToString();
            }
            Con.Close();
            
        }

        // Создание документа из готового шаблона.
        Word._Application oWord = new Word.Application();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            oWord.Quit();
        }

        // Заполнение шаблона
        [Obsolete]
        private void SetTemplate(Word._Document oDoc)
        {
            SpravkaPlaceJob();
            DateTime dateTime = DateTime.UtcNow.Date;
            Random r = new Random();
            int number = r.Next(100000, 999999);
            if (gender == "М")
            {
                string gender1 = "он ";
                oDoc.Bookmarks["gender"].Range.Text = gender1.ToString();
            }
            else if (gender == "Ж")
            {
                string gender1 = "она ";
                oDoc.Bookmarks["gender"].Range.Text = gender1.ToString();
            }
            oDoc.Bookmarks["orgname"].Range.Text = Orgtxtbox.Text;
            oDoc.Bookmarks["number"].Range.Text = number.ToString();
            oDoc.Bookmarks["date"].Range.Text = dateTime.ToString("dd/MM/yyyy");
            oDoc.Bookmarks["fio"].Range.Text = $@"{LastnameSkl(surname)} {NameSkl(name)} {FatherNameSKL(fathername)}";
            oDoc.Bookmarks["startdate"].Range.Text = startdate;
            oDoc.Bookmarks["jobplace"].Range.Text = jobplace;
            oDoc.Bookmarks["position"].Range.Text = position;
            oDoc.Bookmarks["salary"].Range.Text = salary;
            oDoc.Bookmarks["orgnamemain"].Range.Text = $"{jobplace}\n";
        }

        [Obsolete]
        private Word._Document Getting_Doc(string path)
        {
            Word._Document oDoc = oWord.Documents.Add(path);
            SetTemplate(oDoc);
            return oDoc;
        }

        private void CreateDoc_Click(object sender, RoutedEventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Regex patternId = new Regex("^[0-9]");
            bool checkID = patternId.IsMatch(Lastnametxtbox.Text);

            if (Lastnametxtbox.Text == String.Empty || OrgMainTB.Text == String.Empty || Orgtxtbox.Text == String.Empty)
            {
                MessageBox.Show("Ошибка! Все поля должны быть заполнены!", "Мастер документов", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (checkID == false)
            {
                MessageBox.Show("ID человека введено в неверном формате!", "Мастер документов", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                try
                {
                    string title = TitleTB.Text;
                    Word._Document oDoc = Getting_Doc(Environment.CurrentDirectory + "\\SpravkaPlaceJob.docx");
                    oDoc.SaveAs(FileName: desktopPath + $"\\{title}.docx");
                    oDoc.Close();
                    MessageBox.Show("Ваш документ создан! Вы можете найти его на рабочем столе.", "Мастер документов", MessageBoxButton.OK);
                    TitleTB.Text = "";
                    Lastnametxtbox.Text = "";
                    OrgMainTB.Text = "";
                    Orgtxtbox.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Мастер документов");
                }
            }           
        }
        #endregion

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
