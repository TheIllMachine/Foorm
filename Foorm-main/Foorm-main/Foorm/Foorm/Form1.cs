using Microsoft.VisualBasic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Foorm
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();


            Sectors[0] = new Sector(0, 4, 0, 40);
            Sectors[1] = new Sector(4, 8, 0, 40);
            Sectors[2] = new Sector(8, 12, 0, 40);
            Sectors[3] = new Sector(12, 16, 0, 40);
            Walls[0] = new Wall(0, 0, 320, 0, Color.Yellow);
            Walls[1] = new Wall(320, 0, 320, 320, Color.Orange);
            Walls[2] = new Wall(320, 320, 0, 320, Color.Yellow);
            Walls[3] = new Wall(0, 320, 0, 0, Color.Orange);

            Walls[4] = new Wall(640, 0, 960, 0, Color.Red);
            Walls[5] = new Wall(960, 0, 960, 320, Color.DarkRed);
            Walls[6] = new Wall(960, 320, 640, 320, Color.Red);
            Walls[7] = new Wall(640, 320, 640, 0, Color.DarkRed);

            Walls[8] = new Wall(640, 640, 960, 640, Color.Purple);
            Walls[9] = new Wall(960, 640, 960, 960, Color.DeepPink);
            Walls[10] = new Wall(960, 960, 640, 960, Color.Purple);
            Walls[11] = new Wall(640, 960, 640, 640, Color.DeepPink);

            Walls[12] = new Wall(0, 640, 320, 640, Color.SkyBlue);
            Walls[13] = new Wall(320, 640, 320, 960, Color.Blue);
            Walls[14] = new Wall(320, 960, 0, 960, Color.SkyBlue);
            Walls[15] = new Wall(0, 960, 0, 640, Color.Blue);

            enemies.Add(new Zombie(100, 100, 40));
            timer1.Start();

        }
        static Igrac Slayer = new Igrac(70, -110, 20, 0, 0);
        public static int res = 1;
        public static int SW2 = 80 * 8;
        public static int SH2 = 60 * 8;
        public static int SW = SW2 * 2;
        public static int SH = SH2 * 2;
        public static K k = new K();
        public static Sector[] Sectors = new Sector[30];
        public static Wall[] Walls = new Wall[100];
        public static int NumSect = 4;
        public static int NumWalls = 16;
        public List<EnemyBase> enemies = new List<EnemyBase>();
        const float playerRadius = 12.0f;
        Shotgun shotgun = new Shotgun();
        public void Draw3D()
        {



            int[] wx, wy, wz;
            int w, s;
            wx = new int[4];
            wy = new int[4];
            wz = new int[4];
            double CS = Matematika.Cos(Slayer.A);
            double SN = Matematika.Sin(Slayer.A);
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            BufferedGraphics buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);
            Graphics g = buffer.Graphics;
            g.Clear(Color.White);
            g.DrawImage(shotgun.Image, 500, 500);
            for (s = 0; s < NumSect; s++)
            {
                for (w = 0; w < NumSect - s - 1; w++)
                {
                    if (Sectors[w].d < Sectors[w + 1].d)
                    {
                        Sector st = Sectors[w + 1];
                        Sectors[w + 1] = Sectors[w];
                        Sectors[w] = st;
                    }
                }
            }

            for (s = 0; s < NumSect; s++)
            {
                Sectors[s].d = 0;
                for (int a = Sectors[s].ws; a < Sectors[s].we - 1; a++)
                {
                    if (Walls[a].d(Slayer.X, Slayer.Y) < Walls[a + 1].d(Slayer.X, Slayer.Y))
                    {
                        Wall st = Walls[a + 1];
                        Walls[a + 1] = Walls[a];
                        Walls[a] = st;

                    }
                }

                for (w = Sectors[s].ws; w < Sectors[s].we; w++)
                {

                    int x1 = Walls[w].x1 - Slayer.X, y1 = Walls[w].y1 - Slayer.Y;
                    int x2 = Walls[w].x2 - Slayer.X, y2 = Walls[w].y2 - Slayer.Y;
                    //x sveta sirina
                    wx[0] = Convert.ToInt32(x1 * CS - y1 * SN);
                    wx[1] = Convert.ToInt32(x2 * CS - y2 * SN);
                    wx[2] = wx[0];
                    wx[3] = wx[1];
                    //y sveta dubina
                    wy[0] = Convert.ToInt32(y1 * CS + x1 * SN);
                    wy[1] = Convert.ToInt32(y2 * CS + x2 * SN);
                    wy[2] = wy[0];
                    wy[3] = wy[1];
                    Sectors[s].d += Matematika.distance(0, 0, (wx[0] + wx[1]) / 2, (wy[0] + wy[1]) / 2); //belezimo udaljenost od zida

                    wz[0] = Convert.ToInt32(Sectors[s].z1 - Slayer.Z + ((Slayer.L * wy[0]) / 32.0));//z sveta visina
                    wz[1] = Convert.ToInt32(Sectors[s].z1 - Slayer.Z + ((Slayer.L * wy[1]) / 32.0));
                    wz[2] = wz[0] + Sectors[s].z2;
                    wz[3] = wz[1] + Sectors[s].z2;
                    //ekran x, ekran y pozicije

                    //ne crtaj ako iza igraca
                    if (wy[0] < 1 && wy[1] < 1) continue;
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



                    wx[0] = wx[0] * 200 / wy[0] + SW2;
                    wy[0] = wz[0] * 200 / wy[0] + SH2;
                    wx[1] = wx[1] * 200 / wy[1] + SW2; wy[1] = wz[1] * 200 / wy[1] + SH2;
                    wx[2] = wx[2] * 200 / wy[2] + SW2; wy[2] = wz[2] * 200 / wy[2] + SH2;
                    wx[3] = wx[3] * 200 / wy[3] + SW2; wy[3] = wz[3] * 200 / wy[3] + SH2;


                    Pen pen = new Pen(Color.Black);
                    Brush brush = new SolidBrush(Walls[w].c);

                    Point[] points = new Point[4];
                    points[0] = new Point(wx[0], wy[0]);
                    points[1] = new Point(wx[2], wy[2]);
                    points[2] = new Point(wx[3], wy[3]);
                    points[3] = new Point(wx[1], wy[1]);


                    g.FillPolygon(brush, points);



                }
                Sectors[s].d /= (Sectors[s].we - Sectors[s].ws);
                foreach (var enemy in enemies)
                {
                    enemy.Draw(g, Slayer);
                }




            }

            buffer.Render();
            buffer.Dispose();
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
        bool CheckCollision(float newX, float newY, int sector)
        {
            if (sector == -1) return false;
            for (int w = Sectors[sector].ws; w < Sectors[sector].we; w++)
            {
                if (Walls[w].d(Slayer.X, Slayer.Y) < playerRadius)
                {
                    return true; // Collision detected
                }
            }
            return false; // No collision
        }
        bool IsPointInPolygon(float px, float py, List<PointF> polygon)
        {
            bool inside = false;
            int count = polygon.Count;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                if (((polygon[i].Y > py) != (polygon[j].Y > py)) &&
                    (px < (polygon[j].X - polygon[i].X) * (py - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }
        int GetPlayerSector(float playerX, float playerY)
        {
            for (int s = 0; s < NumSect; s++)
            {
                // Build polygon points for this sector from its walls
                List<PointF> polygon = new List<PointF>();

                for (int w = Sectors[s].ws; w < Sectors[s].we; w++)
                {
                    // Use wall start points as polygon vertices
                    polygon.Add(new PointF(Walls[w].x1, Walls[w].y1));
                }

                if (IsPointInPolygon(playerX, playerY, polygon))
                {
                    return s; // Player is inside sector s
                }
            }

            return -1;
        }
        public void PlayerAttack(Igrac Slayer, Weapon Weapon, EnemyBase enemy)
        {
            double dx =  enemy.X-Slayer.X;
            double dy =  enemy.Y-Slayer.Y;
            double angle = Math.Atan2(dx, dy);
            double playerAngle = Math.Atan(Convert.ToDouble(Slayer.A)/180*Math.PI);
          
          
            
                Console.WriteLine(angle + " " + Slayer.A);
         if(angle>playerAngle-Math.PI/16&&angle<playerAngle+Math.PI/16){ enemy.TakeDamage(Weapon.Damage); } 
        }

        void MovePlayer()
        {
            int newX = Slayer.X;
            int newY = Slayer.Y;
            if (k.a && !k.m) { Slayer.A -= 4; if (Slayer.A < 0) Slayer.A += 360; }
            if (k.d && !k.m) { Slayer.A += 4; if (Slayer.A > 359) Slayer.A -= 360; }
            int dx = Convert.ToInt32(Matematika.Sin(Slayer.A) * 10);
            int dy = Convert.ToInt32(Matematika.Cos(Slayer.A) * 10);
            if (k.w && !k.m) { Slayer.X += dx; Slayer.Y += dy; }
            if (k.s && !k.m) { Slayer.X -= dx; Slayer.Y -= dy; }
            if (k.sr) { Slayer.X += dy; Slayer.Y -= dx; }
            if (k.sl) { Slayer.X -= dy; Slayer.Y += dx; }
            if (k.a && k.m) { Slayer.L -= 1; }
            if (k.d && k.m) { Slayer.L += 1; }
            if (k.w && k.m) { Slayer.Z -= 4; }
            if (k.s && k.m) { Slayer.Z += 4; }
            if (CheckCollision(Slayer.X, Slayer.Y, GetPlayerSector(Slayer.X, Slayer.Y)))
            {
                Slayer.X = newX; Slayer.Y = newY;
            }
        }
        public void Clear()
        {
            Graphics g = this.CreateGraphics();
            g.Clear(Color.White);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            MovePlayer();
            shotgun.Update(20);
            
            Draw3D();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { k.w = true; }
            if (e.KeyCode == Keys.S) { k.s = true; }
            if (e.KeyCode == Keys.D) { k.d = true; }
            if (e.KeyCode == Keys.A) { k.a = true; }
            if (e.KeyCode == Keys.M) { k.m = true; }
            if (e.KeyCode == Keys.N) { k.sr = true; }
            if (e.KeyCode == Keys.B) { k.sl = true; }
            if (e.KeyCode == Keys.Space && shotgun.CanShoot())
            {
                shotgun.Shoot(); 
                {
                    foreach (var enemy in enemies) 
                    {
                        PlayerAttack(Slayer, shotgun, enemies[0]);
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { k.w = false; }
            if (e.KeyCode == Keys.S) { k.s = false; }
            if (e.KeyCode == Keys.D) { k.d = false; }
            if (e.KeyCode == Keys.A) { k.a = false; }
            if (e.KeyCode == Keys.M) { k.m = false; }
            if (e.KeyCode == Keys.N) { k.sr = false; }
            if (e.KeyCode == Keys.B) { k.sl = false; }
            
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            SH = Form1.ActiveForm.Height;
            SW = Form1.ActiveForm.Width;
            SH2 = SH / 2;
            SW2 = SW / 2;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }
        }
        public class K
        {
            public bool w, a, s, d, sl, sr, m;
        }
    }