using System;

public class Sector
{
    public int ws, we; //pocetni i krajnji index zidova
    public int z1, z2; //visina poda i plafona
    public int x, y; //centar sektora
    public int d; //pomeraj
    public Sector(int WallStart, int WallEnd, int FloorHeight, int CeilingHeight)
    {
        this.ws = WallStart;
        this.we = WallEnd;
        this.z1 = FloorHeight;
        this.z2 = CeilingHeight;
    }
}


