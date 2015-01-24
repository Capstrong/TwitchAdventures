using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class VoteManager : SingletonBehaviour<VoteManager>
{
	public delegate void VoteCallback( VoteManager voteManager );

	public VoteCallback voteCallbacks = delegate( VoteManager voteManager ) { };

	[Tooltip( "Interval between queries in seconds." )]
	public float interval;

	public Text upDisplay;
	public Text downDisplay;
	public Text winnerDisplay;

	[HideInInspector]
	public MoveDirection winningVote;

	private Dictionary<MoveDirection, int> votes;
	private bool votesDirty = false;
	private DateTime _lastTime;

	void Awake()
	{
		_lastTime = DateTime.UtcNow;
		votes = new Dictionary<MoveDirection,int>();
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
			upDisplay.text = "North: " + votes[MoveDirection.North];
			downDisplay.text = "South: " + votes[MoveDirection.South];
			winnerDisplay.text = "Winner: " + winningVote;

			voteCallbacks( this );

			votesDirty = false;
		}
	}

	public void ResetVotes()
	{
		foreach ( MoveDirection MoveDirection in (MoveDirection[])Enum.GetValues( typeof( MoveDirection ) ) )
		{
			votes[MoveDirection] = 0;
		}
	}

	public IEnumerator QueryVotes()
	{
		while ( true )
		{
			ParseQuery<ParseObject> query = ParseObject.GetQuery( "Vote" )
				.WhereGreaterThan( "createdAt", _lastTime );
			query.FindAsync().ContinueWith( t =>
			{
				ResetVotes();

				winningVote = MoveDirection.Tie;
				int mostVotes = 0;

				IEnumerable<ParseObject> results = t.Result;
				foreach ( ParseObject vote in results )
				{
					MoveDirection voteType = (MoveDirection)Enum.Parse( typeof( MoveDirection ), vote.Get<String>("vote") );
					++votes[voteType];

					int voteCount = votes[voteType];
					if ( voteCount > mostVotes )
					{
						winningVote = voteType;
						mostVotes = voteCount;
					}
					else if ( voteCount == mostVotes )
					{
						winningVote = MoveDirection.Tie;
					}
				}

				votesDirty = true;
			} );

			_lastTime = DateTime.UtcNow;

			yield return new WaitForSeconds( interval );
		}
	}
}
