using System;
using System.Drawing;
using System.Globalization;

namespace EntryColorFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("오브젝트의 원래 RGB값을 입력하세요: ");
            int rgb = int.Parse(Console.ReadLine(), NumberStyles.HexNumber);
            Color c1 = Color.FromArgb(255, (rgb >> 16) & 0xff, (rgb >> 8) & 0xff, rgb & 0xff);

            Console.Write("변환시킬 RGB값을 입력하세요: ");
            rgb = int.Parse(Console.ReadLine(), NumberStyles.HexNumber);
            Color c2 = Color.FromArgb(255, (rgb >> 16) & 0xff, (rgb >> 8) & 0xff, rgb & 0xff);

            Console.Write("반복해서 찾을 횟수를 입력하세요(10000번 정도면 비슷하게 찾습니다): ");
            int iterations = int.Parse(Console.ReadLine());

            double minValue = double.PositiveInfinity;
            double minX = 0;
            for (int i = 0; i < iterations; i++)
            {
                double x = 100.0 / iterations * i;

                double distance = Distance(c2, Apply(c1, x));
                if (distance < minValue)
                {
                    minValue = distance;
                    minX = x;
                }
            }

            Console.WriteLine();

            Color color = Apply(c1, minX);
            Console.WriteLine($"가장 가까운 색: {color.R:x2}{color.G:x2}{color.B:x2}");
            Console.WriteLine($"색깔 효과 값: {minX}");

            Console.WriteLine();
            Console.Write("엔터 키를 눌러 종료 . . .");

            Console.Read();
        }

        public static double Distance(Color a, Color b) => Math.Sqrt(Math.Pow(a.R - b.R, 2) + Math.Pow(a.G - b.G, 2) + Math.Pow(a.B - b.B, 2));

        public static Color Apply(Color color, double filter)
        {
            double r, g, b;

            if (Math.Abs(filter) % 100 <= 33)
            {
                r = color.R;
                g = color.G * Math.Cos(0.06 * filter * Math.PI) + color.B * Math.Sin(0.06 * filter * Math.PI);
                b = -color.G * Math.Sin(0.06 * filter * Math.PI) + color.B * Math.Cos(0.06 * filter * Math.PI);
            }
            else if (Math.Abs(filter) % 100 <= 66)
            {
                r = color.R * Math.Cos(0.06 * filter * Math.PI) + color.B * Math.Sin(0.06 * filter * Math.PI);
                g = color.R;
                b = color.R * Math.Sin(0.06 * filter * Math.PI) + color.B * Math.Cos(0.06 * filter * Math.PI);
            }
            else // Math.Abs(filter) % 100 <= 99
            {
                r = color.R * Math.Cos(0.06 * filter * Math.PI) + color.G * Math.Sin(0.06 * filter * Math.PI);
                g = -color.R * Math.Sin(0.06 * filter * Math.PI) + color.G * Math.Cos(0.06 * filter * Math.PI);
                b = color.B;
            }

            int RGB(double v)
            {
                return Math.Min(Math.Max((int)Math.Round(v), 0), 255);
            }

            return Color.FromArgb(255, RGB(r), RGB(g), RGB(b));
        }
    }
}
