using System;
using System.Windows.Forms;

namespace task2
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
            StartGame(); 
        }


        private void StartGame()
        {
            int low = 1;
            int high = 2000;
            int tries = 0;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                tries++;

    
                DialogResult res = MessageBox.Show(
                    $"Ваше число больше {mid}?",
                    "Угадай число",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (res == DialogResult.Cancel)
                {
                    MessageBox.Show("Игра прервана пользователем.", "Выход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (res == DialogResult.Yes)
                {
                    low = mid + 1;
                }
                else
                {
          
                    DialogResult eq = MessageBox.Show(
                        $"Ваше число равно {mid}?",
                        "Проверка",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (eq == DialogResult.Yes)
                    {
                        MessageBox.Show(
                            $"Число угадано!\nВаше число: {mid}\nКоличество запросов: {tries}",
                            "Победа!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                   
                        DialogResult again = MessageBox.Show(
                            "Хотите сыграть ещё раз?",
                            "Продолжить?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (again == DialogResult.Yes)
                        {
                            StartGame(); 
                        }

                        return; 
                    }
                    else
                    {
                        high = mid - 1;
                    }
                }
            }

            MessageBox.Show("Не удалось определить число!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
