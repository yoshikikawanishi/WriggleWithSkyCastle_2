using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Okina : BossEnemy {

    [Space]
    [SerializeField] private MovieSystem before_Movie;
    [SerializeField] private MovieSystem before_Movie_Skip;
    [SerializeField] private MovieSystem clear_Movie;


    void Start() {
        if (SceneManagement.Instance.Is_First_Visit()) {
            before_Movie.Start_Movie();
        }
        else {
            before_Movie_Skip.Start_Movie();
        }
    }


    public override void Start_Battle() {
        //BGMManager.Instance.Change_BGM()
        GetComponentInChildren<MelodyManager>().Start_Time_Count();
        base.Start_Battle();
    }


    protected override void Clear() {
        base.Clear();
    }


    protected override void Do_After_Clear_Process() {
        clear_Movie.Start_Movie();
    }

    
    public void Become_Invincible() {
        GetComponentInChildren<BossChildCollision>().Become_Invincible();
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    }


    public void Release_Invincible() {
        GetComponentInChildren<BossChildCollision>().Release_Invincible();
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

}
