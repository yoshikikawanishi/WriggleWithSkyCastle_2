using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullFairy : FairyEnemy {

    private GameObject skull;    
    private Rigidbody2D _rigid;


    private void Start() {      
        skull = transform.GetChild(0).gameObject;
        _rigid = GetComponent<Rigidbody2D>();
    }


    //消滅時の処理
    public override void Vanish() {
        //骨を出す
        skull.SetActive(true);
        skull.transform.SetParent(null);
        skull.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100f);

        base.Vanish();

        //４秒後に復活
        Invoke("Revive", 4.0f);
    }
    

    //復活
    private void Revive() {
        if(gameObject == null) {
            return;
        }
        gameObject.SetActive(true);
        StartCoroutine("Revive_Cor");
    }


    private IEnumerator Revive_Cor() {        
        Change_Exist_Component(false, false, false);

        //skull.GetComponent<ParticleSystem>().Play();
        //yield return new WaitForSeconds(1.0f);

        transform.position = skull.transform.position + new Vector3(-1.5f, 3f);
        Change_Exist_Component(true, false, false);        

        //ひまわりを戻す
        skull.transform.SetParent(transform);
        skull.SetActive(false);

        //点滅
        for(int i = 0; i < 4; i++) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);
        }

        Change_Exist_Component(true, true, true);
    }


    //RendererとColliderとrigidbodyの変更
    private void Change_Exist_Component(bool renderer, bool collider, bool rigid) {
        GetComponent<Renderer>().enabled = renderer;
        GetComponent<CapsuleCollider2D>().enabled = collider;
        _rigid.simulated = rigid;
    }

}
