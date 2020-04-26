using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_1Scene : SingletonMonoBehaviour<Stage1_1Scene> {   


    private void Start() {
        //オープニング
        GetComponent<OpeningMovie>().Start_Movie();            
        
        BGMManager.Instance.Change_BGM("Stage1");
        
    }

}
