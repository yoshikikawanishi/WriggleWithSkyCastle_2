using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionItem : MonoBehaviour {

    public PlayerManager.Option option;

    private void Start() {
        Animator _anim = GetComponent<Animator>();
        switch (option) {
            case PlayerManager.Option.bee:          _anim.SetTrigger("BeeTrigger"); break;
            case PlayerManager.Option.butterfly:    _anim.SetTrigger("ButterflyTrigger"); break;
            case PlayerManager.Option.mantis:       _anim.SetTrigger("MantisTrigger"); break;
            case PlayerManager.Option.spider:       _anim.SetTrigger("SpiderTrigger"); break;
            case PlayerManager.Option.none:         _anim.SetTrigger("NoneTrigger"); break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            Aquire_Option();
        }
    }

    private void Aquire_Option() {
        //初入手時ガイドを出す
        if(PlayerManager.Instance.Get_Option() == PlayerManager.Option.none) {            
            if(PlayerPrefs.GetString("OPTION") == "none") {
                GuideWindowDisplayer _guide = gameObject.AddComponent<GuideWindowDisplayer>();
                _guide.Open_Window("UI/GuideOption");
            }
        }
        PlayerManager.Instance.Set_Option(option);
        Play_Get_Effect();
        Destroy(gameObject, Time.deltaTime * 10);
    }


    private void Play_Get_Effect() {
        var effect = Instantiate(Resources.Load("Effect/GetOptionEffect") as GameObject);
        effect.transform.position = transform.position;
        Destroy(effect, 2.0f);
    }
}
