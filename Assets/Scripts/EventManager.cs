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
	public VillagerClass villagerClass;
	public CompareType compareType;
	public int value;
};

[System.Serializable]
public class DynamicEvent : GameEvent
{
	public EventCondition eventCondition;

	public bool Success(Village village)
	{
		int compareValue = village.GetClassCount(eventCondition.villagerClass);

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
	Vector3 eventPanelInitPos;

	[SerializeField] Sprite eventIcon;
	[SerializeField] Text eventText;
	bool showPlayerOptions = false;

	GameEvent currentGameEvent;

	string textToRead = "";

	[SerializeField] float eventVoteTime = 10f;
	[SerializeField] float eventEndWaitTime = 3f;

	void Awake()
	{
		eventPanelInitPos = eventPanel.transform.position;
	}

	IEnumerator PlayEvent(GameEvent gameEvent)
	{
		// Play event sound
		// Wait for song to end

		// Slide Panel In
		Vector3 startPos = eventPanelInitPos;
		Vector3 endPos = eventPanelInitPos + new Vector3(0f, panelMoveDist, 0f);

		float slideTimer = 0f;
		while(slideTimer < slideTime)
		{
			eventPanel.transform.position = Vector3.Lerp(startPos, endPos, slideTimer/slideTime);
			slideTimer += Time.deltaTime;
			yield return 0;
		}

		currentGameEvent = gameEvent;

		if(gameEvent is DynamicEvent)
		{
//			DynamicEvent dynamicEvent = (DynamicEvent)gameEvent;
//			if(dynamicEvent.Success(village))
//			{
//
//			}
//			else
//			{
//
//			}
		}
		else
		{
			VoteManager.instance.StopCoroutine("MoveVote");

			VoteManager.SetLastTime();
			yield return new WaitForSeconds(eventVoteTime);

			VoteManager.instance.voteCallbacks += EventVoteCallback;
			VoteManager.QueryVotes();
		}
	}

	void EventVoteCallback(VoteManager voteManager)
	{
		if((int)voteManager.winningVote < 4)
		{
			Debug.LogError("Direction received instead of answer!");
		}
		else if(voteManager.winningVote == VoteResponse.Yes)
		{
			eventText.text = currentGameEvent.yesResult.text;
			Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
			Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
		}
		else if(voteManager.winningVote == VoteResponse.No)
		{
			eventText.text = currentGameEvent.noResult.text;
			Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
			Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
		}
		else if(currentGameEvent is ChoiceEvent && 
		        voteManager.winningVote == VoteResponse.Tie)
		{
			ChoiceEvent choiceEvent = (ChoiceEvent)currentGameEvent;
			eventText.text = choiceEvent.tieResult.text;

			Village.instance.AddVillagers(choiceEvent.tieResult.peopleChange);
			Village.instance.AddFood(choiceEvent.tieResult.foodChange);
		}
		
		StartCoroutine(FinishEvent());
	}

	IEnumerator FinishEvent()
	{
		yield return new WaitForSeconds(eventEndWaitTime);
		
		VoteManager.instance.StartCoroutine("MoveVote");
		
		// Slide Panel Out
		Vector3 startPos = eventPanelInitPos + new Vector3(0f, panelMoveDist, 0f);
		Vector3 endPos = eventPanelInitPos;
		
		float slideTimer = 0f;
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
