using UnityEngine;
using System.Collections;

public enum MoveDirection
{
	North,
	East,
	South,
	West,
	Yes,
	No,
	Tie
}

public class Avatar : GridObject
{
	public VoteManager voteManager;

	[SerializeField] float inputTime = 0.2f;
	float inputTimer = 0f;

	[SerializeField] Camera cam;
	[SerializeField] float camFollowSpeed = 5f;

	void Start()
	{
		voteManager.voteCallbacks += MovePlayer;
	}

	//void Update()
	//{
	//	Movement();
	//}

	void FixedUpdate()
	{
		CameraControl();
	}

	void CameraControl()
	{
		Vector3 targetPos = transform.position;
		targetPos.z = cam.transform.position.z;

		cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, 
		                                      Time.deltaTime * camFollowSpeed);
	}

	//void Movement()
	//{
	//	float hInput = Input.GetAxis("Horizontal");
	//	float vInput = Input.GetAxis("Vertical");
		
	//	if(Mathf.Abs(hInput) > WadeUtils.SMALLNUMBER)
	//	{
	//		if(inputTimer > inputTime)
	//		{
	//			MovePlayer(hInput > 0f ? MoveDirection.East : MoveDirection.West);
	//			inputTimer = 0f;
	//		}
	//	}
	//	else if(Mathf.Abs(vInput) > WadeUtils.SMALLNUMBER)
	//	{
	//		if(inputTimer > inputTime)
	//		{
	//			MovePlayer(vInput > 0f ? MoveDirection.North : MoveDirection.South);
	//			inputTimer = 0f;
	//		}
	//	}
		
	//	inputTimer += Time.deltaTime;
	//}

	public void MovePlayer( VoteManager voteManager )
	{
		int newX = x;
		int newY = y;

		switch( voteManager.winningVote )
		{
		case MoveDirection.North:
			newY++;
			break;
		case MoveDirection.East:
			newX++;
			break;
		case MoveDirection.South:
			newY--;
			break;
		case MoveDirection.West:
			newX--;
			break;
		case MoveDirection.Tie:
			return;
		}

		if( GridManager.IsGridLocationOpen( newX, newY ) )
		{
			Vector3 newPos = transform.position;
			newPos.x = (float)newX;
			newPos.y = (float)newY;
			transform.position = newPos;
		}
		else
		{
			GridObject gridObject = GridManager.Get( newX, newY );
			gridObject.Interact( GetComponent<Village>() );
		}
	}
}
