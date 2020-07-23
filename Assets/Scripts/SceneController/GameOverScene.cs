using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Play_Movie();
	}

    private void Play_Movie() {
        GetComponent<MovieSystem>().Start_Movie();
    }    
		
    public void Revive() {
        GameManager.Instance.StartCoroutine("Revive");
    }
}
