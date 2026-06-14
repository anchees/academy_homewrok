using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        Process childProcess;

        TextBox txtProgramPath;
        TextBox txtNumber1;
        TextBox txtNumber2;
        TextBox txtOperation;

        TextBox txtFilePath;
        TextBox txtWord;

        TextBox txtResult;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        void CreateInterface()
        {
            Text = "Модуль 2. Процессы";
            Width = 850;
            Height = 600;

            Label lblProgram = new Label();
            lblProgram.Text = "Путь к ChildApp.exe:";
            lblProgram.Left = 20;
            lblProgram.Top = 20;
            lblProgram.Width = 150;
            Controls.Add(lblProgram);

            txtProgramPath = new TextBox();
            txtProgramPath.Left = 180;
            txtProgramPath.Top = 20;
            txtProgramPath.Width = 600;
            Controls.Add(txtProgramPath);

            Button btnRunWait = new Button();
            btnRunWait.Text = "Задание 1";
            btnRunWait.Left = 20;
            btnRunWait.Top = 60;
            btnRunWait.Click += RunAndWait_Click;
            Controls.Add(btnRunWait);

            Button btnRun = new Button();
            btnRun.Text = "Запустить";
            btnRun.Left = 130;
            btnRun.Top = 60;
            btnRun.Click += Run_Click;
            Controls.Add(btnRun);

            Button btnWait = new Button();
            btnWait.Text = "Ждать";
            btnWait.Left = 240;
            btnWait.Top = 60;
            btnWait.Click += Wait_Click;
            Controls.Add(btnWait);

            Button btnKill = new Button();
            btnKill.Text = "Завершить";
            btnKill.Left = 350;
            btnKill.Top = 60;
            btnKill.Click += Kill_Click;
            Controls.Add(btnKill);

            Label lbl1 = new Label();
            lbl1.Text = "Число 1:";
            lbl1.Left = 20;
            lbl1.Top = 120;
            Controls.Add(lbl1);

            txtNumber1 = new TextBox();
            txtNumber1.Left = 100;
            txtNumber1.Top = 120;
            txtNumber1.Width = 80;
            Controls.Add(txtNumber1);

            Label lbl2 = new Label();
            lbl2.Text = "Число 2:";
            lbl2.Left = 200;
            lbl2.Top = 120;
            Controls.Add(lbl2);

            txtNumber2 = new TextBox();
            txtNumber2.Left = 280;
            txtNumber2.Top = 120;
            txtNumber2.Width = 80;
            Controls.Add(txtNumber2);

            Label lbl3 = new Label();
            lbl3.Text = "Операция:";
            lbl3.Left = 380;
            lbl3.Top = 120;
            Controls.Add(lbl3);

            txtOperation = new TextBox();
            txtOperation.Left = 470;
            txtOperation.Top = 120;
            txtOperation.Width = 80;
            Controls.Add(txtOperation);

            Button btnCalc = new Button();
            btnCalc.Text = "Задание 3";
            btnCalc.Left = 570;
            btnCalc.Top = 118;
            btnCalc.Click += Calc_Click;
            Controls.Add(btnCalc);

            Label lblFile = new Label();
            lblFile.Text = "Путь к файлу:";
            lblFile.Left = 20;
            lblFile.Top = 180;
            Controls.Add(lblFile);

            txtFilePath = new TextBox();
            txtFilePath.Left = 130;
            txtFilePath.Top = 180;
            txtFilePath.Width = 500;
            Controls.Add(txtFilePath);

            Label lblWord = new Label();
            lblWord.Text = "Слово:";
            lblWord.Left = 20;
            lblWord.Top = 220;
            Controls.Add(lblWord);

            txtWord = new TextBox();
            txtWord.Left = 130;
            txtWord.Top = 220;
            txtWord.Width = 200;
            Controls.Add(txtWord);

            Button btnSearch = new Button();
            btnSearch.Text = "Задание 4";
            btnSearch.Left = 350;
            btnSearch.Top = 218;
            btnSearch.Click += SearchWord_Click;
            Controls.Add(btnSearch);

            txtResult = new TextBox();
            txtResult.Left = 20;
            txtResult.Top = 280;
            txtResult.Width = 760;
            txtResult.Height = 230;
            txtResult.Multiline = true;
            txtResult.ScrollBars = ScrollBars.Vertical;
            Controls.Add(txtResult);
        }

        void RunAndWait_Click(object sender, EventArgs e)
        {
            Process process = Process.Start(txtProgramPath.Text);

            process.WaitForExit();

            txtResult.Text = "Процесс завершён. Код завершения: " + process.ExitCode;
        }

        void Run_Click(object sender, EventArgs e)
        {
            childProcess = Process.Start(txtProgramPath.Text);
            txtResult.Text = "Дочерний процесс запущен.";
        }

        void Wait_Click(object sender, EventArgs e)
        {
            if (childProcess != null)
            {
                childProcess.WaitForExit();
                txtResult.Text = "Процесс завершён. Код завершения: " + childProcess.ExitCode;
            }
        }

        void Kill_Click(object sender, EventArgs e)
        {
            if (childProcess != null && !childProcess.HasExited)
            {
                childProcess.Kill();
                txtResult.Text = "Процесс принудительно завершён.";
            }
        }

        void Calc_Click(object sender, EventArgs e)
        {
            string args = txtNumber1.Text + " " + txtNumber2.Text + " " + txtOperation.Text;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = txtProgramPath.Text;
            info.Arguments = args;

            Process process = Process.Start(info);
            process.WaitForExit();

            txtResult.Text = "Дочерний процесс с аргументами завершён.";
        }

        void SearchWord_Click(object sender, EventArgs e)
        {
            string args = "\"" + txtFilePath.Text + "\" " + txtWord.Text;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = txtProgramPath.Text;
            info.Arguments = args;

            Process process = Process.Start(info);
            process.WaitForExit();

            txtResult.Text = "Поиск слова выполнен.";
        }
    }
}