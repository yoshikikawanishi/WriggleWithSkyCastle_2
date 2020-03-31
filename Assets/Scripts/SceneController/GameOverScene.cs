using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour {

    [SerializeField] private Button initial_Selected_Button;


	// Use this for initialization
	void Start () {
        initial_Selected_Button.Select();
	}
		
}
