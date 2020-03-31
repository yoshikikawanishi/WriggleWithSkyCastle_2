using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFooting : MonoBehaviour {

    private Rigidbody2D player_Rigid;

    private bool is_Collasping = false;


    private void Start() {
        player_Rigid = GameObject.FindWithTag("PlayerTag").GetComponent<Rigidbody2D>();    
    }


    private void OnEnable() {
        is_Collasping = false;
    }


    //自機に踏まれると消滅する
    private void OnTriggerEnter2D(Collider2D collision) {
        if (is_Collasping) {
            return;
        }
        if(collision.tag == "PlayerFootTag" && player_Rigid.velocity.y < 10f) {
            StartCoroutine("Collaspe_Cor");
            is_Collasping = true;
        }
    }


    //点滅して消滅する
    private IEnumerator Collaspe_Cor() {        
        Renderer _renderer = GetComponent<Renderer>();

        yield return new WaitForSeconds(0.2f);

        float span = 0.2f;
        for(int i = 0; i < 6; i++) {
            _renderer.enabled = false;
            yield return new WaitForSeconds(span);
            _renderer.enabled = true;
            yield return new WaitForSeconds(span);
            span *= 0.5f;
        }

        gameObject.SetActive(false);
    }
}
