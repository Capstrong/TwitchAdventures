using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Parse;

public enum VoteType
{
	UP,
	DOWN,
	LEFT,
	RIGHT,
	TIE
}

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
	public VoteType winningVote;

	private Dictionary<VoteType, int> votes;
	private bool votesDirty = false;
	private DateTime _lastTime;

	void Awake()
	{
		_lastTime = DateTime.UtcNow;
		votes = new Dictionary<VoteType,int>();
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
			upDisplay.text = "Up: " + votes[VoteType.UP];
			downDisplay.text = "Down: " + votes[VoteType.DOWN];
			winnerDisplay.text = "Winner: " + winningVote;

			voteCallbacks( this );

			votesDirty = false;
		}
	}

	public void ResetVotes()
	{
		foreach ( VoteType voteType in (VoteType[])Enum.GetValues( typeof( VoteType ) ) )
		{
			votes[voteType] = 0;
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

				winningVote = VoteType.TIE;
				int mostVotes = 0;

				IEnumerable<ParseObject> results = t.Result;
				foreach ( ParseObject vote in results )
				{
					VoteType voteType = (VoteType)Enum.Parse( typeof( VoteType ), vote.Get<String>("vote") );
					++votes[voteType];

					int voteCount = votes[voteType];
					if ( voteCount > mostVotes )
					{
						winningVote = voteType;
						mostVotes = voteCount;
					}
					else if ( voteCount == mostVotes )
					{
						winningVote = VoteType.TIE;
					}
				}

				votesDirty = true;
			} );

			_lastTime = DateTime.UtcNow;

			yield return new WaitForSeconds( interval );
		}
	}
}
