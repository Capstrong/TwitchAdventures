using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
