using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour
{
	public int x;
	public int y;

	public virtual void Interact( Village village ) { }
}
