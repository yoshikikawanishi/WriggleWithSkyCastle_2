using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃を受けると収集アイテムを出す
/// 子要素に収集アイテムを置くこと
/// </summary>
public class CollectionBox : MonoBehaviour {    

    private List<string> hit_Tag_List = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
        "PlayerKickTag",
    };	


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in hit_Tag_List) {
            if(collision.tag == tag) {
                StartCoroutine(Put_Out_Collection());
            }
        }
    }


    private IEnumerator Put_Out_Collection() {
        if(transform.childCount == 0) {
            yield break;
        }
        GetComponent<Animator>().SetBool("OpenBool", true);
        GetComponents<AudioSource>()[0].Play();
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.3f);

        transform.GetChild(0).gameObject.SetActive(true);
    }

}
