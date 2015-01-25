using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : SingletonBehaviour<GridManager> 
{
	private List<GridObject> gridObjects = new List<GridObject>();

	public static void RegisterGridObject( GridObject gridObject )
	{
		if( !instance.gridObjects.Contains( gridObject ) )
		{
			instance.gridObjects.Add( gridObject );
		}
	}

	public static void DeregisterGridObject( GridObject gridObject )
	{
		instance.gridObjects.Remove( gridObject );
	}

	public static bool IsGridLocationOpen( GridObject gridObject )
	{
		return !instance.gridObjects.Contains( gridObject );
	}

	public static bool IsGridLocationOpen( int x, int y )
	{
		return instance.gridObjects.Find( gridObject => gridObject.x == x && gridObject.y == y ) == null;
	}

	public static GridObject Get( int x, int y )
	{
		return instance.gridObjects.Find( gridObject => gridObject.x == x && gridObject.y == y );
	}
}
