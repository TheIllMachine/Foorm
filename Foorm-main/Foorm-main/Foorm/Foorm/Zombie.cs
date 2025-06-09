using System;

public class Zombie : EnemyBase
{

    public Zombie(int x, int y, int z) : base(10, 100, 40, Color.DarkGreen)
    {

        base.X = x; base.Y = y; base.Z = z;

    }
    public override void Draw(Graphics g, Igrac Slayer)
    {
        int SW = Foorm.Form1.ActiveForm.Width;
        int SH=Foorm.Form1.ActiveForm.Height;
        if (IsDead) return;
        float dx = X - Slayer.X;
        float dy = Y - Slayer.Y;

        double CS = Matematika.Cos(Slayer.A);
        double SN = Matematika.Sin(Slayer.A);
        double ex = dx * CS - dy * SN;
        double ey = dy * CS + dx * SN;

        if (ey < 1) return;


        double zWorld = Z - Slayer.Z;
        double ez = zWorld + (Slayer.L * ey / 32.0);

        int screenX = (int)(ex * 200 / ey) + SW/2;
        int screenY = (int)(ez * 200 / ey) + SH/2;


        int enemyHeight = (int)(Height * 200 / ey);
        int enemyWidth = enemyHeight / 2;


        Rectangle enemyRect = new Rectangle(
            screenX - enemyWidth / 2,
            screenY - enemyHeight,
            enemyWidth,
            enemyHeight
        );

        // Render enemy
        Brush brush = new SolidBrush(this.Color);
        g.FillRectangle(brush, enemyRect);
    }
}
