using UnityEngine;
using System.Collections;

public enum VillagerClass
{
	Peasant = 0,
	Warrior = 1,
	Royalty = 2,
	Craftsmen = 3,
	Criminal = 4,
	Elderly = 5,
	Child = 6
}

[System.Serializable]
public struct Villager 
{
	public string name;
	public VillagerClass villagerClass;
}
