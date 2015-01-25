using UnityEngine;
using System.Collections;

public enum VillagerClass
{
	Peasant = 0,
	Warrior = 1,
	Royalty = 2,
	Craftsmen = 3,
	Criminal = 4
}

[System.Serializable]
public struct Villager 
{
	VillagerClass villagerClass;
	string name;
	bool female;
	int age;
}
