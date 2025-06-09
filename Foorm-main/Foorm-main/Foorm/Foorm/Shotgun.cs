using System;

public class Shotgun : Weapon
{ 
    public Image Image { get; set; }
    
    public Shotgun() : base("Shotgun", 12, 50, 50)
    {
        Image = Foorm.Properties.Resources.Shotgun_Fixed;
    }
}
