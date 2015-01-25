using UnityEngine;
using System.Collections;

public class Oni : GridObject
{
	public int damage;
	public int health;

	public override void Interact( Village village )
	{
		village.villagers -= damage;
		health -= village.villagers;

		if ( health <= 0 )
		{
			GridManager.DeregisterGridObject( this );
			Destroy( gameObject );
		}
	}
}
