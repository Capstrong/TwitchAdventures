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
	//public VillagerClass villagerClass;
	public CompareType compareType;
	public int value;
};

public class EventManager : SingletonBehaviour<EventManager> 
{
	string[] firstNames = new string[]{ 	"George", "Shingen", "Huriyama", "Hanzo", "Jiro", "Mufasa", "Hideo", 
										"Kenji", "Ryo", "Takashi", "Akane", "Chiyo", "Emiko", "Izumi",
										"Kame", "Masume", "Rika", "Jerry", "Percival", "Terrence", "Ophelia", 
										"Masamune", "Cliff", "Chazz", "Felix", "Juliet", "Mariah", "Jose", "Seymour",
										"Becky", "Hugo", "Hans", "Geronimo", "Doctor", "Obviously", "Lucius", 
										"Hercules", "Captain", "Moby", "Vin", "The Big", "MC", "Teddy", "Eugene", 
										"Shea", "Pavel", "Buddy", "Abraham", "Methuselah", "Nebuchadnezzar", 
										"Quagguiníbush", "Hannibal", "Vuvuzela", "Euripedes", "Ricky", "Dick", 
										"Chi Chi", "Wendy", "Olga", "Myriam", "Stonecold", "Skip", "Chris", 
										"David", "Will", "Wyatt", "Jake" };
	
	string[] lastNames = new string[]{ 	"Jake-San", "Takei", "Nintendoug", "Godfreyzilla", "Hibachi", "Arcade-Master", 
										"Matada", "Thousand-Fists", "Kojima", "Mothra", "Johnson-san", "Jellyfish", "Incognito", 
										"Power", "Lionheart", "Furioso", "But-With-A-Silent-A", "Franc-With-a-C", "Esquire", "Wu", 
										"McSamurai", "Chaos", "-San", "-Sama", "Senpai", "Dover", "Steele", "Paintrain", "Goldberg",
										"Snapdragon", "Tebow", "Strickland", " Lechuga", "Relampagos", "Evangelo", "Puddiní", 
										"Frankenstein", "Pants", "The Mildly Unpleasant", "Tuesday", "Sideswipe", "Prime", "Tron",
										"Weber", "Datsyuk", "Balboa", "Fury", "Duper", "Hightower", "Suzuki", "Nomo", "The Cat",
										"Conehead", "Poppins", "Wade", " Legare", "Skipper", "Goliath", "Zappa", "Danger", "Armstrong",
										"Garcia", "Cain", "Zink", "Shenanigans", "Hulkbuster" };
	
	[SerializeField] GameEvent[] gameEvents;
	public Dictionary<string, GameEvent> eventMap = new Dictionary<string, GameEvent>();
	
	[SerializeField] Transform eventPanel;
	[SerializeField] float panelMoveDist = 1f;
	[SerializeField] float slideTime = 0.3f;
	[SerializeField] Slider voteSlider; 
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
			PlayEvent(gameEvents[Random.Range(0, gameEvents.Length - 1)]);
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

		AudioSource source = SoundManager.instance.Play2DSong("EventPopUp");
		yield return new WaitForSeconds(source.clip.length - 3f);

		currentGameEvent = gameEvent;

		eventImage.sprite = currentGameEvent.sprite;

		eventText.text = currentGameEvent.text;
		while(eventText.text.Contains("[p]"))
		{
			eventText.text = eventText.text.Replace("[p]", 	firstNames[Random.Range(0, firstNames.Length - 1)] + " " +
			                       							lastNames[Random.Range(0, lastNames.Length - 1)]);
		}

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
			voteSlider.enabled = false;
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
			voteSlider.enabled = true;
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
				SoundManager.instance.Play2DSong("HoorayChildren");

				eventText.text = currentGameEvent.yesResult.text;
				while(eventText.text.Contains("[p]"))
				{
					eventText.text = eventText.text.Replace("[p]", 	firstNames[Random.Range(0, firstNames.Length - 1)] + " " +
						                       						lastNames[Random.Range(0, lastNames.Length - 1)]);
				}

				Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
				Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
			}
			else if(voteManager.winningVote == VoteResponse.No)
			{
				SoundManager.instance.Play2DSong("KidsBooing");

				eventText.text = currentGameEvent.noResult.text;
				while(eventText.text.Contains("[p]"))
				{
					eventText.text = eventText.text.Replace("[p]", 	firstNames[Random.Range(0, firstNames.Length - 1)] + " " +
				                      				 				lastNames[Random.Range(0, lastNames.Length - 1)]);
				}

				Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
				Village.instance.AddVillagers(currentGameEvent.noResult.peopleChange);
			}
			else if(currentGameEvent is ChoiceEvent && 
			        voteManager.winningVote == VoteResponse.Tie)
			{
				ChoiceEvent choiceEvent = (ChoiceEvent)currentGameEvent;

				eventText.text = choiceEvent.tieResult.text;
				while(eventText.text.Contains("[p]"))
				{
					eventText.text = eventText.text.Replace("[p]", 	firstNames[Random.Range(0, firstNames.Length - 1)] + " " +
				                      								lastNames[Random.Range(0, lastNames.Length - 1)]);
				}

				Village.instance.AddVillagers(choiceEvent.tieResult.peopleChange);
				Village.instance.AddFood(choiceEvent.tieResult.foodChange);
			}

			voteSlider.value = voteManager.noVotePercentage;
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
