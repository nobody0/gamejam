﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (NetworkView))]
[RequireComponent (typeof (CharacterController))]
public class Player : MonoBehaviour {
	public int playerId;
	
	private CharacterController controller;
	
	private float gravity = 10;
	private float speed = 5;
	private float jumpSpeed = 5;
	private float jumpVelocity = 0;

	private int lineIndex;
	private float[] Lines = {-2, 2};
	
	private float lastSync = 0.0f;
	private Vector3 lastSyncedPos;
	private float maxLastSync =  0.018f;
	private float lastMessage;
	
	private float ipDeltaTime = 0.1f;
	
	public Vector3 cameraOffset = new Vector3(-4, 3, -7);
	private float cameraFollow =  10;
	private float cameraFollowJump =  5;
	//public Vector3 cameraOffsetRot = new Vector3(20,30,5);

	private List<Vector3> positions = new List<Vector3>();
	private List<float> times = new List<float>();
	
	void Start () {
		controller = GetComponent<CharacterController>();
		lastSyncedPos = transform.position;
	}
	
	void Update () {
		if (!Network.isServer && !Network.isClient){
			// delete me!
			return;
		}
		
		if (GameModel.PlayerId == this.playerId) { // move our player
			Vector3 moveDirection = Vector3.zero;
			// save old position!
			
			if (controller.isGrounded) {
				jumpVelocity = 0;
				if (Input.GetButton("Jump")) {
					jumpVelocity = jumpSpeed;
				}
			}
			jumpVelocity -= gravity * Time.deltaTime;
			moveDirection.y = jumpVelocity * Time.deltaTime;
			
			moveDirection.x = speed * Input.GetAxis("Horizontal") * Time.deltaTime;

			if (Input.GetButtonDown("Vertical")) {
				if (Input.GetAxis("Vertical") > 0) {
					lineIndex ++;
					if (lineIndex >= Lines.Length) {
						lineIndex = Lines.Length - 1;
					}
				} else {
					lineIndex--;
					if (lineIndex < 0) {
						lineIndex = 0;
					}
				}
			}

			float diffZ = Lines[lineIndex] - transform.position.z;

			moveDirection.z = speed * Time.deltaTime;

			if (diffZ < 0) {
				moveDirection.z *= -1;
			}
			if (moveDirection.z / diffZ > 1) {
				moveDirection.z = diffZ;
			}

			controller.Move(moveDirection);

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
		
		newCameraPosition.x = interpolate(newCameraPosition.x, cameraOffset.x + transform.position.x, followX);
		newCameraPosition.y = interpolate(newCameraPosition.y, cameraOffset.y + transform.position.y, followY);

		Camera.main.transform.position = newCameraPosition;
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
