using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

public class Hina : TalkCharacter {

    //厄、毛玉ザコ敵生成用
    [SerializeField] private HinaDisaster hina_Disaster;    

    private GameObject disaster_Effect;
    private GameObject player;

    private bool is_Player_Having_Disaster;


    private new void Start() {
        base.Start();
        //取得
        player = GameObject.FindWithTag("PlayerTag");

        //にとりと雛の収集アイテムを取得済みなら移動
        CollectionManager c = CollectionManager.Instance;
        if (c.Is_Collected("Nitori") && c.Is_Collected("Hina")) {
            transform.position = new Vector3(5350f, -36f);
            gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
            this.enabled = false;
        }
    }

    private void Update() {
        //収集アイテム獲得イベント
        Play_Collection_Event();
    }


    //==========================会話関連==========================       
    protected override float Action_Before_Talk() {        
        if(start_ID == 1 && !is_Player_Having_Disaster)
            Infect_Disaster_Effect_To_Player();
        return 0;
    }


    //=================ボタン関数===================
    //はいボタン押下時
    //厄を払う
    public void Yes_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {            
            Delete_Disaster_Effect_In_Player();            
        }
    }


    //いいえボタン押下時
    //厄攻撃開始
    public void No_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            hina_Disaster.Start_Generate();
            //セリフを変える
            Change_Message_Status("HinaText", 3, 3);
            GetComponent<MessageDisplay>().is_Display_Selection_After_Message = false;
        }
    }

    //===============厄イベント関連==========================
    //自機に厄エフェクトをまとわせる
    private void Infect_Disaster_Effect_To_Player() {        
        GameObject effect = transform.Find("HinaDisaster").gameObject;
        disaster_Effect = Instantiate(effect, player.transform);

        player.GetComponentInChildren<PlayerEffect>().Play_Dark_Powder_Effect();
        is_Player_Having_Disaster = true;
    }

    //自機の厄エフェクトをはずす
    private void Delete_Disaster_Effect_In_Player() {
        Destroy(disaster_Effect);

        player.GetComponentInChildren<PlayerEffect>().Play_Green_Powder_Effect();
        is_Player_Having_Disaster = false;
    }

    //アイテム獲得イベント
    private void Play_Collection_Event() {
        //厄をまとったままステージ右端まで行ったとき
        if (is_Player_Having_Disaster && player.transform.position.x > 5300f) {
            Delete_Disaster_Effect_In_Player();
            GetComponent<HinaMovie>().Start_Hina_Movie();
        }
    }
}
