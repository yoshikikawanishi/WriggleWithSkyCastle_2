using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaring : MonoBehaviour {

    [SerializeField] private GameObject roaring_Effect;

    private GameObject player;
    private Rigidbody2D player_Rigid;


    private void Start() {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        player_Rigid = player.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 咆哮
    /// </summary>
    /// <param name="radius">自機をはじく範囲</param>
    /// <param name="duration">咆哮の期間</param>
    /// <param name="power">自機をはじく強さ(AddForce)</param>
    public void Roar(float radius, float duration, float power) {
        //自機をはじく
        StartCoroutine(Reject_Player_Cor(radius, duration, power));
        //パーティクルエフェクト
        var effect = Instantiate(roaring_Effect);
        effect.transform.position = transform.position;
        Destroy(effect, duration);
        //カメラ揺らす
        GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>().Shake(duration - 0.5f, new Vector2(2f, 2f), true);
    }

    public void Stop_Roaring() {
        StopCoroutine(Reject_Player_Cor(0, 0, 0));
    }


    //範囲radius内の自機をpowerではじく
    private IEnumerator Reject_Player_Cor(float radius, float duration, float power) {
        Vector2 vector;
        for (float t = 0; t < duration; t += Time.deltaTime*2) {
            if(Vector2.Distance(player.transform.position, transform.position) < radius) {
                vector = (player.transform.position - transform.position).normalized;
                player_Rigid.AddForce(vector * power);                                
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
}
