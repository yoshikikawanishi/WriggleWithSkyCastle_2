using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaCameraFrame : MonoBehaviour {

    private Animator _anim;
    private SEManager _se;
    private GameObject player;
    

    private void Awake() {
        _anim = GetComponent<Animator>();
        _se = GetComponentInChildren<SEManager>();
        player = GameObject.FindWithTag("PlayerTag");
    }   
	

    public void Appear() {
        _anim.SetTrigger("AppearTrigger");
    }

    public void Disappear() {
        _anim.SetTrigger("DisappearTrigger");
    }


    public void Attack() {
        StartCoroutine("Attack_Cor");
    }

    private IEnumerator Attack_Cor() {
        _anim.SetTrigger("ConvergeTrigger");
        _se.Play("Charge");
        Vector3 direction;

        for(float t = 0; t < 1.85f; t += Time.deltaTime) {
            if (player == null)
                yield break;
            direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * 0.8f * Time.timeScale;
            yield return null;
        }
        _se.Play("Shutter1");
        yield return new WaitForSeconds(0.1f);
        _se.Play("Shutter2");

        yield return new WaitForSeconds(0.6f);
        transform.localPosition = new Vector3(0, 0, 10);        

    }
	
}
