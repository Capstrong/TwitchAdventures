using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class VoteManager : MonoBehaviour
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

	private Dictionary<MoveDirection, int> _votes;
	private HashSet<String> _countedVotes = new HashSet<string>();
	private bool _votesDirty = false;
	private DateTime _lastTime;

	void Awake()
	{
		_lastTime = DateTime.UtcNow;
		_votes = new Dictionary<MoveDirection,int>();
		ResetVotes();
	}

	void Start()
	{
		StartCoroutine( QueryVotes() );
	}

	void Update()
	{
		if ( _votesDirty )
		{
			upDisplay.text = "North: " + _votes[MoveDirection.North];
			downDisplay.text = "South: " + _votes[MoveDirection.South];
			winnerDisplay.text = "Winner: " + winningVote;

			voteCallbacks( this );

			_votesDirty = false;
		}
	}

	public void ResetVotes()
	{
		foreach ( MoveDirection MoveDirection in (MoveDirection[])Enum.GetValues( typeof( MoveDirection ) ) )
		{
			_votes[MoveDirection] = 0;
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
					if ( !_countedVotes.Contains( vote.ObjectId ) )
					{
						_countedVotes.Add( vote.ObjectId );

						MoveDirection voteType = (MoveDirection)Enum.Parse( typeof( MoveDirection ), vote.Get<String>("vote") );
						++_votes[voteType];

						int voteCount = _votes[voteType];
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
				}

				_votesDirty = true;
			} );

			_lastTime = DateTime.UtcNow;

			yield return new WaitForSeconds( interval );
		}
	}
}
