using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace task8
{
    public partial class Form1 : Form
    {
        private Dictionary<string, double> fuels = new Dictionary<string, double>
        {
            {"А-95", 50.0},
            {"А-92", 46.0},
            {"ДТ", 48.0}
        };

        private Dictionary<string, double> cafeItems = new Dictionary<string, double>
        {
            {"Кофе", 25.0},
            {"Сэндвич", 40.0},
            {"Пирожок", 15.0}
        };

        private ComboBox cmbFuel;
        private TextBox txtPrice, txtLiters, txtSum;
        private RadioButton rbLiters, rbMoney;
        private CheckBox cbCoffee, cbSandwich, cbPie;
        private TextBox txtCoffeeQty, txtSandwichQty, txtPieQty;
        private Button btnCalculate;
        private TextBox txtTotal;

        public Form1()
        {
            InitializeComponent();
            InitUI();
        }

        private void InitUI()
        {
            this.Text = "BestOil";
            this.Size = new Size(600, 400);

            Label lblFuel = new Label() { Text = "Топливо:", Location = new Point(20, 20), AutoSize = true };
            this.Controls.Add(lblFuel);

            cmbFuel = new ComboBox() { Location = new Point(100, 18), Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFuel.Items.AddRange(fuels.Keys.ToArray());
            cmbFuel.SelectedIndex = 0;
            cmbFuel.SelectedIndexChanged += CmbFuel_SelectedIndexChanged;
            this.Controls.Add(cmbFuel);

            Label lblPrice = new Label() { Text = "Цена:", Location = new Point(220, 20), AutoSize = true };
            this.Controls.Add(lblPrice);

            txtPrice = new TextBox() { Location = new Point(270, 18), Width = 60, ReadOnly = true };
            this.Controls.Add(txtPrice);

            rbLiters = new RadioButton() { Text = "Литры", Location = new Point(20, 60), Checked = true };
            rbLiters.CheckedChanged += PaymentMethodChanged;
            this.Controls.Add(rbLiters);

            rbMoney = new RadioButton() { Text = "Сумма", Location = new Point(100, 60) };
            rbMoney.CheckedChanged += PaymentMethodChanged;
            this.Controls.Add(rbMoney);

            txtLiters = new TextBox() { Location = new Point(180, 58), Width = 80 };
            this.Controls.Add(txtLiters);

            txtSum = new TextBox() { Location = new Point(280, 58), Width = 80, Enabled = false };
            this.Controls.Add(txtSum);

            cbCoffee = new CheckBox() { Text = "Кофе", Location = new Point(20, 100), AutoSize = true };
            cbCoffee.CheckedChanged += CafeItem_CheckedChanged;
            this.Controls.Add(cbCoffee);
            txtCoffeeQty = new TextBox() { Location = new Point(120, 98), Width = 50, Enabled = false };
            this.Controls.Add(txtCoffeeQty);

            cbSandwich = new CheckBox() { Text = "Сэндвич", Location = new Point(20, 130), AutoSize = true };
            cbSandwich.CheckedChanged += CafeItem_CheckedChanged;
            this.Controls.Add(cbSandwich);
            txtSandwichQty = new TextBox() { Location = new Point(120, 128), Width = 50, Enabled = false };
            this.Controls.Add(txtSandwichQty);

            cbPie = new CheckBox() { Text = "Пирожок", Location = new Point(20, 160), AutoSize = true };
            cbPie.CheckedChanged += CafeItem_CheckedChanged;
            this.Controls.Add(cbPie);
            txtPieQty = new TextBox() { Location = new Point(120, 158), Width = 50, Enabled = false };
            this.Controls.Add(txtPieQty);

            btnCalculate = new Button() { Text = "Рассчитать", Location = new Point(20, 200) };
            btnCalculate.Click += BtnCalculate_Click;
            this.Controls.Add(btnCalculate);

            txtTotal = new TextBox() { Location = new Point(120, 200), Width = 100, ReadOnly = true };
            this.Controls.Add(txtTotal);

            UpdatePrice();
        }

        private void CmbFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            string fuel = cmbFuel.SelectedItem.ToString();
            txtPrice.Text = fuels[fuel].ToString("F2");
        }

        private void PaymentMethodChanged(object sender, EventArgs e)
        {
            if (rbLiters.Checked)
            {
                txtLiters.Enabled = true;
                txtSum.Enabled = false;
            }
            else
            {
                txtLiters.Enabled = false;
                txtSum.Enabled = true;
            }
        }

        private void CafeItem_CheckedChanged(object sender, EventArgs e)
        {
            txtCoffeeQty.Enabled = cbCoffee.Checked;
            txtSandwichQty.Enabled = cbSandwich.Checked;
            txtPieQty.Enabled = cbPie.Checked;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            double total = 0;

            double pricePerLiter = fuels[cmbFuel.SelectedItem.ToString()];
            if (rbLiters.Checked)
            {
                if (double.TryParse(txtLiters.Text, out double liters))
                    total += liters * pricePerLiter;
            }
            else
            {
                if (double.TryParse(txtSum.Text, out double sum))
                    total += sum;
            }

            if (cbCoffee.Checked && double.TryParse(txtCoffeeQty.Text, out double q1))
                total += q1 * cafeItems["Кофе"];
            if (cbSandwich.Checked && double.TryParse(txtSandwichQty.Text, out double q2))
                total += q2 * cafeItems["Сэндвич"];
            if (cbPie.Checked && double.TryParse(txtPieQty.Text, out double q3))
                total += q3 * cafeItems["Пирожок"];

            txtTotal.Text = total.ToString("F2");
        }
    }
}
