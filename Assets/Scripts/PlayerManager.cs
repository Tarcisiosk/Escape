using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants {
}

public class PlayerManager : MonoBehaviour {

	public enum Lane {
		Left,
		Middle,
		Right,
	}

	public Lane currentPos;
	public float xSpeed;
	public bool isAligned;
	private float treshold = 0.01f;
	private float distanceToAttack = 1f;

	// Use this for initialization
	void Start () {
		xSpeed = 3;
		isAligned = true;
		currentPos = Lane.Middle;
		transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
	}

	void SetPos(){
		Vector3 destination =  new Vector3(0, 0, 0);

		switch(currentPos){
		case Lane.Left:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case Lane.Middle:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		case Lane.Right:
			destination = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6*5, Screen.height/6, Camera.main.nearClipPlane) );
			break;
		}

		transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime*xSpeed);

		if(Mathf.Abs(transform.position.x-destination.x) < treshold){
			isAligned = true;
		}
	}

	bool inFrontOfEnemy(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
		return hit.collider != null && hit.collider.gameObject.tag == "Enemy" && Mathf.Abs(hit.point.y - transform.position.y) < distanceToAttack;
	}

	void Attack(){
		Debug.Log("Toma seu puto!!!");	
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			if(currentPos == Lane.Right){
				currentPos = Lane.Middle;
			} else if(currentPos == Lane.Middle){
				currentPos = Lane.Left;
			} else {
				currentPos = Lane.Left;
			}
			isAligned = false;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)){
			if(currentPos == Lane.Left){
				currentPos = Lane.Middle;
			} else if(currentPos == Lane.Middle){
				currentPos = Lane.Right;
			} else {
				currentPos = Lane.Right;
			}
			isAligned = false;
		}

		if(!isAligned){
			SetPos();
		}
	}

	void FixedUpdate() {
		if(inFrontOfEnemy()){
			Attack();
		}
	}
		

}
