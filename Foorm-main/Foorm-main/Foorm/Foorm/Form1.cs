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
            enemies.Add(new Zombie(150, 100, 40));
            enemies.Add(new Zombie(100, 150, 40));
            enemies.Add(new Zombie(150, 150, 40));
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
        bool GameOver => Slayer.Health <= 0;
        public void Draw3D()
        {



       
            int w, s;
   
            double CS = Matematika.Cos(Slayer.A);
            double SN = Matematika.Sin(Slayer.A);
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            BufferedGraphics buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);
            Graphics g = buffer.Graphics;
            g.Clear(Color.White);

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

                    Sectors[s].d += Walls[w].d(Slayer.X,Slayer.Y);
                    Walls[w].Draw(g, Slayer, Sectors[s]);
                }
                Sectors[s].d /= (Sectors[s].we - Sectors[s].ws);
                foreach (var enemy in enemies)
                {
                    enemy.Draw(g, Slayer);
                }
            }
            g.DrawImage(shotgun.Image, SW2, SH);
            buffer.Render();
            buffer.Dispose();
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
            double playerAngle = Slayer.A * Math.PI / 180.0;
            label1.Text = "Ammo: "+Convert.ToString(Weapon.Ammo);
            double angleDiff=NormalizeAngle(angle-playerAngle);
            double tolerance = Math.PI / 8;

            Console.WriteLine(angle + " " + Slayer.A);
            if (Math.Abs(angleDiff)<=tolerance){ enemy.TakeDamage(Weapon.Damage); } 
        }
        double NormalizeAngle(double angle)
        {
            while (angle < -Math.PI) angle += 2 * Math.PI;
            while (angle > Math.PI) angle -= 2 * Math.PI;
            return angle;
        }

        void MovePlayer()
        {
            int newX = Slayer.X;
            int newY = Slayer.Y;
            int playerSector = GetPlayerSector(Slayer.X, Slayer.Y);
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
            if (playerSector != -1) Slayer.Z = Sectors[playerSector].z1;
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
            foreach(EnemyBase enemy in enemies)
            {
                if (enemy.IsDead) { continue; }
                enemy.EnemyMove(Slayer);
                enemy.Update(Slayer.X, Slayer.Y,20);
                Slayer.TakeDamage(enemy);
            }
            label2.Text = "Health: " + Slayer.Health;
            Slayer.Update(20);
            Draw3D();
            if(GameOver)timer1.Stop();
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
                    foreach (EnemyBase enemy in enemies) 
                    {
                        PlayerAttack(Slayer, shotgun, enemy);
                    }
                }
            }
            if (e.KeyCode == Keys.Escape) { if (timer1.Enabled) timer1.Stop(); else timer1.Start(); }
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