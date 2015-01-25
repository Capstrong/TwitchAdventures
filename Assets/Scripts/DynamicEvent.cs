using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

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