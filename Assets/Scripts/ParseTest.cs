using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class ParseTest : MonoBehaviour
{
	[Tooltip( "Interval between queries in seconds." )]
	public float interval;

	public Text upDisplay;
	public Text downDisplay;

	public String[] voteTypes;

	public Dictionary<String, int> votes;


	private bool votesDirty = false;

	private DateTime _lastTime;

	void Awake()
	{
		_lastTime = DateTime.UtcNow;
		votes = new Dictionary<string,int>();
		ResetVotes();
	}

	void Start()
	{
		StartCoroutine( QueryVotes() );
	}

	void Update()
	{
		if ( votesDirty )
		{
			upDisplay.text = "Up: " + votes["UP"];
			downDisplay.text = "Down: " + votes["DOWN"];
			
			votesDirty = false;
		}
	}

	public void ResetVotes()
	{
		foreach ( String voteType in voteTypes )
		{
			votes[voteType] = 0;
		}
	}

	public IEnumerator QueryVotes()
	{
		while ( true )
		{
			Debug.Log( "Querying Parse..." );

			ParseQuery<ParseObject> query = ParseObject.GetQuery( "Vote" )
				.WhereGreaterThan( "createdAt", _lastTime );
			query.FindAsync().ContinueWith( t =>
			{
				ResetVotes();

				IEnumerable<ParseObject> results = t.Result;
				foreach ( ParseObject vote in results )
				{
					++votes[vote.Get<String>("vote")];
				}

				votesDirty = true;
			} );

			_lastTime = DateTime.UtcNow;

			yield return new WaitForSeconds( interval );
		}
	}
}
