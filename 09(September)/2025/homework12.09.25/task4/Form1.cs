using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace task4
{
    public partial class Form1 : Form
    {
        private Point startPoint;
        private bool isDrawing = false;
        private int nextIndex = 1;

        public Form1()
        {
            InitializeComponent();

            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                startPoint = e.Location;
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDrawing)
                return;

            isDrawing = false;

            Point endPoint = e.Location;

            int x = Math.Min(startPoint.X, endPoint.X);
            int y = Math.Min(startPoint.Y, endPoint.Y);
            int width = Math.Abs(startPoint.X - endPoint.X);
            int height = Math.Abs(startPoint.Y - endPoint.Y);

            if (width < 10 || height < 10)
            {
                MessageBox.Show("Минимальный размер 10x10!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Label lbl = new Label();
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(width, height);
            lbl.Text = nextIndex.ToString();
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Tag = nextIndex; 
            lbl.BackColor = Color.LightBlue;

            lbl.MouseClick += Static_MouseClick;
            lbl.DoubleClick += Static_DoubleClick;

            this.Controls.Add(lbl);

            nextIndex++; 
        }

        private void Static_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;


            Point clickPos = this.PointToClient(Cursor.Position);

            var hits = this.Controls
                .OfType<Label>()
                .Where(l => l.Bounds.Contains(clickPos))
                .ToList();

            if (hits.Count == 0)
                return;

            var chosen = hits.OrderByDescending(l => (int)l.Tag).First();

            int area = chosen.Width * chosen.Height;
            this.Text = $"Площадь = {area}, Координаты = ({chosen.Left}, {chosen.Top})";
        }


        private void Static_DoubleClick(object sender, EventArgs e)
        {
            Point clickPos = this.PointToClient(Cursor.Position);


            var hits = this.Controls
                .OfType<Label>()
                .Where(l => l.Bounds.Contains(clickPos))
                .ToList();

            if (hits.Count == 0)
                return;

            var chosen = hits.OrderBy(l => (int)l.Tag).First();

            this.Controls.Remove(chosen);
            chosen.Dispose();
        }
    }
}
