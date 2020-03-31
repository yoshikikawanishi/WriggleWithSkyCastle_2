using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

    private PlayerDamaged player_Damaged;
    private float time = 0;

    private void Start() {
        player_Damaged = transform.parent.GetComponent<PlayerDamaged>();
    }

    private List<string> tag_List = new List<string> {
        "GroundTag",
        "ScreenWallTag",
        "SandbackGroundTag"
    };


    private void OnTriggerStay2D(Collider2D collision) {
        foreach(string tag in tag_List) {
            if(collision.tag == tag) {
                time += Time.deltaTime;
            }
        }
        if(time >= 0.05f) {
            time = 0;
            player_Damaged.StartCoroutine("Damaged");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        foreach (string tag in tag_List) {
            if (collision.tag == tag) {
                time = 0;
            }
        }
    }
}
