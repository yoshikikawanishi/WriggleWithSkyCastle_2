using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 収集アイテム
/// </summary>
public class CollectionItem : MonoBehaviour {

    [SerializeField] private string collection_Name;

    //取得時に表示したいガイドがあれそのキャンバスの名前を入れる
    //キャンバスのプレハブはResources/UIフォルダに入れる
    [SerializeField] private string guide_Canvas_Name;

    //すでに取得済みの時、代わりに回復アイテムを出すか否か
    //出さない場合は何も出さない
    [SerializeField] private bool is_Change_Life_Up_Item = false;


    private void Awake() {
        //すでに取得済みの時
        if (CollectionManager.Instance.Is_Collected(collection_Name)) {
            if (is_Change_Life_Up_Item) {
                GameObject item = Instantiate(Resources.Load("Object/LifeUpItem") as GameObject);
                item.transform.position = transform.position;
            }
            Destroy(gameObject);            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            StartCoroutine(Aquire_Collection());
        }
    }


    protected virtual IEnumerator Aquire_Collection() {        

        GetComponent<VerticalVibeMotion>().enabled = false;
        GetComponent<Animator>().SetBool("RaiseBool", true);
        GetComponent<AudioSource>().Play();
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");

        //アイテムを初めて取得したときのガイド表示
        Display_Guide_In_First_Item();

        yield return new WaitForSeconds(0.5f);

        //表示したいガイドがあれば表示
        if(guide_Canvas_Name != "") {
            Display_Specific_Guide();
        }

        yield return new WaitForSeconds(0.5f);
        
        CollectionManager.Instance.Aquire_Collection(collection_Name);

        Destroy(gameObject);
    }


    //アイテムを初めて取得したときのガイド表示
    private void Display_Guide_In_First_Item() {        
        //アイテムを初めて取得するかどうか
        var data_Dictionary = CollectionManager.Instance.Get_Collections_Data();
        List<string> key_List = new List<string>(data_Dictionary.Keys);        

        foreach (string key in key_List) {
            if(data_Dictionary[key]) {                
                return;
            }
        }

        //ガイドウィンドウの表示
        GetComponent<GuideWindowDisplayer>().Open_Window("UI/GuideInFirstItem");        
    }


    //アイテム取得時にガイドを表示する
    private void Display_Specific_Guide() {        
        GetComponent<GuideWindowDisplayer>().Open_Window("UI/" + guide_Canvas_Name);
    }
}
