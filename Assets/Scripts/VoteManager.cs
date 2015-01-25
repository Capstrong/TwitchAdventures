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

	public Avatar avatar;

	[Tooltip( "Interval between queries in seconds." )]
	public float interval;

	public Text upDisplay;
	public Text downDisplay;
	public Text eastDisplay;
	public Text westDisplay;
	public Text yesDisplay;
	public Text noDisplay;
	public Text winnerDisplay;

	[HideInInspector]
	public VoteResponse winningVote;

	private Dictionary<VoteResponse, int> _votes;
	private HashSet<String> _countedVotes = new HashSet<string>();
	private bool _votesDirty = false;
	private DateTime _lastTime;

	public float noVotePercentage
	{
		get 
		{
			if(_votes[VoteResponse.Yes] + _votes[VoteResponse.No] == 0)
			{
				return 0.5f;
			}
			else
			{
				return (float)_votes[VoteResponse.No]/(float)(_votes[VoteResponse.Yes] + _votes[VoteResponse.No]);
			}
		}
	}

	new void Awake()
	{
		base.Awake();

		_lastTime = DateTime.UtcNow;
		_votes = new Dictionary<VoteResponse,int>();
		ResetVotes();
	}

	void Start()
	{
		StartCoroutine("MoveVote");
	}

	void Update()
	{
		if ( _votesDirty )
		{
			upDisplay.text = "North: " + _votes[VoteResponse.North];
			downDisplay.text = "South: " + _votes[VoteResponse.South];
			eastDisplay.text = "East: " + _votes[VoteResponse.East];
			westDisplay.text = "West: " + _votes[VoteResponse.West];
			winnerDisplay.text = "Winner: " + winningVote;

			voteCallbacks( this );

			_votesDirty = false;
		}
	}

	public void ResetVotes()
	{
		foreach ( VoteResponse voteResponse in (VoteResponse[])Enum.GetValues( typeof( VoteResponse ) ) )
		{
			_votes[voteResponse] = 0;
		}
	}

	public IEnumerator MoveVote()
	{
		while ( true )
		{
			QueryVotes(false);
			SetLastTime();

			yield return new WaitForSeconds( interval );
		}
	}

	public static void SetLastTime()
	{
		instance._lastTime = DateTime.UtcNow;
	}

	public static void QueryVotes(bool eventChoice)
	{
		ParseQuery<ParseObject> query = ParseObject.GetQuery( "Vote" )
			.WhereGreaterThan( "createdAt", instance._lastTime );
		query.FindAsync().ContinueWith( t =>
		{
			instance.ResetVotes();
			
			instance.winningVote = VoteResponse.Tie;
			int mostVotes = 0;
			
			IEnumerable<ParseObject> results = t.Result;
			foreach ( ParseObject vote in results )
			{
				if ( !instance._countedVotes.Contains( vote.ObjectId ) )
				{
					instance._countedVotes.Add( vote.ObjectId );
					
					VoteResponse voteType = (VoteResponse)Enum.Parse( typeof( VoteResponse ), vote.Get<String>("vote") );

					if((eventChoice && (voteType != VoteResponse.Yes || voteType != VoteResponse.No)) ||
					   (!eventChoice && (voteType != VoteResponse.No && voteType != VoteResponse.Yes)))
					{
						++instance._votes[voteType];
						
						int voteCount = instance._votes[voteType];
						if ( voteCount > mostVotes )
						{
							instance.winningVote = voteType;
							mostVotes = voteCount;
						}
						else if ( voteCount == mostVotes )
						{
							instance.winningVote = VoteResponse.Tie;
						}
					}
				}
			}

			//Debug.Log(instance.winningVote);
			instance._votesDirty = true;
		} );
	}
}
