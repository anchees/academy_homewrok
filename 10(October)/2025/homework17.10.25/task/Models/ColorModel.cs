using System.Windows.Media;

namespace task.Models
{
    public class ColorModel
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public SolidColorBrush Brush =>
            new SolidColorBrush(Color.FromArgb(A, R, G, B));

        public string Name => $"ARGB({A}, {R}, {G}, {B})";

        public override bool Equals(object obj)
        {
            if (obj is ColorModel c)
                return A == c.A && R == c.R && G == c.G && B == c.B;
            return false;
        }

        public override int GetHashCode()
        {
            return (A, R, G, B).GetHashCode();
        }
    }
}
