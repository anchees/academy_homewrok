using System;
using System.Globalization;
using System.Windows.Forms;

namespace task7
{
    public partial class Form1 : Form
    {
        private Label lblInput;
        private TextBox txtDate;
        private Button btnCalc;

        private RadioButton rbYears;
        private RadioButton rbMonths;
        private RadioButton rbDays;
        private RadioButton rbHours;
        private RadioButton rbMinutes;
        private RadioButton rbSeconds;

        private Label lblResult;
        private TextBox txtResult;

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "Время до даты";
            this.Size = new System.Drawing.Size(450, 300);

            lblInput = new Label();
            lblInput.Text = "Введите дату (дд.мм.гггг):";
            lblInput.Location = new System.Drawing.Point(20, 20);
            lblInput.AutoSize = true;
            this.Controls.Add(lblInput);

            txtDate = new TextBox();
            txtDate.Location = new System.Drawing.Point(200, 18);
            txtDate.Width = 150;
            this.Controls.Add(txtDate);
            rbYears = new RadioButton() { Text = "Годы", Location = new System.Drawing.Point(20, 60), AutoSize = true };
            rbMonths = new RadioButton() { Text = "Месяцы", Location = new System.Drawing.Point(100, 60), AutoSize = true };
            rbDays = new RadioButton() { Text = "Дни", Location = new System.Drawing.Point(200, 60), AutoSize = true };
            rbHours = new RadioButton() { Text = "Часы", Location = new System.Drawing.Point(280, 60), AutoSize = true };
            rbMinutes = new RadioButton() { Text = "Минуты", Location = new System.Drawing.Point(20, 90), AutoSize = true };
            rbSeconds = new RadioButton() { Text = "Секунды", Location = new System.Drawing.Point(100, 90), AutoSize = true };

            this.Controls.AddRange(new Control[] { rbYears, rbMonths, rbDays, rbHours, rbMinutes, rbSeconds });

            rbDays.Checked = true;

            btnCalc = new Button();
            btnCalc.Text = "Вычислить";
            btnCalc.Location = new System.Drawing.Point(200, 90);
            btnCalc.Click += BtnCalc_Click;
            this.Controls.Add(btnCalc);

            lblResult = new Label();
            lblResult.Text = "Результат:";
            lblResult.Location = new System.Drawing.Point(20, 140);
            lblResult.AutoSize = true;
            this.Controls.Add(lblResult);

            txtResult = new TextBox();
            txtResult.Location = new System.Drawing.Point(20, 170);
            txtResult.Width = 350;
            txtResult.ReadOnly = true;
            this.Controls.Add(txtResult);
        }

        private void BtnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime target = DateTime.ParseExact(txtDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                DateTime now = DateTime.Now;

                if (target < now)
                {
                    txtResult.Text = "Указанная дата уже прошла!";
                    return;
                }

                TimeSpan diff = target - now;
                double result = 0;
                string unit = "";

                if (rbYears.Checked)
                {
                    result = diff.TotalDays / 365.25;
                    unit = "лет";
                }
                else if (rbMonths.Checked)
                {
                    result = diff.TotalDays / 30.44; 
                    unit = "месяцев";
                }
                else if (rbDays.Checked)
                {
                    result = diff.TotalDays;
                    unit = "дней";
                }
                else if (rbHours.Checked)
                {
                    result = diff.TotalHours;
                    unit = "часов";
                }
                else if (rbMinutes.Checked)
                {
                    result = diff.TotalMinutes;
                    unit = "минут";
                }
                else if (rbSeconds.Checked)
                {
                    result = diff.TotalSeconds;
                    unit = "секунд";
                }

                txtResult.Text = $"{result:F2} {unit}";
            }
            catch
            {
                MessageBox.Show("Введите дату в формате дд.мм.гггг", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
