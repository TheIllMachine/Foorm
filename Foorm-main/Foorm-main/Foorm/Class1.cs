using System;

public abstract class Weapon
{
    public string Name { get; set; }
    public int Ammo { get; set; }
    public int MaxAmmo { get; set; }
    public float Cooldown { get; set; } // seconds between shots
    public float TimeSinceLastShot { get; set; }
    public int Damage { get; set; }

    public Weapon(string name, int maxAmmo, float cooldown, int damage)
    {
        Name = name;
        MaxAmmo = maxAmmo;
        Ammo = maxAmmo;
        Cooldown = cooldown;
        TimeSinceLastShot = cooldown;
        Damage = damage;
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
public class Shotgun : Weapon
{
    public Shotgun() : base("Shotgun", 12, 50, 50)
    {

    }
}