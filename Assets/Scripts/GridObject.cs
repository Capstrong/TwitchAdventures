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
			return (int)( transform.position.x + 0.1f );
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
			return (int)( transform.position.y + 0.1f );
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
