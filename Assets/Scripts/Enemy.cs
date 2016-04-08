using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	enum CharacterState { Idle, LeftPunch, Blocking, BlockStun, KnockedOut };

	const int MAX_HITPOINTS = 7;

	CharacterState currentState;
	Animator anim;
	PlayerController player;
	float timer;	
	int hitpoints;
	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
		GameObject playerObj = GameObject.Find ("Player");
		player = playerObj.GetComponent<PlayerController>();
		timer = 0.0f;
		hitpoints = MAX_HITPOINTS;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer >= 1.0f) {

			if(!player.isKnockedOut() && currentState == CharacterState.Idle) {
				if(Random.value <= 0.50f) 
				{
					anim.SetTrigger("LeftPunch");
					currentState = CharacterState.LeftPunch;
				}
				else if(Random.value <= 0.50f)
				{
					anim.SetTrigger("Block");
					currentState = CharacterState.Blocking;
				}
			}




			timer = 0.0f;
		}


	}

	public void OnLeftHeadHit() {

		if (currentState == CharacterState.Blocking) {
			anim.SetTrigger("LeftHeadHit");
		}
		else if (currentState != CharacterState.KnockedOut){
			anim.SetTrigger("LeftHeadHit");

			--hitpoints;
			if(hitpoints <= 0) {
				anim.SetTrigger("Knockout");
				GameManager.instance.RestartGame();
			}
		}

	}

	public void OnRightHeadHit() {
		anim.SetTrigger("RightHeadHit");
		
		hitpoints = hitpoints - 2;
		if(hitpoints <= 0) {
			anim.SetTrigger("Knockout");
			GameManager.instance.RestartGame();
		}
	}
	
	public void OnPlayerJab() {
		if (currentState == CharacterState.Idle) {
			anim.SetTrigger("Block");
			currentState = CharacterState.Blocking;

			if(Random.value <= 0.40f) {
				anim.SetTrigger("CounterHook");
			}
		}
	}

	public void OnPlayerHook() {
		if (currentState == CharacterState.Idle) {
			anim.SetTrigger("CounterJab");
			currentState = CharacterState.LeftPunch;
		}
	}

	void HitPlayer() {
		player.OnReceivedHit();
	}

	void HookedPlayer() {
		player.OnReceivedHook();
	}

	void SetToIdle() {
		currentState = CharacterState.Idle;
	}
}
