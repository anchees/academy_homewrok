using System;
using System.Linq;
using System.Windows.Forms;

namespace task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            string[] resumePages =
            {
                "Имя: Ленар Фахрутдинов\nВозраст: 19 лет\nГород: Сургут",
                "Образование: студент 2 курса, специальность — Прикладаная матемтика и информатика",
                "Навыки: C#, Python, C++\nХобби: гитара, спорт, игры"
            };

           
            int totalChars = resumePages.Sum(page => page.Length);

            double average = (double)totalChars / resumePages.Length;

           
            for (int i = 0; i < resumePages.Length; i++)
            {
                string title;

                
                if (i == resumePages.Length - 1)
                    title = $"Среднее число символов: {average:F1}";
                else
                    title = $"Резюме ({i + 1})";

                MessageBox.Show(resumePages[i], title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
