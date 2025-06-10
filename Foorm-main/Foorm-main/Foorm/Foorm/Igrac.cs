using System;

public class Igrac
{
    int x, y, z;//pozicija
    int a; //ugao levo desno
    int l; //ugao gore dole
    public int playerRadius = 12;
    public int Health = 100;
    public bool IsDead => Health <= 0;

    public Igrac(int x, int y, int z, int a, int l)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.a = a;
        this.l = l;
    }
    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }
    public int Z { get { return z; } set { z = value; } }
    public int A { get { return a; } set { a = value; } }
    public int L { get { return l; } set { l = value; } }
    public void TakeDamage(EnemyBase enemy)
    {
        if (Matematika.distance(enemy.X, enemy.Y, X, Y) < playerRadius&&enemy.AttackTimer>=enemy.AttackCooldown) { Health -= enemy.Damage; enemy.AttackTimer = 0; }
    }
    public void Update(int deltaTime)
    {
     
    }
}
