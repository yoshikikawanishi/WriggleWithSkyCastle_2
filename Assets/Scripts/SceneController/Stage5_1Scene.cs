using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5_1Scene : MonoBehaviour {

    [SerializeField] private MovieSystem first_Visit_Movie;

	
	void Start () {
        if (SceneManagement.Instance.Is_First_Visit()) {
            first_Visit_Movie.Start_Movie();
        }	
	}
	
	
}
