using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenYinBall2 : MonoBehaviour {

    float time = 0.1f;

    private void OnEnable() {
        time = 0.1f;
    }

    // Update is called once per frame
    void Update () {
		if(0 < time && time < 1.5f) {
            time += Time.deltaTime;
        }
        else if(0 < time) {
            time = -1;
            if (Random.Range(0, 100) < 25) {
                GetComponent<ShootSystem>().Shoot();
                UsualSoundManager.Instance.Play_Shoot_Sound(0.05f);
            }
        }
	}
}
