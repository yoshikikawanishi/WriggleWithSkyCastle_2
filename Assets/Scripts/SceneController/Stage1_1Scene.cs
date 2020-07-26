using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Scene : SingletonMonoBehaviour<Stage1_1Scene> {   


    private void Start() {
        //ムービー
        GetComponent<Stage1_1Movie>().Start_Movie();                                   
    }

}
