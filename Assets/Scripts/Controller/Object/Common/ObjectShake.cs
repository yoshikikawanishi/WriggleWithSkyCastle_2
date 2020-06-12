using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour {

    /// <summary>
    /// 揺らす
    /// </summary>
    /// <param name="duration">期間</param>
    /// <param name="magnitude">強さ</param>
    /// <param name="fix_Position">揺らしはじめと終わりの座標をそろえるか</param>
    public void Shake(float duration, Vector2 magnitude, bool fix_Position) {
        StartCoroutine(DoShake(duration, magnitude, fix_Position));
    }

    //揺らす
    private IEnumerator DoShake(float duration, Vector2 magnitude, bool fix_Position) {

        var pos = transform.position;
        var elapsed = 0f;

        while (elapsed < duration) {
            var x = Random.Range(-1f, 1f) * magnitude.x;
            var y = Random.Range(0, 1f) * magnitude.y;
            if (transform.position.y > 0)
                y = -y;

            transform.localPosition = transform.position + new Vector3(x, y) * Time.timeScale;

            elapsed += Time.deltaTime;
            yield return null;

            if (fix_Position)
                transform.position = pos;
        }        
    }
}
