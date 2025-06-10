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
    public void Draw(Graphics g,Igrac Slayer,Sector sector)
    {
        int[] wx = new int[4];
        int[] wy = new int[4];
        int[] wz = new int[4];
        int X1 = x1 - Slayer.X, Y1 = y1 - Slayer.Y;
        int X2 = x2 - Slayer.X, Y2 = y2 - Slayer.Y;
        double CS = Matematika.Cos(Slayer.A);
        double SN=Matematika.Sin(Slayer.A);
        //x sveta sirina
        wx[0] = Convert.ToInt32(X1 * CS - Y1 * SN);
        wx[1] = Convert.ToInt32(X2 * CS - Y2 * SN);
        wx[2] = wx[0];
        wx[3] = wx[1];
        //y sveta dubina
        wy[0] = Convert.ToInt32(Y1 * CS + X1 * SN);
        wy[1] = Convert.ToInt32(Y2 * CS + X2 * SN);
        wy[2] = wy[0];
        wy[3] = wy[1];

        wz[0] = Convert.ToInt32(sector.z1 - Slayer.Z + ((Slayer.L * wy[0]) / 32.0));//z sveta visina
        wz[1] = Convert.ToInt32(sector.z1 - Slayer.Z + ((Slayer.L * wy[1]) / 32.0));
        wz[2] = wz[0] + sector.z2;
        wz[3] = wz[1] + sector.z2;
        //ekran x, ekran y pozicije

        //ne crtaj ako iza igraca
        if (wy[0] < 1 && wy[1] < 1) return;
        if (wy[0] < 1)
        {
          ClipBehindPlayer(ref wx[0], ref wy[0], ref wz[0], ref wx[1], ref wy[1], ref wz[1]);//donja linija
          ClipBehindPlayer(ref wx[2], ref wy[2], ref wz[2], ref wx[3], ref wy[3], ref wz[3]);//gornja linija
            wy[0] = 1;
        }
        if (wy[1] < 1)
        {
            ClipBehindPlayer(ref wx[1], ref wy[1], ref wz[1], ref wx[0], ref wy[0], ref wz[0]);
            ClipBehindPlayer(ref wx[3], ref wy[3], ref wz[3], ref wx[2], ref wy[2], ref wz[2]);
            wy[1] = 1;
        }



        wx[0] = wx[0] * 200 / wy[0] + Foorm.Form1.ActiveForm.Width / 2;
        wy[0] = wz[0] * 200 / wy[0] + Foorm.Form1.ActiveForm.Height/2;
        wx[1] = wx[1] * 200 / wy[1] + Foorm.Form1.ActiveForm.Width/2 ; wy[1] = wz[1] * 200 / wy[1] + Foorm.Form1.ActiveForm.Height/2;
        wx[2] = wx[2] * 200 / wy[2] + Foorm.Form1.ActiveForm.Width/2 ; wy[2] = wz[2] * 200 / wy[2] + Foorm.Form1.ActiveForm.Height/2;
        wx[3] = wx[3] * 200 / wy[3] + Foorm.Form1.ActiveForm.Width/2 ; wy[3] = wz[3] * 200 / wy[3] + Foorm.Form1.ActiveForm.Height/2;


        Pen pen = new Pen(Color.Black);
        Brush brush = new SolidBrush(c);

        Point[] points = new Point[4];
        points[0] = new Point(wx[0], wy[0]);
        points[1] = new Point(wx[2], wy[2]);
        points[2] = new Point(wx[3], wy[3]);
        points[3] = new Point(wx[1], wy[1]);


        g.FillPolygon(brush, points);
    }
    void ClipBehindPlayer(ref int x1, ref int y1, ref int z1, ref int x2, ref int y2, ref int z2)
    {
        float da = y1;
        float db = y2;
        float d = da - db;
        if (d == 0) { d = 1; }
        float s = da / d;
        x1 = Convert.ToInt32(x1 + s * (x2 - (x1)));
        y1 = Convert.ToInt32(y1 + s * (y2 - (y1))); if (y1 == 0) { y1 = 1; }
        z1 = Convert.ToInt32(z1 + s * (z2 - (z1)));
    }
}
     
