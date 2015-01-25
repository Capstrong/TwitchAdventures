using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Village : SingletonBehaviour<Village>
{
	public List<Villager> villagers = new List<Villager>();
	public int numVillagers = 0;
	public int numFood = 0;

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
