using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mystia : TalkCharacter {

    private new void Start() {
        base.Start();
        //ルーミアとミスティア発見済みならルーミアとの会話用に
        if(PlayerPrefs.GetInt("Rumia") == 2) {
            Change_Status_With_Rumia();
        }
    }


    //会話前
    protected override float Action_Before_Talk() {
        base.Action_Before_Talk();

        int rumia = PlayerPrefs.GetInt("Rumia");
        Debug.Log(rumia + " " + talk_Count);
        //ルーミア未発見
        if(rumia == 0) {
            if (talk_Count == 1)
                Change_Message_Status("MystiaText", 1, 2);
            else
                Change_Message_Status("MystiaText", 3, 3);
        }
        //ルーミア発見済み
        else if(rumia == 1) {
            if (talk_Count == 1)
                Change_Message_Status("MystiaText", 1, 2);
            else
                Change_Message_Status("MystiaText", 4, 6);
        }
        //ルーミアの隣にいるとき
        else if(rumia == 2) {
            Change_Status_With_Rumia();
        }
        return 0;
    }


    protected override void Action_In_End_Talk() {       
        StartCoroutine("Action_After_Talking_Cor");        
    }


    //会話終了時
    private IEnumerator Action_After_Talking_Cor() {
        int rumia = PlayerPrefs.GetInt("Rumia");

        //初回かつルーミアの隣じゃない
        if (talk_Count == 1 && rumia != 2) {
            Put_Out_Collection_Box();
            yield break;
        }
        //ルーミア発見済み
        else if (talk_Count > 1 && rumia == 1) {
            //当たり判定消す
            GetComponent<BoxCollider2D>().enabled = false;
            //飛び去る
            MoveTwoPoints _move = GetComponent<MoveTwoPoints>();
            _move.Start_Move(transform.position + new Vector3(-300f, 150f));
            GetComponent<Animator>().SetTrigger("FlyTrigger");
            mark_Up_Baloon.SetActive(false);
            //エフェクト消す
            GetComponentInChildren<ParticleSystem>().Stop();
            //アイテムを出す
            Put_Out_Score(15);
            //ルーミアの隣に配置
            yield return new WaitForSeconds(5.0f);
            GetComponent<BoxCollider2D>().enabled = true;
            Change_Status_With_Rumia();
        }        
    }
    

    //ルーミアの隣に移動
    private void Change_Status_With_Rumia() {
        Change_Message_Status("MystiaText", 7, 7);
        transform.position = new Vector3(824f, 18f, 0);
        PlayerPrefs.SetInt("Rumia", 2);
    }

}
