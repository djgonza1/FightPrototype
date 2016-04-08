using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	enum CharacterState { Idle, LeftHeadPunch, RightHeadPunch, TakingJab, TakingHook, KnockedOut, Blocking, BlockStun };

	const int MAX_HITPOINTS = 5;

	CharacterState currentState;

	Animator anim;
	Enemy enemy;
	int hitpoints;

	int idleState;
	// Use this for initialization
	void Start () {
		currentState = CharacterState.Idle;
		anim = this.GetComponent<Animator>();
		hitpoints = MAX_HITPOINTS;

		GameObject enemyObj = GameObject.Find("Enemy") as GameObject;
		enemy = enemyObj.GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!isBusy ()) {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				anim.SetTrigger ("LeftHeadPunch");
				currentState = CharacterState.LeftHeadPunch;
			}
			else if(Input.GetKeyDown(KeyCode.Mouse1)) {
				anim.SetTrigger("RightHeadPunch");
				currentState = CharacterState.RightHeadPunch;
			}
			else if (Input.GetKey(KeyCode.Space)) {
				anim.SetBool ("Blocking", true);
				currentState = CharacterState.Blocking;
			}
		} 

		if(currentState == CharacterState.Blocking && !Input.GetKey(KeyCode.Space)) {
			anim.SetBool ("Blocking", false);
			currentState = CharacterState.Idle;
		}
	}

	public void OnReceivedHit() {
		if(currentState != CharacterState.KnockedOut) {
			anim.SetTrigger("TakingJab");

			if(currentState == CharacterState.Blocking) {
				currentState = CharacterState.BlockStun;
			}
			else if(currentState != CharacterState.Blocking) {
				currentState = CharacterState.TakingJab;

				--hitpoints;
				if (hitpoints <= 0) {
					anim.SetTrigger("Knockout");
					currentState = CharacterState.KnockedOut;
					GameManager.instance.RestartGame();
				}
			}

			

		}
	}

	public void OnReceivedHook() {
		if(currentState != CharacterState.KnockedOut) {
			anim.SetTrigger("TakingHook");
			anim.SetBool("Blocking", false);
			currentState = CharacterState.TakingHook;


			hitpoints = hitpoints - 2;
			if (hitpoints <= 0) {
				anim.SetTrigger("Knockout");
				currentState = CharacterState.KnockedOut;
				GameManager.instance.RestartGame();
			}
		}
	}

	public bool isKnockedOut() {
		return currentState == CharacterState.KnockedOut;
	}
	
	void OnJab(){
		enemy.OnPlayerJab();
	}

	void OnHook() {
		enemy.OnPlayerHook();
	}

	bool isBusy() {
		return currentState != CharacterState.Idle;
	}

	void SetToIdle() {
		if (currentState != CharacterState.KnockedOut) {
			currentState = CharacterState.Idle;
		}
	}

	void OnBlockStunEnd() {
		currentState = CharacterState.Blocking;
	}

	void HitEnemy(){
		enemy.OnLeftHeadHit();
	}

	void HookEnemy() {
		enemy.OnRightHeadHit();
	}
	
}
