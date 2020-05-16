using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGround : MonoBehaviour {

    private GameObject player;
    private PlayerController player_Controller;
    private bool is_Landing = false;


    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
    }

    private void LateUpdate() {
        if (player == null)
            return;

        if (is_Landing) {
            //自機が飛行状態になったとき地面から外す
            if (player_Controller.Get_Is_Ride_Beetle()) {
                player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                is_Landing = false;
                return;
            }
            //自機を回転させる
            Rotate_Player();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            Get_On_Player();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            Get_Off_Player();
        }
    }


    //自機が回転する地面に乗ったとき
    private void Get_On_Player() {
        player.transform.SetParent(transform);
        is_Landing = true;
    }


    //自機が回転する地面から離れた時
    private void Get_Off_Player() {
        player.transform.SetParent(null);
        player.transform.localScale = new Vector3(player.transform.localScale.x.CompareTo(0), 1, 1);
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
        is_Landing = false;
    }


    //自機を斜面に合わせて回転させる
    private void Rotate_Player() {
        AngleCalculater _angle = new AngleCalculater();
        float angle = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        player.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
