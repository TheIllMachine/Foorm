using System;

	public abstract class Consumable
	{
		int x, y, z;
		bool pickedUp;
		int potency;
		int Potency { get; }
		public virtual void PickUp(Igrac Slayer) { pickedUp = true; };
		public bool PickedUp { get; set; }
		public Consumable(int Potency)
		{
			potency = Potency;
		}
	}
