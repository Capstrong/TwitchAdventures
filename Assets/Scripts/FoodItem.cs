using UnityEngine;
using System.Collections;

public class FoodItem : GridObject
{
	public int foodValue;
	public Sprite consumedSprite;

	private bool _consumed = false;

	public override void Interact( Village village )
	{
		if ( !_consumed )
		{
			village.numFood += foodValue;
			GetComponent<SpriteRenderer>().sprite = consumedSprite;
			GridManager.DeregisterGridObject( this );
		}
	}
}
