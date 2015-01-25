using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum CompareType
{
	Greater 		= 0,
	GreaterEqual 	= 1,
	Less 			= 2,
	LessEqual 		= 3,
	Equal		 	= 4
}

public struct EventResult
{
	public string text;
	public int foodChange;
	public int peopleChange;
};

[System.Serializable]
public class GameEvent : ScriptableObject
{
	public string name;
	public Sprite sprite;
	public string text;
	public EventResult yesResult;
	public EventResult noResult;
};

[System.Serializable]
public class ChoiceEvent : GameEvent
{
	public EventResult tieResult;

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Events/Choice Event")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ChoiceEvent> ();
	}
	#endif
};

public struct EventCondition
{
	public string tag;
	public CompareType compareType;
	public int value;
};

[System.Serializable]
public class DynamicEvent : GameEvent
{
	public EventCondition eventCondition;

	public bool Success(int compareValue)
	{
		switch(eventCondition.compareType)
		{
		case CompareType.Equal:
			return eventCondition.value == compareValue;
		case CompareType.Greater:
			return eventCondition.value > compareValue;
		case CompareType.GreaterEqual:
			return eventCondition.value >= compareValue;
		case CompareType.Less:
			return eventCondition.value < compareValue;
		case CompareType.LessEqual:
			return eventCondition.value <= compareValue;
		default:
			return false;
		}
	}

	#if UNITY_EDITOR
	[MenuItem("Assets/Create/Events/Dynamic Event")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<DynamicEvent> ();
	}
	#endif
};

public class EventManager : MonoBehaviour 
{
	[SerializeField] GameEvent[] gameEvents;
	public Dictionary<string, GameEvent> eventMap = new Dictionary<string, GameEvent>();

	[SerializeField] Image eventPanel;
	[SerializeField] float panelMoveDist = 1f;
	[SerializeField] float slideTime = 0.3f;

	[SerializeField] Sprite eventIcon;
	[SerializeField] Text eventText;
	bool showPlayerOptions = false;

	string textToRead = "";

	void Awake()
	{
	}

	IEnumerator PlayEvent(GameEvent gameEvent)
	{
		// Play event sound
		// Wait for song to end

		// Slide Panel In
		Vector3 startPos = eventPanel.transform.position;
		Vector3 endPos = eventPanel.transform.position + new Vector3(0f, panelMoveDist, 0f);

		float slideTimer = 0f;
		while(slideTimer < slideTime)
		{
			eventPanel.transform.position = Vector3.Lerp(startPos, endPos, slideTimer/slideTime);
			slideTimer += Time.deltaTime;
			yield return 0;
		}

		if(gameEvent is DynamicEvent)
		{
			DynamicEvent dynamicEvent = (DynamicEvent)gameEvent;
			//if(dynamicEvent.Success())
		}
		else
		{

		}

		// Slide Panel Out
		startPos = eventPanel.transform.position;
		endPos = eventPanel.transform.position - new Vector3(0f, panelMoveDist, 0f);

		slideTimer = 0f;
		while(slideTimer < slideTime)
		{
			eventPanel.transform.position = Vector3.Lerp(startPos, endPos, slideTimer/slideTime);
			slideTimer += Time.deltaTime;
			yield return 0;
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
