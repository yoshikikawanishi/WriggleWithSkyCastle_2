using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {    

	// Use this for initialization
	void Start () {
        Invoke("Test1", 1);
        Invoke("Test2", 5);
	}
	

	private void Test1() {
        ;
    }

   
    private void Test2() {
        
    }

}
