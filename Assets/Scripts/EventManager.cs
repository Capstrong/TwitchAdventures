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

[System.Serializable]
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
public struct EventCondition
{
	public VillagerClass villagerClass;
	public CompareType compareType;
	public int value;
};

public class EventManager : SingletonBehaviour<EventManager> 
{
	[SerializeField] GameEvent[] gameEvents;
	public Dictionary<string, GameEvent> eventMap = new Dictionary<string, GameEvent>();

	[SerializeField] Transform eventPanel;
	[SerializeField] float panelMoveDist = 1f;
	[SerializeField] float slideTime = 0.3f;
	Vector3 eventPanelInitPos;

	[SerializeField] Image eventImage;
	[SerializeField] Text eventText;
	//bool showPlayerOptions = false;

	GameEvent currentGameEvent;

	//string textToRead = "";

	[SerializeField] float eventVoteTime = 10f;
	[SerializeField] float eventEndWaitTime = 3f;

	void Start()
	{
		eventPanelInitPos = eventPanel.transform.position;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		if(Input.GetKeyDown(KeyCode.E))
		{
			StopAllCoroutines();
			PlayEvent(gameEvents[0]);
		}
	}

	public void PlayEvent(GameEvent gameEvent)
	{
		if(!currentGameEvent)
		{
			eventImage.sprite = gameEvent.sprite;
			StartCoroutine(PlayEventRoutine(gameEvent));
		}
	}

	IEnumerator PlayEventRoutine(GameEvent gameEvent)
	{
		Debug.Log("Do Event");

		currentGameEvent = gameEvent;

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
		eventPanel.transform.position = endPos;

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
			Debug.Log("Choice Event");
			VoteManager.instance.StopCoroutine("MoveVote");

			VoteManager.SetLastTime();
			yield return new WaitForSeconds(eventVoteTime);

			VoteManager.instance.voteCallbacks = EventVoteCallback;
			VoteManager.QueryVotes(true);
		}
	}

	void EventVoteCallback(VoteManager voteManager)
	{
		if(currentGameEvent)
		{
			Debug.Log(voteManager.winningVote);

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
	}

	IEnumerator FinishEvent()
	{
		if(currentGameEvent)
		{
			Debug.Log("End event");

			yield return new WaitForSeconds(eventEndWaitTime);

			VoteManager.instance.StopAllCoroutines();
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
			eventPanel.transform.position = endPos;

			currentGameEvent = null;
			VoteManager.instance.voteCallbacks = null;
			VoteManager.instance.voteCallbacks = VoteManager.instance.avatar.MovePlayer;

			StopAllCoroutines();
		}
	}
}
