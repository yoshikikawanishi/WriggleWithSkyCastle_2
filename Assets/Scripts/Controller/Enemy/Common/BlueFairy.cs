using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFairy : MonoBehaviour {

    
    private void OnEnable() {
        StartCoroutine("Action_Cor");
    }


    private IEnumerator Action_Cor() {
        yield return null;
        int vertical_Direction = transform.position.y > 0 ? 1 : -1;        
        //登場
        float speed = 3.5f;
        for (float t = 0; t < 1.5f; t += Time.deltaTime) {
            transform.position += new Vector3(-speed, -speed * vertical_Direction);
            //減速
            if (speed > 0.2f) {
                speed -= 0.05f;
            }
            yield return new WaitForSeconds(0.016f);
        }

        for(float t = 0; t < 1.5f; t += Time.deltaTime) {
            transform.position += new Vector3(-0.6f, -0.2f * vertical_Direction);
            yield return new WaitForSeconds(0.016f);
        }

        //はける
        float escape_Acc = 0.05f;
        Vector3 escape_Speed = new Vector3(-0.65f, 0);
        if (transform.position.y > 0) { escape_Acc = -0.05f; }
        while (Mathf.Abs(transform.position.y) < 210f) {
            transform.position += escape_Speed * Time.timeScale;
            escape_Speed += new Vector3(0, escape_Acc);
            yield return new WaitForSeconds(0.016f);
        }

        gameObject.SetActive(false);
    }
}
