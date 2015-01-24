using UnityEngine;
using System.Collections;

public enum MoveDirection
{
	North,
	East,
	South,
	West,
	Tie
}

public class Avatar : MonoBehaviour 
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
		Vector2 pos = Vector2.zero;

		switch( voteManager.winningVote )
		{
		case MoveDirection.North:
			pos.y++;
			break;
		case MoveDirection.East:
			pos.x++;
			break;
		case MoveDirection.South:
			pos.y--;
			break;
		case MoveDirection.West:
			pos.x--;
			break;
		}

		if(GridManager.instance.IsGridLocationOpen(pos.x, pos.y))
		{
			transform.position += (Vector3)pos;
		}
		else
		{
			// Shake chores
		}
	}
}
