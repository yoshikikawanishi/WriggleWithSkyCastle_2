using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOption : MonoBehaviour {

    
    private PlayerManager.Option now_Option = PlayerManager.Option.none;


	// Update is called once per frame
	void Update () {
		if(now_Option != PlayerManager.Instance.Get_Option()) {
            now_Option = PlayerManager.Instance.Get_Option();
            Change_Animation();
        }
	}


    //アニメーション変更
    private void Change_Animation() {
        Animator _anim = GetComponent<Animator>();

        switch (now_Option) {
            case PlayerManager.Option.none:     _anim.SetTrigger("NoneTrigger");        break;
            case PlayerManager.Option.bee:      _anim.SetTrigger("BeeTrigger");         break;
            case PlayerManager.Option.butterfly: _anim.SetTrigger("ButterflyTrigger");  break;
            case PlayerManager.Option.mantis:   _anim.SetTrigger("MantisTrigger");      break;
            case PlayerManager.Option.spider:    _anim.SetTrigger("SpiderTrigger");     break;
        }
    }
    
}
