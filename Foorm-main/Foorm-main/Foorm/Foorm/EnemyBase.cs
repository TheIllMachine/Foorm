using System;

public abstract class EnemyBase:IDamagable
{
    // Position in world space
    // Height, if needed
    public int X, Y, Z;
    public int Height;
    public Color Color;
    public Color OriginalColor;
    public int Speed { get; protected set; }
    public int Health { get; protected set; }
    public int Damage {  get; protected set; }  
    public bool IsDead => Health <= 0;

    // Facing angle (radians)
    public float Angle { get; protected set; }
    public int Cooldown = 500;
    public int Timer = 500;
    public int AttackTimer = 500;
    public int AttackCooldown = 500;

    // Constructor
    public EnemyBase(int speed, int health, int height, Color color,int damage)
    {

        Speed = speed;
        Health = health;
        Height = height;
        Color = color;
        OriginalColor = color;
        Damage = damage;
        Angle = 0f;
    }

    public virtual void Update(int playerX, int playerY, int deltaTime)
    {
        if (Color == Color.Red && Timer < Cooldown) Timer += deltaTime;
        else if(Color==Color.Red)Color=OriginalColor;
        if(AttackTimer < AttackCooldown)AttackTimer += deltaTime;
    }
    public virtual void EnemyMove(Igrac Slayer)
    {
        double dx = X - Slayer.X;
        double dy = Y - Slayer.Y;
        double angle = Math.Atan2(dx, dy);
        X -= Convert.ToInt32(Speed * Math.Sin(angle));
        Y -= Convert.ToInt32(Speed * Math.Cos(angle));
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        Color = Color.Red;
        Timer = 0;
    }

    public virtual void Draw(Graphics g, Igrac Slayer)
    {

    }
}
