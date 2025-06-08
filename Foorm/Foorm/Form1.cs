using Microsoft.VisualBasic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            Walls[0] = new Wall(0, 0, 32, 0, Color.Yellow);
            Walls[1] = new Wall(32, 0, 32, 32, Color.Orange);
            Walls[2] = new Wall(32, 32, 0, 32, Color.Yellow);
            Walls[3] = new Wall(0, 32, 0, 0, Color.Orange);

            Walls[4] = new Wall(64, 0, 96, 0, Color.Red);
            Walls[5] = new Wall(96, 0, 96, 32, Color.DarkRed);
            Walls[6] = new Wall(96, 32, 64,32, Color.Red);
            Walls[7] = new Wall(64, 32, 64, 0, Color.DarkRed);

            Walls[8] =  new Wall(64, 64, 96, 64, Color.Purple);
            Walls[9] =  new Wall(96, 64, 96, 96, Color.DeepPink);
            Walls[10] = new Wall(96, 96, 64, 96, Color.Purple);
            Walls[11] = new Wall(64, 96, 64, 64, Color.DeepPink);

            Walls[12] = new Wall(0, 64, 32, 64, Color.SkyBlue);
            Walls[13] = new Wall(32, 64, 32, 96, Color.Blue);
            Walls[14] = new Wall(32, 96, 0, 96, Color.SkyBlue);
            Walls[15] = new Wall(0, 96, 0, 64, Color.Blue);
            enemies.Add(new Zombie(100, 100,40));
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
        const float playerRadius = 30.0f;
       
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
                for (int a = Sectors[s].ws; a < Sectors[s].we-1; a++)
                {
                    if (Walls[a].d(Slayer.X,Slayer.Y) < Walls[a + 1].d(Slayer.X,Slayer.Y))
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
                        wy[0]=1;   
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
                    //  if (wx[0] > 0 && wx[0] < SW && wy[0] > 0 && wy[0] < SH) { g.FillRectangle(brush, wx[0], wy[0], 4, 4); }
                    //  if (wx[1] > 0 && wx[1] < SW && wy[1] > 0 && wy[1] < SH) { g.FillRectangle(brush, wx[1], wy[1], 4, 4); }
                   
                    Point[] points = new Point[4];
                    points[0] = new Point(wx[0], wy[0]);
                    points[1] = new Point(wx[2], wy[2]);
                    points[2] = new Point(wx[3], wy[3]);
                    points[3] = new Point(wx[1], wy[1]);


                    g.FillPolygon(brush, points);
                    // Render to screen


                }//crtanje zidova
                Sectors[s].d /= (Sectors[s].we - Sectors[s].ws); //prosecna sektor distanca
                foreach (var enemy in enemies)
                {
                    // Relative position
                    float dx = enemy.X - Slayer.X;
                    float dy = enemy.Y - Slayer.Y;

                    // Transform to view space
                    double ex = dx * CS - dy * SN;
                    double ey = dy * CS + dx * SN;

                    if (ey < 1) continue; // Behind player

                    // Project vertical position (like wz[i] for walls!)
                    double zWorld = enemy.Z - Slayer.Z;
                    double ez = zWorld + (Slayer.L * ey / 32.0);
                    // Screen projection
                    int screenX = (int)(ex * 200 / ey) + SW2;
                    int screenY = (int)(ez * 200 / ey) + SH2;

                    // Scale enemy height based on distance
                    int enemyHeight = (int)(enemy.Height * 200 / ey);
                    int enemyWidth = enemyHeight / 2;

                    // Now use screenY as the *bottom* of the sprite
                    Rectangle enemyRect = new Rectangle(
                        screenX - enemyWidth / 2,
                        screenY - enemyHeight, // sprite extends upward from screenY
                        enemyWidth,
                        enemyHeight
                    );

                    // Render enemy
                    Brush brush = new SolidBrush(enemy.Color);
                    g.FillRectangle(brush, enemyRect);
                }




            }

            buffer.Render();
            buffer.Dispose();  // Important!
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
            if(sector==-1)return false;
            for (int w = Sectors[sector].ws; w < Sectors[sector].we; w++)
            {
                if (Walls[w].d(Slayer.X,Slayer.Y) < playerRadius)
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

            return -1; // Player not inside any sector
        }


        void MovePlayer()
        {
            int newX=Slayer.X;
            int newY=Slayer.Y;
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
            if (CheckCollision(Slayer.X, Slayer.Y, GetPlayerSector(Slayer.X, Slayer.Y))){ Slayer.X=newX; Slayer.Y=newY;
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
    }
    public class Wall
    {
      public int x1, y1;
      public int x2, y2;
        public int d(int px,int py) { 
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
        public Wall(int x1,int y1,int x2,int y2,Color Color)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.c = Color;
        }
    }
   public class Sector
    {
       public int ws, we; //pocetni i krajnji index zidova
       public int z1, z2; //visina poda i plafona
       public int x, y; //centar sektora
       public int d; //pomeraj
        public Sector(int WallStart, int WallEnd,int FloorHeight,int CeilingHeight)
        {
            this.ws = WallStart;
            this.we = WallEnd;
            this.z1 = FloorHeight;
            this.z2 = CeilingHeight;
        }
    }
    public class K
    {
        public bool w, a, s, d, sl, sr, m;
    }
    public class Igrac
    {
       int x, y, z;//pozicija
       int a; //ugao levo desno
       int l; //ugao gore dole
       
        public Igrac(int x, int y, int z, int a, int l)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.a = a;
            this.l = l;
        }
        public int X {  get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public int A { get { return a; } set { a = value; } }
        public int L { get { return l; } set { l = value; } }
       

    }
    public abstract class Weapon
    {
        public string Name { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }
        public float Cooldown { get; set; } // seconds between shots
        public float TimeSinceLastShot { get; set; }

        public Weapon(string name, int maxAmmo, float cooldown)
        {
            Name = name;
            MaxAmmo = maxAmmo;
            Ammo = maxAmmo;
            Cooldown = cooldown;
            TimeSinceLastShot = cooldown;
        }

        public virtual void Update(float deltaTime)
        {
            if (TimeSinceLastShot < Cooldown)
            {
                TimeSinceLastShot += deltaTime;
            }
        }

        public virtual bool CanShoot()
        {
            return Ammo > 0 && TimeSinceLastShot >= Cooldown;
        }

        public virtual void Shoot()
        {
            if (!CanShoot()) return;

            TimeSinceLastShot = 0f;
            Ammo--;


        }

        public virtual void Reload()
        {
            Ammo = MaxAmmo;
        }
    }

    public abstract class Matematika
    {
        public  static double Sin(double x) { return Math.Sin(x / 180 * Math.PI); }
        public static  double Cos(double x) { return Math.Cos(x / 180 * Math.PI); }

        public static int distance(int x1,int y1,int x2,int y2)
        {
            return Convert.ToInt32(Math.Sqrt((x1 - x2) * (x1-x2)+(y1 - y2) * (y1-y2)));
        }

    }
    public abstract class EnemyBase
    {
        // Position in world space
    // Height, if needed
       public int X, Y,Z;
        public int Height;
        public Color Color;
        public int Speed { get; protected set; }
        public int Health { get; protected set; }

        public bool IsDead => Health <= 0;

        // Facing angle (radians)
        public float Angle { get; protected set; }

        // Constructor
        public EnemyBase( int speed, int health,int height,Color color)
        {
         
            Speed = speed;
            Health = health;
            Height = height;
            Color = color;
            Angle = 0f;
        }

        public virtual void Update(int playerX,int playerY, int deltaTime)
        {
           
        }

       
        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0) Health = 0;
        }

        public virtual void Draw(Graphics g, int screenX, int screenY)
        {
         
        }
    }
    public class Zombie : EnemyBase
    {
        
       public Zombie(int x,int y,int z) : base(10,100,40,Color.DarkGreen)
        {
           base.Health = base.Health; base.Speed = base.Speed;
            base.X= x; base.Y = y; base.Z = z;
           
        }
        public override void Draw(Graphics g, int playerX, int playerY)
        {
            
        }
    }

}
