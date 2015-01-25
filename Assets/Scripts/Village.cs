using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Village : SingletonBehaviour<Village>
{
	public List<Villager> villagers = new List<Villager>();
	public int numVillagers = 0;
	public int numFood = 0;

	[SerializeField]
	private int _villagers;
	
	public int villagerCount
	{
		get
		{
			return _villagers;
		}
		
		set
		{
			_villagers = value;
			villagersDisplay.text = "Villagers: " + _villagers;
		}
	}
	
	public Text villagersDisplay;
	
	void Start()
	{
		villagersDisplay.text = "Villagers: " + _villagers;
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
		numVillagers += num;
	}

	public void AddFood(int num)
	{
		// Do number popup
		numFood += num;
	}
}
