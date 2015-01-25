using UnityEngine;
using System.Collections;

public class Oni : GridObject
{
	public int damage;
	public int health;

	public override void Interact( Village village )
	{
		village.numVillagers -= damage;
		health -= village.numVillagers;

		if ( health < 0 )
		{
			GridManager.DeregisterGridObject( this );
			Destroy( gameObject );
		}
	}
}
