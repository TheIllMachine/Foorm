using System;

public abstract class EnemyBase
{
    // Position in world space
    // Height, if needed
    public int X, Y, Z;
    public int Height;
    public Color Color;
    public int Speed { get; protected set; }
    public int Health { get; protected set; }

    public bool IsDead => Health <= 0;

    // Facing angle (radians)
    public float Angle { get; protected set; }

    // Constructor
    public EnemyBase(int speed, int health, int height, Color color)
    {

        Speed = speed;
        Health = health;
        Height = height;
        Color = color;
        Angle = 0f;
    }

    public virtual void Update(int playerX, int playerY, int deltaTime)
    {

    }


    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

    }

    public virtual void Draw(Graphics g, int screenX, int screenY)
    {

    }
}
