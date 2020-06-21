﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour {


    private void Update() {
        if(transform.position.y < -180f) {
            gameObject.SetActive(false);
        }
    }

    private void OnBecameVisible() {
        StartCoroutine("Shoot_Cor");         
    }

    private void OnBecameInvisible() {
        StopCoroutine("Shoot_Cor");
    }

    private IEnumerator Shoot_Cor() {
        while (true) {
            yield return new WaitForSeconds(2.0f);
            GetComponent<Animator>().SetTrigger("ShootTrigger");
            yield return new WaitForSeconds(0.4f);
            GetComponentInChildren<ShootSystem>().Shoot();
            yield return new WaitForSeconds(4.0f);
        }
    }
}
