using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFairyDeleteBGM : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            BGMManager.Instance.Pause_BGM();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            BGMManager.Instance.Resume_BGM();
        }
    }
}
