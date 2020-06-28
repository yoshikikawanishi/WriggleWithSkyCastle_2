using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃を受けると収集アイテムを出す
/// 子要素に収集アイテムを置くこと
/// </summary>
public class CollectionBox : MonoBehaviour {    

    private List<string> hit_Tag_List = new List<string> {
        "PlayerTag",
        "PlayerBodyTag"
    };

    private bool is_Hitting = false;


    void Update() {
        if (is_Hitting) {
            if (Input.GetAxisRaw("Vertical") > 0) {
                StartCoroutine("Put_Out_Collection");
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (hit_Tag_List.Contains(collision.tag)) {
            is_Hitting = true;
        }        
    }


    void OnTriggerExit2D(Collider2D collision) {
        if (hit_Tag_List.Contains(collision.tag)) {
            is_Hitting = false;
        }
    }


    private IEnumerator Put_Out_Collection() {
        if(transform.childCount == 0) {
            yield break;
        }
        this.enabled = false;
        GetComponent<Animator>().SetBool("OpenBool", true);
        GetComponents<AudioSource>()[0].Play();
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.3f);

        transform.GetChild(0).gameObject.SetActive(true);
    }

}
