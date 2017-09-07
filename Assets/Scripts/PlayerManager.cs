using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants {
}

public class PlayerManager : MonoBehaviour {

	public enum PlayerPosition {
		Left,
		Middle,
		Right,
	}

	public PlayerPosition currentPos;
	public float xSpeed;
	public bool isAligned;
	private float treshold = 0.01f;

	// Use this for initialization
	void Start () {
		xSpeed = 3;
		isAligned = true;
		currentPos = PlayerPosition.Middle;
		transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
	}

	void SetPos(){
		Vector3 destination =  new Vector3(0, 0, 0);

		switch(currentPos){
		case PlayerPosition.Left:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case PlayerPosition.Middle:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case PlayerPosition.Right:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6*5, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		}

		transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime*xSpeed);

		if(Mathf.Abs(transform.position.x-destination.x) < treshold){
			isAligned = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			if(currentPos == PlayerPosition.Right){
				currentPos = PlayerPosition.Middle;
			} else if(currentPos == PlayerPosition.Middle){
				currentPos = PlayerPosition.Left;
			} else {
				currentPos = PlayerPosition.Left;
			}
			isAligned = false;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)){
			if(currentPos == PlayerPosition.Left){
				currentPos = PlayerPosition.Middle;
			} else if(currentPos == PlayerPosition.Middle){
				currentPos = PlayerPosition.Right;
			} else {
				currentPos = PlayerPosition.Right;
			}
			isAligned = false;
		}

		if(!isAligned){
			SetPos();
		}
	}

}
