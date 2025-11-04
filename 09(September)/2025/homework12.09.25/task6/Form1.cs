using System;
using System.Globalization;
using System.Windows.Forms;

namespace task6
{
    public partial class Form1 : Form
    {
        private Label lblInput;
        private TextBox txtDate;
        private Button btnCheck;
        private Label lblResult;
        private TextBox txtResult;

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "Определение дня недели";
            this.Size = new System.Drawing.Size(400, 200);

            lblInput = new Label();
            lblInput.Text = "Введите дату (дд.мм.гггг):";
            lblInput.Location = new System.Drawing.Point(20, 20);
            lblInput.AutoSize = true;
            this.Controls.Add(lblInput);

            txtDate = new TextBox();
            txtDate.Location = new System.Drawing.Point(200, 18);
            txtDate.Width = 150;
            this.Controls.Add(txtDate);

            btnCheck = new Button();
            btnCheck.Text = "Определить день";
            btnCheck.Location = new System.Drawing.Point(120, 60);
            btnCheck.Click += BtnCheck_Click;
            this.Controls.Add(btnCheck);

            lblResult = new Label();
            lblResult.Text = "День недели:";
            lblResult.Location = new System.Drawing.Point(20, 110);
            lblResult.AutoSize = true;
            this.Controls.Add(lblResult);

            txtResult = new TextBox();
            txtResult.Location = new System.Drawing.Point(200, 108);
            txtResult.Width = 150;
            txtResult.ReadOnly = true;
            this.Controls.Add(txtResult);
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.ParseExact(txtDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                string dayOfWeek = date.ToString("dddd", new CultureInfo("ru-RU"));

                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);

                txtResult.Text = dayOfWeek;
            }
            catch
            {
                MessageBox.Show("Введите дату в формате дд.мм.гггг", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
