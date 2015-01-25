using UnityEngine;
using System.Collections;

public class EventObject : GridObject 
{
	public GameEvent gameEvent;

	public override void Interact( Village village )
	{
		EventManager.instance.PlayEvent(gameEvent);
	}
}
