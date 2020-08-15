using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnDogBulletBig : MonoBehaviour {
    
    [SerializeField] private float burst_Time = 3.8f;


    private void OnEnable() {
        StartCoroutine("Shoot_Cor");
    }


    private IEnumerator Shoot_Cor() {
        yield return new WaitForSeconds(burst_Time);
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        GetComponent<ShootSystem>().Shoot();        
        gameObject.SetActive(false);
    }
}
