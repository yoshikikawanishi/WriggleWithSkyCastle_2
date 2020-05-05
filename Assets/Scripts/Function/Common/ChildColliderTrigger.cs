using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderTrigger : MonoBehaviour {

    [SerializeField] private bool is_Trigger = true;
    [Space]
    [SerializeField] private List<string> trigger_Tag_List = new List<string>();

    private bool hit_Trigger = false;


    /// <summary>
    /// 当たり判定の検知用 ( ColliderとTriggerの区別はインスペクタで行う )
    /// </summary>
    /// <returns>衝突している間trueを返す</returns>
    public bool Hit_Trigger() {
        return hit_Trigger;
    }


    /// <summary>
    /// 当たり判定消す
    /// </summary>
    public void Delete_Collision() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 当たり判定を出す
    /// </summary>
    public void Activate_Collision() {
        gameObject.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (!is_Trigger)
            return;
        foreach(string tag in trigger_Tag_List) {
            if(collision.tag == tag) {
                hit_Trigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!is_Trigger)
            return;
        foreach (string tag in trigger_Tag_List) {
            if (collision.tag == tag) {
                hit_Trigger = false;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (is_Trigger) 
            return;
        foreach(string tag in trigger_Tag_List) {
            if(collision.gameObject.tag == tag) {
                hit_Trigger = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (is_Trigger)
            return;
        foreach (string tag in trigger_Tag_List) {
            if (collision.gameObject.tag == tag) {
                hit_Trigger = false;
            }
        }
    }
}
