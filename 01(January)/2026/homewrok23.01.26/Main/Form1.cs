using System;
using System.Threading;
using System.Windows.Forms;

namespace Main
{
    public partial class Form1 : Form
    {
        Thread primeThread;
        Thread fibThread;

        bool stopPrime = false;
        bool stopFib = false;

        ManualResetEvent pausePrime = new ManualResetEvent(true);
        ManualResetEvent pauseFib = new ManualResetEvent(true);

        TextBox txtMin;
        TextBox txtMax;

        ListBox lstPrime;
        ListBox lstFib;

        Button btnStart;
        Button btnStopPrime;
        Button btnStopFib;
        Button btnPausePrime;
        Button btnPauseFib;
        Button btnResumePrime;
        Button btnResumeFib;
        Button btnRestart;

        public Form1()
        {
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            Width = 900;
            Height = 600;

            Label lbl1 = new Label();
            lbl1.Text = "Нижняя граница:";
            lbl1.Left = 20;
            lbl1.Top = 20;
            Controls.Add(lbl1);

            txtMin = new TextBox();
            txtMin.Left = 150;
            txtMin.Top = 20;
            txtMin.Width = 100;
            Controls.Add(txtMin);

            Label lbl2 = new Label();
            lbl2.Text = "Верхняя граница:";
            lbl2.Left = 280;
            lbl2.Top = 20;
            Controls.Add(lbl2);

            txtMax = new TextBox();
            txtMax.Left = 420;
            txtMax.Top = 20;
            txtMax.Width = 100;
            Controls.Add(txtMax);

            btnStart = new Button();
            btnStart.Text = "Старт";
            btnStart.Left = 550;
            btnStart.Top = 18;
            btnStart.Click += Start_Click;
            Controls.Add(btnStart);

            btnRestart = new Button();
            btnRestart.Text = "Рестарт";
            btnRestart.Left = 650;
            btnRestart.Top = 18;
            btnRestart.Click += Restart_Click;
            Controls.Add(btnRestart);

            lstPrime = new ListBox();
            lstPrime.Left = 20;
            lstPrime.Top = 70;
            lstPrime.Width = 350;
            lstPrime.Height = 350;
            Controls.Add(lstPrime);

            lstFib = new ListBox();
            lstFib.Left = 450;
            lstFib.Top = 70;
            lstFib.Width = 350;
            lstFib.Height = 350;
            Controls.Add(lstFib);

            btnStopPrime = new Button();
            btnStopPrime.Text = "Стоп простые";
            btnStopPrime.Left = 20;
            btnStopPrime.Top = 450;
            btnStopPrime.Click += (s, e) => stopPrime = true;
            Controls.Add(btnStopPrime);

            btnPausePrime = new Button();
            btnPausePrime.Text = "Пауза простые";
            btnPausePrime.Left = 140;
            btnPausePrime.Top = 450;
            btnPausePrime.Click += (s, e) => pausePrime.Reset();
            Controls.Add(btnPausePrime);

            btnResumePrime = new Button();
            btnResumePrime.Text = "Продолжить";
            btnResumePrime.Left = 280;
            btnResumePrime.Top = 450;
            btnResumePrime.Click += (s, e) => pausePrime.Set();
            Controls.Add(btnResumePrime);

            btnStopFib = new Button();
            btnStopFib.Text = "Стоп Фибоначчи";
            btnStopFib.Left = 450;
            btnStopFib.Top = 450;
            btnStopFib.Click += (s, e) => stopFib = true;
            Controls.Add(btnStopFib);

            btnPauseFib = new Button();
            btnPauseFib.Text = "Пауза Фибоначчи";
            btnPauseFib.Left = 590;
            btnPauseFib.Top = 450;
            btnPauseFib.Click += (s, e) => pauseFib.Reset();
            Controls.Add(btnPauseFib);

            btnResumeFib = new Button();
            btnResumeFib.Text = "Продолжить";
            btnResumeFib.Left = 740;
            btnResumeFib.Top = 450;
            btnResumeFib.Click += (s, e) => pauseFib.Set();
            Controls.Add(btnResumeFib);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            StartThreads();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            StopThreads();

            lstPrime.Items.Clear();
            lstFib.Items.Clear();

            Thread.Sleep(200);

            StartThreads();
        }

        private void StartThreads()
        {
            stopPrime = false;
            stopFib = false;

            pausePrime.Set();
            pauseFib.Set();

            int min = 2;

            if (txtMin.Text != "")
                min = Convert.ToInt32(txtMin.Text);

            bool hasMax = false;
            int max = 0;

            if (txtMax.Text != "")
            {
                max = Convert.ToInt32(txtMax.Text);
                hasMax = true;
            }

            primeThread = new Thread(() => GeneratePrimes(min, max, hasMax));
            fibThread = new Thread(() => GenerateFibonacci(min, max, hasMax));

            primeThread.Start();
            fibThread.Start();
        }

        private void StopThreads()
        {
            stopPrime = true;
            stopFib = true;

            pausePrime.Set();
            pauseFib.Set();
        }

        private void GeneratePrimes(int min, int max, bool hasMax)
        {
            int number = min;

            if (number < 2)
                number = 2;

            while (!stopPrime)
            {
                pausePrime.WaitOne();

                if (hasMax && number > max)
                    break;

                if (IsPrime(number))
                {
                    AddItem(lstPrime, number.ToString());
                    Thread.Sleep(200);
                }

                number++;
            }
        }

        private void GenerateFibonacci(int min, int max, bool hasMax)
        {
            long a = 0;
            long b = 1;

            while (!stopFib)
            {
                pauseFib.WaitOne();

                if (a >= min)
                {
                    if (hasMax && a > max)
                        break;

                    AddItem(lstFib, a.ToString());
                    Thread.Sleep(200);
                }

                long c = a + b;
                a = b;
                b = c;
            }
        }

        private bool IsPrime(int n)
        {
            if (n < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }

        private void AddItem(ListBox listBox, string text)
        {
            if (listBox.InvokeRequired)
            {
                listBox.Invoke(new Action(() =>
                {
                    listBox.Items.Add(text);
                }));
            }
            else
            {
                listBox.Items.Add(text);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopThreads();
            base.OnFormClosing(e);
        }
    }
}