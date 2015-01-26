using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Village : SingletonBehaviour<Village>
{
	public List<Villager> villagers = new List<Villager>();
	public int numFood = 0;

	private int starvationDecay = 0;

	[SerializeField] AudioClip[] fightSounds;

	[SerializeField]
	private int _villagerCount;
	public int villagerCount
	{
		get
		{
			return _villagerCount;
		}
		
		set
		{
			_villagerCount = value;
			villagersDisplay.text = _villagerCount.ToString();
			if(_villagerCount <= 0)
			{
				_villagerCount = 0;
				EventManager.instance.EndGame(numFood * 50 + (int)Time.timeSinceLevelLoad * 200);
			}
		}
	}
	
	public Text villagersDisplay;
	public Text foodDisplay;
	
	void Start()
	{
		villagersDisplay.text = _villagerCount.ToString();
		foodDisplay.text = numFood.ToString ();
	}

	public static void ConsumeFood()
	{
		instance._ConsumeFood();
	}

	private void _ConsumeFood()
	{
		if ( numFood > 0 )
		{
			numFood -= villagerCount;
			starvationDecay = 0;
		}
		else
		{
			villagerCount -= ++starvationDecay;
		}

		foodDisplay.text = numFood.ToString();
	}

//	public int GetClassCount(VillagerClass villagerClass)
//	{
//		int num = 0;
//		foreach(Villager villager in villagers)
//		{
//			if(villager.villagerClass == villagerClass)
//			{
//				num++;
//			}
//		}
//		
//		return num;
//	}

	public void AddVillagers(int num)
	{
		// Do number popup
		villagerCount += num;

		if(villagerCount <= 0)
		{
			villagerCount = 0;
			EventManager.instance.EndGame(numFood * 50 + (int)Time.timeSinceLevelLoad * 200);
		}
	}

	public void AddFood(int num)
	{
		// Do number popup
		numFood += num;
	}

	public void AddHealthBasedOnFood()
	{
		villagerCount += numFood/700;
	}

	public void PlayFightSound()
	{
		if(fightSounds.Length > 0)
		{
			SoundManager.instance.Play2DSong(fightSounds[Random.Range(0, fightSounds.Length - 1)].name);
		}
	}
}
