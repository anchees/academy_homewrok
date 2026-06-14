using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        TabControl tabs;

        // Задание 1
        TextBox txtBarsCount;
        FlowLayoutPanel panelBars;

        // Задание 2
        ProgressBar[] horseBars = new ProgressBar[5];
        ListBox lstRaceResult;
        List<string> raceResults = new List<string>();
        int finishPlace = 0;

        // Задание 3
        TextBox txtFibLimit;
        ListBox lstFib;

        // Задание 4
        TextBox txtFilePath;
        TextBox txtSearchWordFile;
        Label lblFileResult;

        // Задание 5
        TextBox txtDirPath;
        TextBox txtSearchWordDir;
        ListBox lstDirResult;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        void CreateInterface()
        {
            Text = "Многопоточность и асинхронность. Часть 2";
            Width = 900;
            Height = 650;

            tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            Controls.Add(tabs);

            CreateTask1();
            CreateTask2();
            CreateTask3();
            CreateTask4();
            CreateTask5();
        }

        // ---------------- ЗАДАНИЕ 1 ----------------

        void CreateTask1()
        {
            TabPage page = new TabPage("Задание 1");

            Label label = new Label();
            label.Text = "Количество прогресс-баров:";
            label.Left = 20;
            label.Top = 20;
            label.Width = 180;
            page.Controls.Add(label);

            txtBarsCount = new TextBox();
            txtBarsCount.Left = 210;
            txtBarsCount.Top = 20;
            txtBarsCount.Width = 100;
            page.Controls.Add(txtBarsCount);

            Button btnCreate = new Button();
            btnCreate.Text = "Создать";
            btnCreate.Left = 330;
            btnCreate.Top = 18;
            btnCreate.Click += CreateBars_Click;
            page.Controls.Add(btnCreate);

            Button btnStart = new Button();
            btnStart.Text = "Старт";
            btnStart.Left = 430;
            btnStart.Top = 18;
            btnStart.Click += StartBars_Click;
            page.Controls.Add(btnStart);

            panelBars = new FlowLayoutPanel();
            panelBars.Left = 20;
            panelBars.Top = 70;
            panelBars.Width = 800;
            panelBars.Height = 450;
            panelBars.AutoScroll = true;
            page.Controls.Add(panelBars);

            tabs.TabPages.Add(page);
        }

        void CreateBars_Click(object sender, EventArgs e)
        {
            panelBars.Controls.Clear();

            int count = Convert.ToInt32(txtBarsCount.Text);

            for (int i = 0; i < count; i++)
            {
                ProgressBar bar = new ProgressBar();
                bar.Width = 700;
                bar.Height = 30;
                bar.Maximum = 100;
                bar.Value = 0;

                panelBars.Controls.Add(bar);
            }
        }

        void StartBars_Click(object sender, EventArgs e)
        {
            foreach (Control control in panelBars.Controls)
            {
                ProgressBar bar = control as ProgressBar;

                Thread thread = new Thread(() => FillProgressBar(bar));
                thread.Start();
            }
        }

        void FillProgressBar(ProgressBar bar)
        {
            while (bar.Value < 100)
            {
                int step = random.Next(1, 10);

                Invoke(new Action(() =>
                {
                    if (bar.Value + step <= 100)
                        bar.Value += step;
                    else
                        bar.Value = 100;
                }));

                Thread.Sleep(random.Next(100, 500));
            }
        }

        // ---------------- ЗАДАНИЕ 2 ----------------

        void CreateTask2()
        {
            TabPage page = new TabPage("Задание 2");

            Button btnStart = new Button();
            btnStart.Text = "Старт гонки";
            btnStart.Left = 20;
            btnStart.Top = 20;
            btnStart.Click += StartRace_Click;
            page.Controls.Add(btnStart);

            for (int i = 0; i < 5; i++)
            {
                Label label = new Label();
                label.Text = "Лошадь " + (i + 1);
                label.Left = 20;
                label.Top = 70 + i * 50;
                label.Width = 80;
                page.Controls.Add(label);

                horseBars[i] = new ProgressBar();
                horseBars[i].Left = 110;
                horseBars[i].Top = 70 + i * 50;
                horseBars[i].Width = 600;
                horseBars[i].Height = 30;
                horseBars[i].Maximum = 100;
                page.Controls.Add(horseBars[i]);
            }

            lstRaceResult = new ListBox();
            lstRaceResult.Left = 20;
            lstRaceResult.Top = 350;
            lstRaceResult.Width = 750;
            lstRaceResult.Height = 180;
            page.Controls.Add(lstRaceResult);

            tabs.TabPages.Add(page);
        }

        void StartRace_Click(object sender, EventArgs e)
        {
            raceResults.Clear();
            lstRaceResult.Items.Clear();
            finishPlace = 0;

            for (int i = 0; i < 5; i++)
            {
                horseBars[i].Value = 0;
                int horseNumber = i + 1;

                Thread thread = new Thread(() => RunHorse(horseNumber, horseBars[horseNumber - 1]));
                thread.Start();
            }
        }

        void RunHorse(int horseNumber, ProgressBar bar)
        {
            while (bar.Value < 100)
            {
                int step = random.Next(1, 8);

                Invoke(new Action(() =>
                {
                    if (bar.Value + step <= 100)
                        bar.Value += step;
                    else
                        bar.Value = 100;
                }));

                Thread.Sleep(random.Next(100, 400));
            }

            Invoke(new Action(() =>
            {
                finishPlace++;
                string result = finishPlace + " место — Лошадь " + horseNumber;
                raceResults.Add(result);
                lstRaceResult.Items.Add(result);
            }));
        }

        // ---------------- ЗАДАНИЕ 3 ----------------

        void CreateTask3()
        {
            TabPage page = new TabPage("Задание 3");

            Label label = new Label();
            label.Text = "Граница:";
            label.Left = 20;
            label.Top = 20;
            page.Controls.Add(label);

            txtFibLimit = new TextBox();
            txtFibLimit.Left = 100;
            txtFibLimit.Top = 20;
            txtFibLimit.Width = 120;
            page.Controls.Add(txtFibLimit);

            Button btnStart = new Button();
            btnStart.Text = "Посчитать";
            btnStart.Left = 240;
            btnStart.Top = 18;
            btnStart.Click += CountFibonacci_Click;
            page.Controls.Add(btnStart);

            lstFib = new ListBox();
            lstFib.Left = 20;
            lstFib.Top = 70;
            lstFib.Width = 750;
            lstFib.Height = 450;
            page.Controls.Add(lstFib);

            tabs.TabPages.Add(page);
        }

        async void CountFibonacci_Click(object sender, EventArgs e)
        {
            lstFib.Items.Clear();

            int limit = Convert.ToInt32(txtFibLimit.Text);

            List<long> numbers = await Task.Run(() =>
            {
                List<long> result = new List<long>();

                long a = 0;
                long b = 1;

                while (a <= limit)
                {
                    result.Add(a);

                    long c = a + b;
                    a = b;
                    b = c;
                }

                return result;
            });

            foreach (long number in numbers)
            {
                lstFib.Items.Add(number);
            }
        }

        // ---------------- ЗАДАНИЕ 4 ----------------

        void CreateTask4()
        {
            TabPage page = new TabPage("Задание 4");

            Label lbl1 = new Label();
            lbl1.Text = "Путь к файлу:";
            lbl1.Left = 20;
            lbl1.Top = 20;
            page.Controls.Add(lbl1);

            txtFilePath = new TextBox();
            txtFilePath.Left = 130;
            txtFilePath.Top = 20;
            txtFilePath.Width = 550;
            page.Controls.Add(txtFilePath);

            Label lbl2 = new Label();
            lbl2.Text = "Слово:";
            lbl2.Left = 20;
            lbl2.Top = 60;
            page.Controls.Add(lbl2);

            txtSearchWordFile = new TextBox();
            txtSearchWordFile.Left = 130;
            txtSearchWordFile.Top = 60;
            txtSearchWordFile.Width = 200;
            page.Controls.Add(txtSearchWordFile);

            Button btnSearch = new Button();
            btnSearch.Text = "Искать";
            btnSearch.Left = 350;
            btnSearch.Top = 58;
            btnSearch.Click += SearchInFile_Click;
            page.Controls.Add(btnSearch);

            lblFileResult = new Label();
            lblFileResult.Left = 20;
            lblFileResult.Top = 120;
            lblFileResult.Width = 700;
            lblFileResult.Height = 50;
            lblFileResult.Font = new Font("Arial", 12);
            page.Controls.Add(lblFileResult);

            tabs.TabPages.Add(page);
        }

        async void SearchInFile_Click(object sender, EventArgs e)
        {
            string path = txtFilePath.Text;
            string word = txtSearchWordFile.Text;

            int count = await Task.Run(() =>
            {
                string text = File.ReadAllText(path);
                return CountWord(text, word);
            });

            lblFileResult.Text = "Количество вхождений слова: " + count;
        }

        // ---------------- ЗАДАНИЕ 5 ----------------

        void CreateTask5()
        {
            TabPage page = new TabPage("Задание 5");

            Label lbl1 = new Label();
            lbl1.Text = "Путь к папке:";
            lbl1.Left = 20;
            lbl1.Top = 20;
            page.Controls.Add(lbl1);

            txtDirPath = new TextBox();
            txtDirPath.Left = 130;
            txtDirPath.Top = 20;
            txtDirPath.Width = 550;
            page.Controls.Add(txtDirPath);

            Label lbl2 = new Label();
            lbl2.Text = "Слово:";
            lbl2.Left = 20;
            lbl2.Top = 60;
            page.Controls.Add(lbl2);

            txtSearchWordDir = new TextBox();
            txtSearchWordDir.Left = 130;
            txtSearchWordDir.Top = 60;
            txtSearchWordDir.Width = 200;
            page.Controls.Add(txtSearchWordDir);

            Button btnSearch = new Button();
            btnSearch.Text = "Искать";
            btnSearch.Left = 350;
            btnSearch.Top = 58;
            btnSearch.Click += SearchInDirectory_Click;
            page.Controls.Add(btnSearch);

            lstDirResult = new ListBox();
            lstDirResult.Left = 20;
            lstDirResult.Top = 110;
            lstDirResult.Width = 800;
            lstDirResult.Height = 420;
            page.Controls.Add(lstDirResult);

            tabs.TabPages.Add(page);
        }

        async void SearchInDirectory_Click(object sender, EventArgs e)
        {
            lstDirResult.Items.Clear();

            string directory = txtDirPath.Text;
            string word = txtSearchWordDir.Text;

            List<string> report = await Task.Run(() =>
            {
                List<string> result = new List<string>();

                string[] files = Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    string text = File.ReadAllText(file);
                    int count = CountWord(text, word);

                    result.Add("Название файла: " + Path.GetFileName(file));
                    result.Add("Путь к файлу: " + file);
                    result.Add("Количество вхождений слова: " + count);
                    result.Add("--------------------------------------");
                }

                return result;
            });

            foreach (string line in report)
            {
                lstDirResult.Items.Add(line);
            }
        }


        int CountWord(string text, string word)
        {
            int count = 0;
            string[] words = text.Split(
                new char[] { ' ', '.', ',', '!', '?', ';', ':', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (string item in words)
            {
                if (item.ToLower() == word.ToLower())
                    count++;
            }

            return count;
        }
    }
}