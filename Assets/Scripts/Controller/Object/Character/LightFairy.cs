using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFairy : TalkCharacter {


    new void Start() {
        base.Start();
        mark_Up_Baloon.SetActive(false);
    }


    protected override float Action_Before_Talk() {
        if (start_ID == 1) {
            return 0;
        }
        else if(start_ID == 2) {
            StartCoroutine("Appear");
            return 2.0f;
        }
        else {
            return 1.0f;
        }
    }

    protected override void Action_In_End_Talk() {
        if(start_ID == 1) {
            Change_Message_Status("LightFairyText", 2, 7);
            StartCoroutine(Talk());
        }
        else if(start_ID == 2) {
            Play_Star_Effect();
            Change_Message_Status("LightFairyText", 8, 10);
            StartCoroutine(Talk());
        }
        else {
            Put_Out_Item();
            gameObject.SetActive(false);
        }
    }


    //登場
    private IEnumerator Appear() {
        BGMManager.Instance.Resume_BGM();
        SpriteMask _mask = GetComponentInChildren<SpriteMask>();
        for (int i = 0; i < 8; i++) {
            _mask.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _mask.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        _mask.enabled = false;
    }


    //登場エフェクト
    private void Play_Star_Effect() {
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
    }


    //アイテム放出
    private void Put_Out_Item() {
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(4).SetParent(null);
    }

}
