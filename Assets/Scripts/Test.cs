using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {    

	// Use this for initialization
	void Start () {
        GetComponent<MovieSystem>().Start_Movie();       
	}
	

	private void Test1() {
        Debug.Log("Test");
    }

   

}
