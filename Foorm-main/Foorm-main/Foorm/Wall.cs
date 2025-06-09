using System;

public class Wall
{
    public int x1, y1;
    public int x2, y2;
    public int d(int px, int py)
    {
        double dx = x2 - x1;
        double dy = y2 - y1;
        double t = ((px - x1) * dx + (py - y1) * dy) / (dx * dx + dy * dy);
        t = Math.Max(0, Math.Min(1, t));
        double closestX = x1 + t * dx;
        double closestY = y1 + t * dy;
        double distX = px - closestX;
        double distY = py - closestY;
        return Convert.ToInt32(Math.Sqrt(distX * distX + distY * distY));
    }
    public Color c;
    public Wall(int x1, int y1, int x2, int y2, Color Color)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.c = Color;
    }
}
     }
