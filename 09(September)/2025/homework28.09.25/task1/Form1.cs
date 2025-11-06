using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task1
{
    public class Form1 : Form
    {
        private Button btnRead;
        private ProgressBar progress;
        private TextBox textBox;

        public Form1()
        {
            this.Text = "Чтение файла";
            this.Width = 400;
            this.Height = 300;
            this.StartPosition = FormStartPosition.CenterScreen;

            btnRead = new Button { Text = "Прочитать файл", Location = new System.Drawing.Point(20,20) };
            btnRead.Click += BtnRead_Click;
            this.Controls.Add(btnRead);

            progress = new ProgressBar { Location = new System.Drawing.Point(20,60), Width = 340 };
            this.Controls.Add(progress);

            textBox = new TextBox { Location = new System.Drawing.Point(20,100), Width = 340, Height = 120, Multiline = true, ScrollBars = ScrollBars.Vertical };
            this.Controls.Add(textBox);
        }

        private async void BtnRead_Click(object sender, EventArgs e)
        {
            string filePath = "text.txt";
            if (!File.Exists(filePath)) { MessageBox.Show("Файл text.txt не найден!"); return; }

            textBox.Clear();
            progress.Value = 0;

            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] buffer = new char[256];
                int readCount;
                long total = reader.BaseStream.Length;
                long read = 0;
                while ((readCount = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    textBox.AppendText(new string(buffer, 0, readCount));
                    read += readCount;
                    progress.Value = (int)((double)read / total * 100);
                    await Task.Delay(20);
                }
            }

            MessageBox.Show("Готово");
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
