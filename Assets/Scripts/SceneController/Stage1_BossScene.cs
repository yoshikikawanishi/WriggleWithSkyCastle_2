﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_BossScene : MonoBehaviour {

    [SerializeField] private BossEnemy boss;


	// Use this for initialization
	void Start () {
        //ムービー開始
        GetComponent<Stage1_BossMovie>().Start_Before_Boss_Movie();
	}	
}
