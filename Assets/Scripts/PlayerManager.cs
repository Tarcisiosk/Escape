using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {
	public int level, health, damage, armor;
	public float speed;
};

public class PlayerManager : MonoBehaviour {

	public enum Lane {
		Left,
		Middle,
		Right
	}

	public enum Role {
		Warrior,
		Hunter
	}

	public Lane currentPos;
	public Role currentRole;
	public float xSpeed;
	public bool isAligned;
	private float treshold = 0.01f;
	private float distanceToAttack = 1f;
	public Character warrior = new Character();
	public Character hunter = new Character();

	void Start () {
		xSpeed = 3;
		isAligned = true;
		currentPos = Lane.Middle;
		transform.position = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2, Screen.height/6, Camera.main.nearClipPlane) );
		currentRole = Role.Warrior;
		SetupHunter();
		SetupWarrior();
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

	void SetupWarrior(){
		distanceToAttack = 0.1f;
		warrior.level = 1;
		warrior.health = 100;
		warrior.damage = 10;
		warrior.armor = 5;
	}

	void SetupHunter(){
		distanceToAttack = 1.0f;
		hunter.level = 1;
		hunter.health = 50;
		hunter.damage = 5;
		hunter.armor = 0;
	}

	void ChangeCharacter(){
		if(currentRole == Role.Warrior){
			currentRole = Role.Hunter;
		} else {
			currentRole = Role.Warrior;
		}
	}

	void Attack(EnemyManager enemy){
		Debug.Log("Toma seu puto!!!");
	}

	bool InFrontOfEnemy(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
		return hit.collider != null && hit.collider.gameObject.tag == "Enemy" && Mathf.Abs(hit.point.y - transform.position.y) < distanceToAttack;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy"){
			if(currentRole == Role.Warrior){
			} else {
			}
		}
	}

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

		if (Input.GetKeyDown(KeyCode.Space)){
			ChangeCharacter();
		}

		if(!isAligned){
			SetPos();
		}
	}

	void FixedUpdate() {
		if(InFrontOfEnemy()){
			Attack(null);
		}
	}

}
