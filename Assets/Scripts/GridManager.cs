using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridLocation
{
	public int x;
	public int y;

	public GridLocation(int inX, int inY)
	{
		x = inX;
		y = inY;
	}

	public GridLocation(Vector2 inVec)
	{
		x = (int)inVec.x;
		y = (int)inVec.y;
	}
}

public class GridManager : SingletonBehaviour<GridManager> 
{
	public List<GridLocation> gridLocations = new List<GridLocation>();

	public void RegisterGridLocation(GridLocation gridLocation)
	{
		if(!gridLocations.Contains(gridLocation))
		{
			gridLocations.Add(gridLocation);
		}
	}

	public void RegisterGridLocation(int x, int y)
	{
		GridLocation gridLocation = new GridLocation(x, y);
		if(!gridLocations.Contains(gridLocation))
		{
			gridLocations.Add(gridLocation);
		}
	}

	public bool IsGridLocationOpen(GridLocation gridLocation)
	{
		return !gridLocations.Contains(gridLocation);
	}

	public bool IsGridLocationOpen(int x, int y)
	{
		GridLocation gridLocation = new GridLocation(x, y);
		return !gridLocations.Contains(gridLocation);
	}

	public bool IsGridLocationOpen(float x, float y)
	{
		GridLocation gridLocation = new GridLocation((int)x, (int)y);
		return !gridLocations.Contains(gridLocation);
	}
}
