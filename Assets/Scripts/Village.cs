using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Village : MonoBehaviour
{
	[SerializeField]
	private int _villagers;

	public int villagers
	{
		get
		{
			return _villagers;
		}

		set
		{
			_villagers = value;
			villagersDisplay.text = "Villagers: " + _villagers;
		}
	}

	public Text villagersDisplay;

	void Start()
	{
		villagersDisplay.text = "Villagers: " + _villagers;
	}
}
