using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace task2
{
    public class Form1 : Form
    {
        private TextBox nameBox, surnameBox, emailBox, phoneBox;
        private Button addButton, exportTxtButton, importTxtButton, exportXmlButton, importXmlButton;
        private ListBox listBox;
        private List<User> users = new List<User>();
        private int editIndex = -1;

        public Form1()
        {
            this.Text = "Анкета пользователей";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label nameLabel = new Label() { Text = "Имя:", Location = new System.Drawing.Point(20, 20) };
            nameBox = new TextBox() { Location = new System.Drawing.Point(100, 20), Width = 150 };

            Label surnameLabel = new Label() { Text = "Фамилия:", Location = new System.Drawing.Point(20, 50) };
            surnameBox = new TextBox() { Location = new System.Drawing.Point(100, 50), Width = 150 };

            Label emailLabel = new Label() { Text = "Email:", Location = new System.Drawing.Point(20, 80) };
            emailBox = new TextBox() { Location = new System.Drawing.Point(100, 80), Width = 150 };

            Label phoneLabel = new Label() { Text = "Телефон:", Location = new System.Drawing.Point(20, 110) };
            phoneBox = new TextBox() { Location = new System.Drawing.Point(100, 110), Width = 150 };

            addButton = new Button() { Text = "Добавить", Location = new System.Drawing.Point(100, 140), Width = 150 };
            addButton.Click += AddButton_Click;

            listBox = new ListBox() { Location = new System.Drawing.Point(270, 20), Width = 200, Height = 200 };
            listBox.DoubleClick += ListBox_DoubleClick;

            exportTxtButton = new Button() { Text = "Экспорт TXT", Location = new System.Drawing.Point(20, 200), Width = 100 };
            importTxtButton = new Button() { Text = "Импорт TXT", Location = new System.Drawing.Point(130, 200), Width = 100 };
            exportXmlButton = new Button() { Text = "Экспорт XML", Location = new System.Drawing.Point(20, 240), Width = 100 };
            importXmlButton = new Button() { Text = "Импорт XML", Location = new System.Drawing.Point(130, 240), Width = 100 };

            exportTxtButton.Click += ExportTxtButton_Click;
            importTxtButton.Click += ImportTxtButton_Click;
            exportXmlButton.Click += ExportXmlButton_Click;
            importXmlButton.Click += ImportXmlButton_Click;

            Controls.AddRange(new Control[]
            {
                nameLabel, nameBox,
                surnameLabel, surnameBox,
                emailLabel, emailBox,
                phoneLabel, phoneBox,
                addButton,
                listBox,
                exportTxtButton, importTxtButton,
                exportXmlButton, importXmlButton
            });
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string name = nameBox.Text.Trim();
            string surname = surnameBox.Text.Trim();
            string email = emailBox.Text.Trim();
            string phone = phoneBox.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname))
            {
                MessageBox.Show("Имя и фамилия обязательны!");
                return;
            }

            User user = new User { Name = name, Surname = surname, Email = email, Phone = phone };

            if (editIndex == -1)
            {
                users.Add(user);
            }
            else
            {
                users[editIndex] = user;
                editIndex = -1;
                addButton.Text = "Добавить";
            }

            UpdateList();
            ClearFields();
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1) return;

            DialogResult result = MessageBox.Show("Редактировать (Да) или удалить (Нет)?", "Выбор действия", MessageBoxButtons.YesNoCancel);
            int index = listBox.SelectedIndex;

            if (result == DialogResult.Yes)
            {
                var user = users[index];
                nameBox.Text = user.Name;
                surnameBox.Text = user.Surname;
                emailBox.Text = user.Email;
                phoneBox.Text = user.Phone;
                editIndex = index;
                addButton.Text = "Сохранить";
            }
            else if (result == DialogResult.No)
            {
                users.RemoveAt(index);
                UpdateList();
            }
        }

        private void ExportTxtButton_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("users.txt"))
            {
                foreach (var u in users)
                {
                    sw.WriteLine($"{u.Name}|{u.Surname}|{u.Email}|{u.Phone}");
                }
            }
            MessageBox.Show("Экспорт в TXT выполнен!");
        }

        private void ImportTxtButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists("users.txt"))
            {
                MessageBox.Show("Файл users.txt не найден!");
                return;
            }

            users.Clear();
            foreach (var line in File.ReadAllLines("users.txt"))
            {
                var parts = line.Split('|');
                if (parts.Length == 4)
                    users.Add(new User { Name = parts[0], Surname = parts[1], Email = parts[2], Phone = parts[3] });
            }

            UpdateList();
            MessageBox.Show("Импорт из TXT выполнен!");
        }

        private void ExportXmlButton_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream("users.xml", FileMode.Create))
            {
                serializer.Serialize(fs, users);
            }
            MessageBox.Show("Экспорт в XML выполнен!");
        }

        private void ImportXmlButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists("users.xml"))
            {
                MessageBox.Show("Файл users.xml не найден!");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream("users.xml", FileMode.Open))
            {
                users = (List<User>)serializer.Deserialize(fs);
            }
            UpdateList();
            MessageBox.Show("Импорт из XML выполнен!");
        }

        private void UpdateList()
        {
            listBox.Items.Clear();
            foreach (var u in users)
            {
                listBox.Items.Add($"{u.Name} {u.Surname} ({u.Email}, {u.Phone})");
            }
        }

        private void ClearFields()
        {
            nameBox.Clear();
            surnameBox.Clear();
            emailBox.Clear();
            phoneBox.Clear();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        public class User
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}
