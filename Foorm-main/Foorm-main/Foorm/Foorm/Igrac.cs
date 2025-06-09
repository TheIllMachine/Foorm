using System;

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
    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }
    public int Z { get { return z; } set { z = value; } }
    public int A { get { return a; } set { a = value; } }
    public int L { get { return l; } set { l = value; } }


}
