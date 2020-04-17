using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public enum ItemKind {
        power,
        score,
        beetle_Power,
        life_Up,
        stock_Up,
    }
    [SerializeField] private ItemKind kind;
    [SerializeField] private int value;

    
	// Update is called once per frame
	void Update () {
        //下まで落ちたら消す
        if (transform.position.y < -200f) {
            gameObject.SetActive(false);
        }
    }


    //自機と当たったら消す
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag" || collision.tag == "BeetleBodyTag") {
            switch (kind) {
                case ItemKind.power:        Gain_Power_Item();          break;
                case ItemKind.score:        Gain_Score_Item();          break;
                case ItemKind.beetle_Power: Gain_Beetle_Power_Item();   break;
                case ItemKind.life_Up:      Gain_Life_Up_Item();        break;
                case ItemKind.stock_Up:     Gain_Stock_Up_Item();       break;
            }
            gameObject.SetActive(false);
        }
    }    


    //P取得時
    private void Gain_Power_Item() {
        PlayerManager.Instance.Add_Power();
        UsualSoundManager.Instance.Play_Get_Small_Item_Sound();
    }

    //点取得時
    private void Gain_Score_Item() {
        PlayerManager.Instance.Add_Score(100);
        UsualSoundManager.Instance.Play_Get_Small_Item_Sound();
    }

    //カブトムシパワー取得時
    private void Gain_Beetle_Power_Item() {
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", value);
        UsualSoundManager.Instance.Play_Get_Small_Item_Sound();
        //エフェクト
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        ReleaseAndConvergePlayer RC = transform.GetChild(0).GetComponent<ReleaseAndConvergePlayer>();        
        RC.Play_Release_And_Converge(6, transform.position, new Vector2(208f, -104f), main_Camera);
        transform.GetChild(0).SetParent(null);

        Destroy(gameObject);
    }

    //回復取得時
    private void Gain_Life_Up_Item() {
        PlayerManager.Instance.Add_Life();
        GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerEffect>().Play_Red_Powder_Effect();
        UsualSoundManager.Instance.Play_Life_Up_Sound();
    }

    //残機アップアイテム取得時
    private void Gain_Stock_Up_Item() {
        PlayerManager.Instance.Add_Stock();
        GameObject.FindWithTag("PlayerTag").GetComponentInChildren<PlayerEffect>().Play_Green_Powder_Effect();
        UsualSoundManager.Instance.Play_Stock_Up_Sound();        
        Destroy(gameObject);

        Debug.Log("Don't ObjectPool Stock Item");
    }

}
