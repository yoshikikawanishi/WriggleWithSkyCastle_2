using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrazeCollision : MonoBehaviour {

    private PlayerSoundEffect player_SE;

    private float time;

    private void Awake() {
        player_SE = transform.parent.GetComponentInChildren<PlayerSoundEffect>();    
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "EnemyBulletTag") {
            BeetlePowerManager.Instance.Increase_In_Update(1.35f);
            if (time < 0.05f) {
                time += Time.deltaTime;
            }
            else {
                time = 0;
                PlayerManager.Instance.Add_Score(1);
                player_SE.Play_Graze_Sound();
            }
        }
    }

}
