using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Start () {
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartGame() {
		StartCoroutine(DelayedRestart(4.0f));
	}

	IEnumerator DelayedRestart(float delay) {
		float currentTime = 0;
		while (currentTime < delay) {


			currentTime = currentTime + Time.deltaTime;
			
			yield return new WaitForEndOfFrame();
		}

		Application.LoadLevel(Application.loadedLevel);
	}
}
