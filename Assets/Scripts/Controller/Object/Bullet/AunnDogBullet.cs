using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// change_Color_Time秒後に画像と、軌道が変わる弾
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class AunnDogBullet : MonoBehaviour {    

    [SerializeField] private Sprite first_Sprite;
    [SerializeField] private Sprite latter_Sprite;
    [Space]
    [SerializeField] private float change_Color_Time = 3.5f;
    [SerializeField] private float speed = 70f;
    [SerializeField] private float rotate_Range = 30f;

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigid;


    void Awake() {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }


    private void OnEnable() {       
        _sprite.sprite = first_Sprite;
        StopAllCoroutines();
        StartCoroutine("Change_Sprite_And_Curve_Cor");
    }


    private IEnumerator Change_Sprite_And_Curve_Cor() {
        yield return new WaitForSeconds(change_Color_Time - 0.5f);
        GetComponent<ParticleSystem>().Play();
        _sprite.sprite = null;
        yield return new WaitForSeconds(0.5f);

        _sprite.sprite = latter_Sprite;
        //軌道の変更
        float angle = Random.Range(-rotate_Range, rotate_Range);
        transform.Rotate(new Vector3(0, 0, angle));
        _rigid.velocity = transform.right * speed;
    }
}
