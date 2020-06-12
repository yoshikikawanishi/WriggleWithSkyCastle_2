using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    public List<string> deleted_Obj_Tag = new List<string> {
        "PlayerBodyTag",
        "BombTag",
        "GroundTag",
        "SandbackGroundTag"
    };
    public bool is_Delete_Invisible = true;


	public void Set_Inactive(float lifeTime) {
        StartCoroutine("Set_Inactive_Routine", lifeTime);
    }

    private IEnumerator Set_Inactive_Routine(float lifeTime) {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "BombTag") {
            PlayerManager.Instance.Add_Score(2);
        }
        foreach (string tag in deleted_Obj_Tag) {
            if (collision.tag == tag) {
                gameObject.SetActive(false);
            }
        }
    }


    private void OnBecameInvisible() {
        if (is_Delete_Invisible) 
            gameObject.SetActive(false);
    }

}
