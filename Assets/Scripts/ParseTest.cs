using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class ParseTest : MonoBehaviour
{
	[Tooltip( "Interval between queries in seconds." )]
	public float interval;

	private DateTime _lastTime;

	void Awake()
	{
		_lastTime = DateTime.UtcNow;
		Debug.Log( "Now: " + _lastTime );
	}

	void Start()
	{
		StartCoroutine( QueryVotes() );
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
				IEnumerable<ParseObject> results = t.Result;
				foreach ( ParseObject vote in results )
				{
					Debug.Log( vote );
					Debug.Log( vote["vote"] );
				}
			} );

			_lastTime = DateTime.UtcNow;

			yield return new WaitForSeconds( interval );
		}
	}
}
