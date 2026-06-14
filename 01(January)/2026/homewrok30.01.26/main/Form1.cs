using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        TabControl tabs;

        TextBox txtNumbersFile;
        ListBox listNumbersResult;

        TextBox txtResumePath;
        TextBox txtCity;
        ListBox listResumeResult;

        List<Resume> resumes = new List<Resume>();

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        void CreateInterface()
        {
            Text = "Параллельное программирование. Часть 2";
            Width = 900;
            Height = 650;

            tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            Controls.Add(tabs);

            CreateNumbersTab();
            CreateResumeTab();
        }

        // ---------- ЗАДАНИЯ 1, 2 ----------

        void CreateNumbersTab()
        {
            TabPage page = new TabPage("Числа PLINQ");

            Label lbl = new Label();
            lbl.Text = "Путь к файлу с числами:";
            lbl.Left = 20;
            lbl.Top = 20;
            lbl.Width = 160;
            page.Controls.Add(lbl);

            txtNumbersFile = new TextBox();
            txtNumbersFile.Left = 190;
            txtNumbersFile.Top = 20;
            txtNumbersFile.Width = 550;
            page.Controls.Add(txtNumbersFile);

            Button btnUnique = new Button();
            btnUnique.Text = "Уникальные";
            btnUnique.Left = 20;
            btnUnique.Top = 60;
            btnUnique.Click += CountUnique_Click;
            page.Controls.Add(btnUnique);

            Button btnIncreasing = new Button();
            btnIncreasing.Text = "Возрастающая";
            btnIncreasing.Left = 140;
            btnIncreasing.Top = 60;
            btnIncreasing.Click += Increasing_Click;
            page.Controls.Add(btnIncreasing);

            Button btnPositive = new Button();
            btnPositive.Text = "Положительная";
            btnPositive.Left = 280;
            btnPositive.Top = 60;
            btnPositive.Click += Positive_Click;
            page.Controls.Add(btnPositive);

            listNumbersResult = new ListBox();
            listNumbersResult.Left = 20;
            listNumbersResult.Top = 110;
            listNumbersResult.Width = 820;
            listNumbersResult.Height = 430;
            page.Controls.Add(listNumbersResult);

            tabs.TabPages.Add(page);
        }

        List<int> ReadNumbers()
        {
            string text = File.ReadAllText(txtNumbersFile.Text);

            string[] parts = text.Split(
                new char[] { ' ', '\n', '\r', '\t', ',', ';' },
                StringSplitOptions.RemoveEmptyEntries
            );

            List<int> numbers = parts.Select(x => Convert.ToInt32(x)).ToList();

            return numbers;
        }

        void CountUnique_Click(object sender, EventArgs e)
        {
            listNumbersResult.Items.Clear();

            List<int> numbers = ReadNumbers();

            int count = numbers.AsParallel().Distinct().Count();

            listNumbersResult.Items.Add("Количество уникальных значений: " + count);
        }

        void Increasing_Click(object sender, EventArgs e)
        {
            listNumbersResult.Items.Clear();

            List<int> numbers = ReadNumbers();

            var result = numbers
                .Select((value, index) => new { value, index })
                .AsParallel()
                .Select(x => GetIncreasingSequence(numbers, x.index))
                .OrderByDescending(x => x.Count)
                .First();

            listNumbersResult.Items.Add("Максимальная длина возрастающей последовательности: " + result.Count);
            listNumbersResult.Items.Add("Последовательность:");

            foreach (int number in result)
                listNumbersResult.Items.Add(number);
        }

        List<int> GetIncreasingSequence(List<int> numbers, int start)
        {
            List<int> result = new List<int>();
            result.Add(numbers[start]);

            for (int i = start + 1; i < numbers.Count; i++)
            {
                if (numbers[i] > result[result.Count - 1])
                    result.Add(numbers[i]);
                else
                    break;
            }

            return result;
        }

        void Positive_Click(object sender, EventArgs e)
        {
            listNumbersResult.Items.Clear();

            List<int> numbers = ReadNumbers();

            var result = numbers
                .Select((value, index) => new { value, index })
                .AsParallel()
                .Select(x => GetPositiveSequence(numbers, x.index))
                .OrderByDescending(x => x.Count)
                .First();

            listNumbersResult.Items.Add("Максимальная длина положительной последовательности: " + result.Count);
            listNumbersResult.Items.Add("Последовательность:");

            foreach (int number in result)
                listNumbersResult.Items.Add(number);
        }

        List<int> GetPositiveSequence(List<int> numbers, int start)
        {
            List<int> result = new List<int>();

            for (int i = start; i < numbers.Count; i++)
            {
                if (numbers[i] > 0)
                    result.Add(numbers[i]);
                else
                    break;
            }

            return result;
        }

        // ---------- ЗАДАНИЕ 3 ----------

        void CreateResumeTab()
        {
            TabPage page = new TabPage("Резюме");

            Label lbl = new Label();
            lbl.Text = "Путь к файлу или папке:";
            lbl.Left = 20;
            lbl.Top = 20;
            lbl.Width = 160;
            page.Controls.Add(lbl);

            txtResumePath = new TextBox();
            txtResumePath.Left = 190;
            txtResumePath.Top = 20;
            txtResumePath.Width = 550;
            page.Controls.Add(txtResumePath);

            Button btnLoadFile = new Button();
            btnLoadFile.Text = "Загрузить файл";
            btnLoadFile.Left = 20;
            btnLoadFile.Top = 60;
            btnLoadFile.Click += LoadOneResume_Click;
            page.Controls.Add(btnLoadFile);

            Button btnLoadFolder = new Button();
            btnLoadFolder.Text = "Загрузить папку";
            btnLoadFolder.Left = 150;
            btnLoadFolder.Top = 60;
            btnLoadFolder.Click += LoadFolderResume_Click;
            page.Controls.Add(btnLoadFolder);

            Button btnReports = new Button();
            btnReports.Text = "Отчёты";
            btnReports.Left = 290;
            btnReports.Top = 60;
            btnReports.Click += Reports_Click;
            page.Controls.Add(btnReports);

            Label lblCity = new Label();
            lblCity.Text = "Город:";
            lblCity.Left = 400;
            lblCity.Top = 65;
            page.Controls.Add(lblCity);

            txtCity = new TextBox();
            txtCity.Left = 460;
            txtCity.Top = 62;
            txtCity.Width = 150;
            page.Controls.Add(txtCity);

            Button btnCity = new Button();
            btnCity.Text = "Кандидаты из города";
            btnCity.Left = 630;
            btnCity.Top = 60;
            btnCity.Width = 160;
            btnCity.Click += City_Click;
            page.Controls.Add(btnCity);

            listResumeResult = new ListBox();
            listResumeResult.Left = 20;
            listResumeResult.Top = 110;
            listResumeResult.Width = 820;
            listResumeResult.Height = 430;
            page.Controls.Add(listResumeResult);

            tabs.TabPages.Add(page);
        }

        void LoadOneResume_Click(object sender, EventArgs e)
        {
            listResumeResult.Items.Clear();
            resumes.Clear();

            Resume resume = ReadResume(txtResumePath.Text);
            resumes.Add(resume);

            listResumeResult.Items.Add("Загружено резюме: " + resume.Name);
        }

        void LoadFolderResume_Click(object sender, EventArgs e)
        {
            listResumeResult.Items.Clear();
            resumes.Clear();

            string[] files = Directory.GetFiles(txtResumePath.Text, "*.txt", SearchOption.AllDirectories);

            object locker = new object();

            Parallel.ForEach(files, file =>
            {
                Resume resume = ReadResume(file);

                lock (locker)
                {
                    resumes.Add(resume);
                }
            });

            listResumeResult.Items.Add("Загружено резюме: " + resumes.Count);
        }

        Resume ReadResume(string path)
        {
            string[] lines = File.ReadAllLines(path);

            Resume resume = new Resume();

            foreach (string line in lines)
            {
                if (line.StartsWith("ФИО:"))
                    resume.Name = line.Replace("ФИО:", "").Trim();

                if (line.StartsWith("Город:"))
                    resume.City = line.Replace("Город:", "").Trim();

                if (line.StartsWith("Опыт:"))
                    resume.Experience = Convert.ToInt32(line.Replace("Опыт:", "").Trim());

                if (line.StartsWith("Зарплата:"))
                    resume.Salary = Convert.ToInt32(line.Replace("Зарплата:", "").Trim());
            }

            return resume;
        }

        void Reports_Click(object sender, EventArgs e)
        {
            listResumeResult.Items.Clear();

            if (resumes.Count == 0)
            {
                listResumeResult.Items.Add("Сначала загрузите резюме.");
                return;
            }

            Resume maxExp = resumes.AsParallel()
                .OrderByDescending(r => r.Experience)
                .First();

            Resume minExp = resumes.AsParallel()
                .OrderBy(r => r.Experience)
                .First();

            Resume minSalary = resumes.AsParallel()
                .OrderBy(r => r.Salary)
                .First();

            Resume maxSalary = resumes.AsParallel()
                .OrderByDescending(r => r.Salary)
                .First();

            listResumeResult.Items.Add("Самый опытный кандидат:");
            AddResumeToList(maxExp);

            listResumeResult.Items.Add("");

            listResumeResult.Items.Add("Самый неопытный кандидат:");
            AddResumeToList(minExp);

            listResumeResult.Items.Add("");

            listResumeResult.Items.Add("Самая низкая зарплата:");
            AddResumeToList(minSalary);

            listResumeResult.Items.Add("");

            listResumeResult.Items.Add("Самая высокая зарплата:");
            AddResumeToList(maxSalary);
        }

        void City_Click(object sender, EventArgs e)
        {
            listResumeResult.Items.Clear();

            string city = txtCity.Text;

            var result = resumes.AsParallel()
                .Where(r => r.City.ToLower() == city.ToLower())
                .ToList();

            listResumeResult.Items.Add("Кандидаты из города: " + city);

            foreach (Resume resume in result)
            {
                AddResumeToList(resume);
                listResumeResult.Items.Add("");
            }
        }

        void AddResumeToList(Resume r)
        {
            listResumeResult.Items.Add("ФИО: " + r.Name);
            listResumeResult.Items.Add("Город: " + r.City);
            listResumeResult.Items.Add("Опыт: " + r.Experience);
            listResumeResult.Items.Add("Зарплата: " + r.Salary);
        }
    }

    public class Resume
    {
        public string Name;
        public string City;
        public int Experience;
        public int Salary;
    }
}