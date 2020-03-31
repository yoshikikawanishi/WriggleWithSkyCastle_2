using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//点滅と消滅
public class BlinkAndDestroy : MonoBehaviour {

    [SerializeField] private float start_Blink_Time = 10.0f;
    [SerializeField] private bool is_Pooled_Object = false;


    private void Start() {
        StartCoroutine("Delete_Cor");
    }

    
    private IEnumerator Delete_Cor() {

        Renderer _renderer = GetComponent<Renderer>();
        yield return new WaitForSeconds(start_Blink_Time);
       
        for (float t = 0.2f; t > 0.05f; t *= 0.9f) {
            _renderer.enabled = false;
            yield return new WaitForSeconds(t);
            _renderer.enabled = true;
            yield return new WaitForSeconds(t);
        }

        if (is_Pooled_Object) {
            gameObject.SetActive(false);
        }
        else {
            Destroy(gameObject);
        }
    }

}
