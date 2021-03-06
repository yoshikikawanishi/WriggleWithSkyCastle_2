﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4_BossScene : MonoBehaviour {

    [SerializeField] private MovieSystem start_Movie;
    [SerializeField] private MovieSystem start_Movie_Skip;    

	
	void Start () {
        BGMManager.Instance.Stop_BGM();
        if (SceneManagement.Instance.Is_First_Visit()) {
            start_Movie.Start_Movie();
        }
        else {
            start_Movie_Skip.Start_Movie();
        }
	}	
}
