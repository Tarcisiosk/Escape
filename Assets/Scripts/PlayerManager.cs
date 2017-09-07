using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants {
}

public class PlayerManager : MonoBehaviour {

	public enum Position {
		Left,
		Middle,
		Right,
	}

	public Position currentPos;

	// Use this for initialization
	void Start () {
		currentPos = Position.Middle;
		transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
	}

	void SetPos(){
		switch(currentPos){
		case Position.Left:
			transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case Position.Middle:
			transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case Position.Right:
			transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6*5, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			if(currentPos == Position.Right){
				currentPos = Position.Middle;
			} else if(currentPos == Position.Middle){
				currentPos = Position.Left;
			} else {
				currentPos = Position.Left;
			}
			SetPos();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)){
			if(currentPos == Position.Left){
				currentPos = Position.Middle;
			} else if(currentPos == Position.Middle){
				currentPos = Position.Right;
			} else {
				currentPos = Position.Right;
			}
			SetPos();
		}
	}

}
