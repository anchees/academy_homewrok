using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        TabControl tabs;

        TextBox txtText;
        TextBox txtReport;
        CheckBox cbSentences, cbChars, cbWords, cbQuestions, cbExclamations;
        RadioButton rbScreen, rbFile;
        CancellationTokenSource textCancel;

        TextBox txtSource;
        TextBox txtReceiver;
        ListBox listFilesReport;
        CancellationTokenSource filesCancel;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        void CreateInterface()
        {
            Text = "Параллельное программирование";
            Width = 900;
            Height = 650;

            tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            Controls.Add(tabs);

            CreateTextTab();
            CreateFilesTab();
        }

        // -------- ЗАДАНИЯ 1, 2, 3 --------

        void CreateTextTab()
        {
            TabPage page = new TabPage("Задания 1-3");

            Label lbl = new Label();
            lbl.Text = "Введите текст:";
            lbl.Left = 20;
            lbl.Top = 20;
            page.Controls.Add(lbl);

            txtText = new TextBox();
            txtText.Left = 20;
            txtText.Top = 50;
            txtText.Width = 820;
            txtText.Height = 180;
            txtText.Multiline = true;
            txtText.ScrollBars = ScrollBars.Vertical;
            page.Controls.Add(txtText);

            cbSentences = new CheckBox();
            cbSentences.Text = "Количество предложений";
            cbSentences.Left = 20;
            cbSentences.Top = 250;
            cbSentences.Checked = true;
            page.Controls.Add(cbSentences);

            cbChars = new CheckBox();
            cbChars.Text = "Количество символов";
            cbChars.Left = 250;
            cbChars.Top = 250;
            cbChars.Checked = true;
            page.Controls.Add(cbChars);

            cbWords = new CheckBox();
            cbWords.Text = "Количество слов";
            cbWords.Left = 480;
            cbWords.Top = 250;
            cbWords.Checked = true;
            page.Controls.Add(cbWords);

            cbQuestions = new CheckBox();
            cbQuestions.Text = "Вопросительные предложения";
            cbQuestions.Left = 20;
            cbQuestions.Top = 280;
            cbQuestions.Checked = true;
            page.Controls.Add(cbQuestions);

            cbExclamations = new CheckBox();
            cbExclamations.Text = "Восклицательные предложения";
            cbExclamations.Left = 250;
            cbExclamations.Top = 280;
            cbExclamations.Checked = true;
            page.Controls.Add(cbExclamations);

            rbScreen = new RadioButton();
            rbScreen.Text = "Показать на экране";
            rbScreen.Left = 20;
            rbScreen.Top = 320;
            rbScreen.Checked = true;
            page.Controls.Add(rbScreen);

            rbFile = new RadioButton();
            rbFile.Text = "Сохранить в файл";
            rbFile.Left = 200;
            rbFile.Top = 320;
            page.Controls.Add(rbFile);

            Button btnStart = new Button();
            btnStart.Text = "Старт анализа";
            btnStart.Left = 20;
            btnStart.Top = 360;
            btnStart.Click += StartTextAnalysis;
            page.Controls.Add(btnStart);

            Button btnStop = new Button();
            btnStop.Text = "Остановить";
            btnStop.Left = 150;
            btnStop.Top = 360;
            btnStop.Click += StopTextAnalysis;
            page.Controls.Add(btnStop);

            Button btnRestart = new Button();
            btnRestart.Text = "Повторный запуск";
            btnRestart.Left = 270;
            btnRestart.Top = 360;
            btnRestart.Click += RestartTextAnalysis;
            page.Controls.Add(btnRestart);

            txtReport = new TextBox();
            txtReport.Left = 20;
            txtReport.Top = 410;
            txtReport.Width = 820;
            txtReport.Height = 150;
            txtReport.Multiline = true;
            txtReport.ScrollBars = ScrollBars.Vertical;
            page.Controls.Add(txtReport);

            tabs.TabPages.Add(page);
        }

        async void StartTextAnalysis(object sender, EventArgs e)
        {
            textCancel = new CancellationTokenSource();
            CancellationToken token = textCancel.Token;

            string text = txtText.Text;

            try
            {
                string report = await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();

                    int sentences = 0;
                    int questions = 0;
                    int exclamations = 0;

                    foreach (char c in text)
                    {
                        token.ThrowIfCancellationRequested();

                        if (c == '.' || c == '?' || c == '!')
                            sentences++;

                        if (c == '?')
                            questions++;

                        if (c == '!')
                            exclamations++;

                        Thread.Sleep(5);
                    }

                    string[] words = text.Split(
                        new char[] { ' ', '.', ',', '!', '?', ';', ':', '\n', '\r', '\t' },
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    StringBuilder result = new StringBuilder();

                    if (cbSentences.Checked)
                        result.AppendLine("Количество предложений: " + sentences);

                    if (cbChars.Checked)
                        result.AppendLine("Количество символов: " + text.Length);

                    if (cbWords.Checked)
                        result.AppendLine("Количество слов: " + words.Length);

                    if (cbQuestions.Checked)
                        result.AppendLine("Количество вопросительных предложений: " + questions);

                    if (cbExclamations.Checked)
                        result.AppendLine("Количество восклицательных предложений: " + exclamations);

                    return result.ToString();

                }, token);

                if (rbScreen.Checked)
                {
                    txtReport.Text = report;
                }
                else
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "Text files|*.txt";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(save.FileName, report);
                        txtReport.Text = "Отчёт сохранён в файл.";
                    }
                }
            }
            catch
            {
                txtReport.Text = "Анализ остановлен.";
            }
        }

        void StopTextAnalysis(object sender, EventArgs e)
        {
            if (textCancel != null)
                textCancel.Cancel();
        }

        void RestartTextAnalysis(object sender, EventArgs e)
        {
            if (textCancel != null)
                textCancel.Cancel();

            txtReport.Clear();

            StartTextAnalysis(sender, e);
        }

        // -------- ЗАДАНИЯ 4, 5 --------

        void CreateFilesTab()
        {
            TabPage page = new TabPage("Задания 4-5");

            Label lbl1 = new Label();
            lbl1.Text = "Директория источник:";
            lbl1.Left = 20;
            lbl1.Top = 20;
            page.Controls.Add(lbl1);

            txtSource = new TextBox();
            txtSource.Left = 180;
            txtSource.Top = 20;
            txtSource.Width = 600;
            page.Controls.Add(txtSource);

            Label lbl2 = new Label();
            lbl2.Text = "Директория приёмник:";
            lbl2.Left = 20;
            lbl2.Top = 60;
            page.Controls.Add(lbl2);

            txtReceiver = new TextBox();
            txtReceiver.Left = 180;
            txtReceiver.Top = 60;
            txtReceiver.Width = 600;
            page.Controls.Add(txtReceiver);

            Button btnStart = new Button();
            btnStart.Text = "Начать перенос";
            btnStart.Left = 20;
            btnStart.Top = 100;
            btnStart.Click += StartFilesWork;
            page.Controls.Add(btnStart);

            Button btnStop = new Button();
            btnStop.Text = "Остановить";
            btnStop.Left = 160;
            btnStop.Top = 100;
            btnStop.Click += StopFilesWork;
            page.Controls.Add(btnStop);

            listFilesReport = new ListBox();
            listFilesReport.Left = 20;
            listFilesReport.Top = 150;
            listFilesReport.Width = 820;
            listFilesReport.Height = 400;
            page.Controls.Add(listFilesReport);

            tabs.TabPages.Add(page);
        }

        async void StartFilesWork(object sender, EventArgs e)
        {
            listFilesReport.Items.Clear();

            filesCancel = new CancellationTokenSource();
            CancellationToken token = filesCancel.Token;

            string source = txtSource.Text;
            string receiver = txtReceiver.Text;

            try
            {
                List<string> report = await Task.Run(() =>
                {
                    List<string> result = new List<string>();
                    Dictionary<string, string> uniqueFiles = new Dictionary<string, string>();

                    if (!Directory.Exists(receiver))
                        Directory.CreateDirectory(receiver);

                    string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

                    foreach (string file in files)
                    {
                        token.ThrowIfCancellationRequested();

                        string hash = GetFileHash(file);

                        if (!uniqueFiles.ContainsKey(hash))
                        {
                            uniqueFiles.Add(hash, file);

                            string newPath = Path.Combine(receiver, Path.GetFileName(file));

                            if (File.Exists(newPath))
                            {
                                string name = Path.GetFileNameWithoutExtension(file);
                                string ext = Path.GetExtension(file);
                                newPath = Path.Combine(receiver, name + "_copy" + ext);
                            }

                            File.Move(file, newPath);

                            result.Add("Оригинальный файл перенесён:");
                            result.Add("Название файла: " + Path.GetFileName(newPath));
                            result.Add("Путь к файлу: " + newPath);
                            result.Add("-----------------------------------");
                        }
                        else
                        {
                            result.Add("Найден файл-двойник:");
                            result.Add("Название файла: " + Path.GetFileName(file));
                            result.Add("Путь к файлу: " + file);
                            result.Add("Действие: не переносился");
                            result.Add("-----------------------------------");
                        }

                        Thread.Sleep(100);
                    }

                    string reportPath = Path.Combine(receiver, "report.txt");
                    File.WriteAllLines(reportPath, result);

                    result.Add("Отчёт сохранён:");
                    result.Add(reportPath);

                    return result;

                }, token);

                foreach (string line in report)
                    listFilesReport.Items.Add(line);
            }
            catch
            {
                listFilesReport.Items.Add("Работа остановлена.");
            }
        }

        void StopFilesWork(object sender, EventArgs e)
        {
            if (filesCancel != null)
                filesCancel.Cancel();
        }

        string GetFileHash(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash);
                }
            }
        }
    }
}