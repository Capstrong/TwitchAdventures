using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Village : SingletonBehaviour<Village>
{
	public List<Villager> villagers = new List<Villager>();
	public int numFood = 0;

	private int starvationDecay = 0;

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
			villagersDisplay.text = "Villagers: " + _villagerCount;
		}
	}
	
	public Text villagersDisplay;
	public Text foodDisplay;
	
	void Start()
	{
		villagersDisplay.text = "Villagers: " + _villagerCount;
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

		foodDisplay.text = "Food: " + numFood;
	}

	public int GetClassCount(VillagerClass villagerClass)
	{
		int num = 0;
		foreach(Villager villager in villagers)
		{
			if(villager.villagerClass == villagerClass)
			{
				num++;
			}
		}
		
		return num;
	}

	public void AddVillagers(int num)
	{
		// Do number popup
		villagerCount += num;
	}

	public void AddFood(int num)
	{
		// Do number popup
		numFood += num;
	}
}
