using System;
using System.Drawing;
using System.Windows.Forms;

namespace task5
{
    public partial class Form1 : Form
    {
        private Label runawayLabel; 
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeRunawayLabel();
            this.MouseMove += Form1_MouseMove;
        }

        private void InitializeRunawayLabel()
        {
            runawayLabel = new Label();
            runawayLabel.Text = "Поймай меня!";
            runawayLabel.AutoSize = false;
            runawayLabel.Size = new Size(100, 40);
            runawayLabel.BackColor = Color.LightCoral;
            runawayLabel.TextAlign = ContentAlignment.MiddleCenter;
            runawayLabel.Location = new Point(100, 100);

            this.Controls.Add(runawayLabel);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int centerX = runawayLabel.Left + runawayLabel.Width / 2;
            int centerY = runawayLabel.Top + runawayLabel.Height / 2;

            double distance = Math.Sqrt(Math.Pow(centerX - e.X, 2) + Math.Pow(centerY - e.Y, 2));

            if (distance < 80)
            {
                MoveLabelAway(e.Location);
            }
        }

        private void MoveLabelAway(Point mouse)
        {
            int step = 30; 

            int newX = runawayLabel.Left;
            int newY = runawayLabel.Top;

            if (mouse.X < runawayLabel.Left)
                newX += step;
            else
                newX -= step;

            if (mouse.Y < runawayLabel.Top)
                newY += step;
            else
                newY -= step;

            if (newX < 0) newX = 0;
            if (newY < 0) newY = 0;
            if (newX + runawayLabel.Width > this.ClientSize.Width)
                newX = this.ClientSize.Width - runawayLabel.Width;
            if (newY + runawayLabel.Height > this.ClientSize.Height)
                newY = this.ClientSize.Height - runawayLabel.Height;
            runawayLabel.Location = new Point(newX, newY);
        }
    }
}
