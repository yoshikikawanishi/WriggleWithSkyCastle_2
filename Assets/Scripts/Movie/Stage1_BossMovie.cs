using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1_BossMovie : MonoBehaviour {

    //スクリプト
    private MessageDisplayCustom _message;
    //自機
    private GameObject player;
    //ラルバ
    private GameObject larva;

    private bool is_First_Visit = true;


    private void Awake() {
        //取得
        _message = GetComponent<MessageDisplayCustom>();
        player = GameObject.FindWithTag("PlayerTag");
        larva = GameObject.Find("Larva");
    }
	

    //ボス前ムービー開始
    public void Start_Before_Boss_Movie() { 
        StartCoroutine("Play_Before_Boss_Movie_Cor");
    }

    //ボス前ムービー
    private IEnumerator Play_Before_Boss_Movie_Cor() {
        //初期設定
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        PauseManager.Instance.Set_Is_Pausable(false);
        is_First_Visit = SceneManagement.Instance.Is_First_Visit();

        BGMManager.Instance.Stop_BGM();

        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.05f);
        yield return new WaitForSeconds(1.0f);

        //セリフ1
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }

        //ラルバ登場
        MoveTwoPoints _move = larva.GetComponent<MoveTwoPoints>();        
        _move.Start_Move(new Vector3(128f, 0));
        yield return new WaitUntil(_move.End_Move);

        //セリフ2
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 2, 5);
            yield return new WaitUntil(_message.End_Message);
        }

        //終了設定
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);

        //戦闘開始
        larva.GetComponent<Larva>().Start_Battle();
        BGMManager.Instance.Change_BGM("Stage1_Boss");
    }


    //クリア時ムービー開始
    public void Start_Clear_Movie() {
        StartCoroutine("Play_Clear_Movie_Cor");
    }

    //クリア時ムービー
    private IEnumerator Play_Clear_Movie_Cor() {
        yield return new WaitForSeconds(3.0f);

        _message.Start_Display("LarvaText", 6, 9);
        yield return new WaitUntil(_message.End_Message);

        FadeInOut.Instance.Start_Fade_Out(new Color(0, 0, 0), 0.01f);
        BGMManager.Instance.Fade_Out();
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene("PlayGuide2Scene");
    }

}
