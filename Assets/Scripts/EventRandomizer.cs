using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventRandomizer : MonoBehaviour 
{
	[SerializeField] EventObject[] eventObjects;
	[SerializeField] GameEvent[] gameEvents;

	public void Start()
	{
		List<GameEvent> curGameEvents = new List<GameEvent>();
		foreach(GameEvent gameEvent in gameEvents)
		{
			curGameEvents.Add(gameEvent);
		}

		foreach(EventObject eventObject in eventObjects)
		{
			eventObject.gameEvent = curGameEvents[Random.Range(0, curGameEvents.Count - 1)];
			curGameEvents.Remove(eventObject.gameEvent);
		}
	}
}
