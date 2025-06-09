using System;

public abstract class Matematika
{
    public static double Sin(double x) { return Math.Sin(x / 180 * Math.PI); }
    public static double Cos(double x) { return Math.Cos(x / 180 * Math.PI); }

    public static int distance(int x1, int y1, int x2, int y2)
    {
        return Convert.ToInt32(Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
    }

}