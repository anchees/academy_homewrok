using System;
using System.Drawing;
using System.Windows.Forms;

namespace task3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
        }
  
        private Rectangle GetInnerRect()
        {
            return new Rectangle(10, 10, ClientSize.Width - 20, ClientSize.Height - 20);
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle rect = GetInnerRect();
            Point p = e.Location; 

            if (e.Button == MouseButtons.Left)
            {

                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    Close();
                    return;
                }


                string message;
                if (IsOnBorder(rect, p))
                    message = "Точка находится на границе прямоугольника.";
                else if (rect.Contains(p))
                    message = "Точка внутри прямоугольника.";
                else
                    message = "Точка снаружи прямоугольника.";

                MessageBox.Show(message, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Button == MouseButtons.Right)
            {
                Text = $"Ширина = {ClientSize.Width}, Высота = {ClientSize.Height}";
            }
        }

 
        private bool IsOnBorder(Rectangle rect, Point p)
        {
            int x = p.X, y = p.Y;

            bool onLeft = Math.Abs(x - rect.Left) <= 0;
            bool onRight = Math.Abs(x - rect.Right) <= 0;
            bool onTop = Math.Abs(y - rect.Top) <= 0;
            bool onBottom = Math.Abs(y - rect.Bottom) <= 0;

            return (onLeft || onRight || onTop || onBottom);
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Text = $"x = {e.X}, y = {e.Y}";
        }
    }
}
