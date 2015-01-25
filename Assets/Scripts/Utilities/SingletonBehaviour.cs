// This class turs writing singletons into 2 lines of code instead of a dozen
// I did not write it - Chris

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Make a subclass of this class with T as the subclass to make a singleton
public class SingletonBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
	public static T instance;

	public void Awake()
	{
		DontDestroyElseKill(this);
	}
	
	// Call this to upgrade a singleton to a persistent singleton.
	// This will kill an instance that tries to be a persistent singleton but isn't the current instance.
	public static void DontDestroyElseKill( MonoBehaviour mb )
	{
		if ( instance == null )
		{
			MonoBehaviour.DontDestroyOnLoad( mb.gameObject );
			instance = (T)mb;
		}
		else
		{
			MonoBehaviour.Destroy( mb );
		}
	}
}