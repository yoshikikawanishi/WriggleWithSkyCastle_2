using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootCollision : MonoBehaviour {

    //本体
    private GameObject player;
    //コンポーネント
    private PlayerController player_Controller;
    private PlayerSoundEffect player_SE;
    private PlayerEffect player_Effect;
    private Rigidbody2D player_Rigid;    

    //着地判定を取るタグリスト
    private List<string> LANDING_TAG_LIST = new List<string> {
        "GroundTag",
        "ThroughGroundTag",
        "SandbackGroundTag",
        "DamagedGroundTag"
    };

    private int leave_Land_Frame_Count = 0;
    private bool is_Count_Leave_Land_Frame = false;

    
    private void Start() {
        player = transform.parent.gameObject;        
        player_Controller   = player.GetComponent<PlayerController>();
        player_SE = player.GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = player.GetComponentInChildren<PlayerEffect>();
        player_Rigid = player.GetComponent<Rigidbody2D>();
    }


    private void LateUpdate() {
        //地面から離れて5F間着地判定がなければ、接地判定をなくす
        if (is_Count_Leave_Land_Frame) {
            leave_Land_Frame_Count++;
            if(leave_Land_Frame_Count == 3) {
                player_Controller.is_Landing = false;
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision) {
        //着地判定
        foreach (string tag_Name in LANDING_TAG_LIST) {
            if (collision.tag == tag_Name) {
                leave_Land_Frame_Count = 0;
                is_Count_Leave_Land_Frame = false;
            }
        }

        if (player_Controller.Get_Is_Ride_Beetle()) //飛行中は無し 
            return;        
        if (player_Controller.is_Landing)           //すでに地面にいるとき
            return;
        if (player_Rigid.velocity.y > 10f)          //上昇中
            return; 

        //着地時の処理        
        foreach (string tag_Name in LANDING_TAG_LIST) {            
            if (collision.tag == tag_Name) {
                player_Controller.is_Landing = true;
                Landing();                
            }
        }        
    }

    
    private void OnTriggerExit2D(Collider2D collision) {
        //地面から離れる
        foreach(string tag_Name in LANDING_TAG_LIST) {
            if(collision.tag == tag_Name) {
                is_Count_Leave_Land_Frame = true;
            }
        }        
    }


    //着地時の処理
    private void Landing() {
        player_Controller.Change_Animation("IdleBool");
        player_SE.Play_Land_Sound();
        player_Effect.Play_Jump_Effect();
    }


    //判定を消す、戻す
    public void Disappear() {
        gameObject.SetActive(false);
    }

    public void Appear() {
        gameObject.SetActive(true);
    }
}
