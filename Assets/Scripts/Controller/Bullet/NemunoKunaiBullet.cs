using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoKunaiBullet : MonoBehaviour {

    private void OnEnable() {
        StopCoroutine("Shoot_Cor");
        StartCoroutine("Shoot_Cor");
    }


    private IEnumerator Shoot_Cor() {
        float r = Random.Range(0f, 1.0f);
        if(r < 0.75f) {
            yield break;
        }
        yield return new WaitForSeconds(r*r);
        GetComponent<ShootSystem>().Shoot();
    }
}
