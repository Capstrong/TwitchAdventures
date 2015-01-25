using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour
{
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();

		GridManager.RegisterGridObject( this );
	}

	void Destroy()
	{
		Debug.Log("get killed");
	}

	public int x
	{
		get
		{
			return Mathf.RoundToInt( transform.position.x );
		}

		set
		{
			Vector3 position = transform.position;
			position.x = (float)value;
			transform.position = position;
		}
	}

	public int y
	{
		get
		{
			return Mathf.RoundToInt( transform.position.y );
		}

		set
		{
			Vector3 position = transform.position;
			position.y = (float)value;
			transform.position = position;
		}
	}

	public virtual void Interact( Village village ) { }
}
