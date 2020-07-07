using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JizoBullet : MonoBehaviour {

    private Rigidbody2D _rigid;
    private ChildColliderTrigger foot_Collision;
    private CameraShake camera_Shake;

    private ParticleSystem appear_Effect;
    private GameObject landing_Effect;

    private const float DROPPING_POWER = 32f;
    private bool is_Landing = false;


    void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        foot_Collision = transform.Find("FootCollision").GetComponent<ChildColliderTrigger>();
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

        appear_Effect = transform.Find("AppearEffect").GetComponent<ParticleSystem>();
        landing_Effect = transform.Find("LandingEffect").gameObject;
    }


    void OnEnable() {
        is_Landing = false;
        StartCoroutine("Play_Appear_Processe_Cor");
    }


    void Update() {
        if (foot_Collision.Hit_Trigger() && !is_Landing) {
            is_Landing = true;
            Play_Landing_Effect();
        }        
        else if(!foot_Collision.Hit_Trigger() && is_Landing) {
            is_Landing = false;
        }
    }


    private IEnumerator Play_Appear_Processe_Cor() {
        _rigid.gravityScale = 0;
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        appear_Effect.Play();
        yield return new WaitForSeconds(0.5f);
        _rigid.gravityScale = DROPPING_POWER;
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }


    private void Play_Landing_Effect() {
        camera_Shake.Shake(0.2f, new Vector2(1, 1), false);
        GameObject effect = Instantiate(landing_Effect);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        Destroy(effect, 2.0f);
    }

}
