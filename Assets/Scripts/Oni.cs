using UnityEngine;
using System.Collections;

public class Oni : GridObject
{
	[SerializeField] AudioClip[] clips;
	public int damage;
	public int health;

	public override void Interact( Village village )
	{
		village.villagerCount -= damage;
		health -= village.villagerCount;

		village.PlayFightSound();

		if ( health <= 0 )
		{
			SoundManager.instance.Play2DSong("Victory");
			GridManager.DeregisterGridObject( this );
			Destroy( gameObject );
		}
		else if(clips.Length > 0)
		{
			SoundManager.instance.Play2DSong(clips[Random.Range(0, clips.Length - 1)].name);
		}
	}
}
