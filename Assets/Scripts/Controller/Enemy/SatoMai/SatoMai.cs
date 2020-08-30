using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMai : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;


    void Start() {
        if (SceneManagement.Instance.Is_First_Visit())
            before_Movie.Start_Movie();
        else
            before_Movie_Skip.Start_Movie();
    }


    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        clear_Movie.Start_Movie();
    }
}
