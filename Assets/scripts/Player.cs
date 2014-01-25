using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
[RequireComponent (typeof (CharacterController))]
public class Player : MonoBehaviour {
	public GameModel.Characters playerId;
	
	private CharacterController controller;
	
	private float gravity = 10;
	private float speed = 5;
	private float jumpSpeed = 5;
	private float jumpVelocity = 0;
	private float waterfloating = 8;
	
	private float lastSync = 0.0f;
	private Vector3 lastSyncedPos;
	private float maxLastSync =  0.018f;
	private float lastMessage;
	
	private float ipDeltaTime = 0.1f;
	
	private Vector3 cameraOffset = new Vector3(-10, 10, -10);
	private float cameraFollow =  10;
	private float cameraFollowJump =  5;
	private Vector3 cameraRot = new Vector3(35,40,0);

	private List<Vector3> positions = new List<Vector3>();
	private List<float> times = new List<float>();

	private bool inWater;

	private bool checkSlope = true;

	public bool isInWater {
		set {
			this.inWater = value;
		}
		get {
			return this.inWater;
		}
	}
	
	void Start () {
		controller = GetComponent<CharacterController>();
		lastSyncedPos = transform.position;

		GameModel.lastCheckpoint = transform.position;
	}
	
	void Update () {
		if (!Network.isServer && !Network.isClient){
			// delete me!
			return;
		}
		
		if (GameModel.PlayerId == this.playerId) { // move our player
			Vector3 moveDirection = Vector3.zero;
			// save old position!
			checkSlope = true;
			if (controller.isGrounded) { // on ground
				jumpVelocity = jumpVelocity * (1- Time.deltaTime);

				if (Input.GetButton("Jump")) {
					jumpVelocity = jumpSpeed;
				}
			}
			
			if (this.inWater) { // on water
				if (jumpVelocity < -15)
					jumpVelocity = jumpVelocity/2;

				if (Input.GetButton("Jump")) {
					jumpVelocity = jumpSpeed;
				}
				
				if (jumpVelocity < 0)
				jumpVelocity += waterfloating * Time.deltaTime;
			}

			jumpVelocity -= gravity * Time.deltaTime;

			moveDirection.y = jumpVelocity * Time.deltaTime;
			
			moveDirection.x = speed * Input.GetAxis("Vertical") * Time.deltaTime;
			moveDirection.z = speed * (-Input.GetAxis("Horizontal")) * Time.deltaTime;

			//if (!slidingDirection.Equals(Vector3.zero))
			//	Debug.Log("real move " + slidingDirection);
			//controller.Move(slidingDirection);
			controller.Move(moveDirection);
			//slidingDirection = Vector3.zero;
			updateCamera();

			// sync to the other player
			lastSync += Time.deltaTime;	
			if (lastSync >= maxLastSync && !lastSyncedPos.Equals(transform.position)) {
				networkView.RPC("movePlayer", RPCMode.Others, this.transform.position.x, this.transform.position.y, this.transform.position.z, Time.time);
				lastSync = 0;
				lastSyncedPos = transform.position;
			}
		} else { // move the other player, interpolate
			if (times.Count > 0) {
				int rmSize = 0;
				float curTime = Time.time;
				int index = times.FindIndex(
					delegate(float t)
					{
					return t + ipDeltaTime > curTime;
				});
				if (index == -1) {
					rmSize = times.Count;
				} else if (index == 0) {
					transform.position = positions[0];
				} else {
					rmSize = index - 1; // delete all before the one before index
					
					float cp = (curTime - ipDeltaTime - times[index-1])/(times[index] - times[index-1]);
					
					transform.position = interpolate(positions[index-1], positions[index], cp);
					
				}
				times.RemoveRange(0, rmSize);
				positions.RemoveRange(0, rmSize);
				
			}
			
		}
	}
	
	Vector3 interpolate (Vector3 x1, Vector3 x2, float t) {
		return new Vector3 (interpolate(x1.x, x2.x, t), interpolate(x1.y, x2.y, t), interpolate(x1.z, x2.z, t));
	}
	
	float interpolate (float x1, float x2, float t) {
		return x2 * t + x1 * (1 - t);
	}

	void updateCamera () {
		Vector3 newCameraPosition = Camera.main.transform.position;
		float followX = cameraFollow;
		float followY = cameraFollow;
		float followZ = cameraFollow;

		followX = followX * Time.deltaTime;
		if (followX > 1) {
			followX = 1;
		}

		if (!controller.isGrounded) {
			followY = cameraFollowJump;
		}
		followY = followY * Time.deltaTime;
		if (followY > 1) {
			followY = 1;
		}

		followZ = followZ * Time.deltaTime;
		if (followZ > 1) {
			followZ = 1;
		}
		
		newCameraPosition.x = interpolate(newCameraPosition.x, cameraOffset.x + transform.position.x, followX);
		newCameraPosition.y = interpolate(newCameraPosition.y, cameraOffset.y + transform.position.y, followY);
		newCameraPosition.z = interpolate(newCameraPosition.z, cameraOffset.z + transform.position.z, followZ);

		Camera.main.transform.position = newCameraPosition;
		Camera.main.transform.rotation = Quaternion.Euler(cameraRot);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		GameObject block = hit.gameObject;
		if (block.name.Equals("Eisblock_")) {
			IceBlock iceblock = block.GetComponent<IceBlock>();
			if (!iceblock.inPlace) {
				block.transform.position = block.transform.position + (-hit.normal*speed*Time.deltaTime/2);
			}
		} else {
			slider(hit.normal);
		}

	}

	void slider (Vector3 colliderNormal) {
		if (GameModel.PlayerId == this.playerId &&  checkSlope) {
			checkSlope = false;
			if (Mathf.Abs(colliderNormal.x) + Mathf.Abs(colliderNormal.z) > 0.2) {
				
				Vector3 slidingDirection = Vector3.zero;
				slidingDirection.x = colliderNormal.x*Time.deltaTime * speed;
				slidingDirection.z =  colliderNormal.z*Time.deltaTime * speed;
				
				controller.Move(slidingDirection);
				//Debug.Log(hit.normal + " " + slidingDirection);
				//controller.Move(new Vector3(hit.normal.x*Time.deltaTime * speed/2, 0, hit.normal.x*Time.deltaTime * speed/2));
			} 
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.name == "GiantCubeOfDeath") {
			die ();
		}

		if (other.name == "CheckPoint") {
			GameModel.lastCheckpoint = other.transform.position;
		}
	}

	void die() {
		GameModel.deathCounter++;
		transform.position = GameModel.lastCheckpoint;
	}
	
	[RPC]
	public void movePlayer(float x, float y, float z, float t) {
		if (t < lastMessage) {
			Debug.Log("message too late " + t);
			return;
		}
		positions.Add(new Vector3(x,y,z));
		times.Add(Time.time);
		lastMessage = t;
	}
}
