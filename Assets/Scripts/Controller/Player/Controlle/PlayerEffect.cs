using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour {

    
    [SerializeField] private GameObject[] shoot_Charge = new GameObject[3];
    [SerializeField] private ParticleSystem green_Powder;
    [SerializeField] private ParticleSystem red_Powder;
    [SerializeField] private ParticleSystem ride_Beetle;
    [SerializeField] private ParticleSystem dark_Powder;
    [SerializeField] private GameObject jump_Effect;
    [SerializeField] private ParticleSystem charge_Kick_Charge_Effect;

    private ParticleSystem[] shoot_Charge_Particle = new ParticleSystem[3];


    private void Start() {
        //取得
        for (int i = 0; i < 3; i++) {
            shoot_Charge_Particle[i] = shoot_Charge[i].GetComponent<ParticleSystem>();
        }
       
    }

    /// <summary>
    /// チャージショット用エフェクト
    /// </summary>
    /// <param name="phase">チャージ段階(１～３段階)それ以外の場合エフェクト止める</param>
    public void Start_Shoot_Charge(int phase) {        
        for(int i = 0; i < 3; i++) {
            if(i + 1 == phase) {
                shoot_Charge_Particle[i].Play();
            }
            else {
                shoot_Charge_Particle[i].Stop();
            }
        }
    }


    /// <summary>
    /// 緑パウダーエフェクトを再生する
    /// </summary>
    public void Play_Green_Powder_Effect() {
        if(green_Powder != null)
            green_Powder.Play();
    }


    /// <summary>
    /// 赤パウダーエフェクトを再生する
    /// </summary>
    public void Play_Red_Powder_Effect() {
        if(red_Powder != null)
            red_Powder.Play();
    }

    /// <summary>
    /// 黒パウダーエフェクトを再生する
    /// </summary>
    public void Play_Dark_Powder_Effect() {
        if(dark_Powder != null)
            dark_Powder.Play();
    }

    /// <summary>
    /// カブトムシ乗り時の収束エフェクトを再生を開始する
    /// </summary>
    public void Start_Ridding_Beetle_Effect() {
        ride_Beetle.gameObject.SetActive(true);
        StartCoroutine("Play_Ridding_Beetle_Effect");        
    }

    private IEnumerator Play_Ridding_Beetle_Effect() {
        ride_Beetle.Play();
        while (true) {
            ride_Beetle.Simulate(
                t: Time.unscaledDeltaTime,  //パーティクルシステムを早送りする時間
                withChildren: true,         //子のパーティクルシステムもすべて早送りするかどうか
                restart: false              //再起動し最初から再生するかどうか
            );
            yield return null;
        }
    }

    /// <summary>
    /// カブトムシの理事の収束エフェクトを止める
    /// </summary>
    public void Stop_Ridding_Beetle_Effect() {
        ride_Beetle.Stop();
        StopCoroutine("Play_Ridding_Beetle_Effect");
        ride_Beetle.gameObject.SetActive(false);
    }


    /// <summary>
    /// ジャンプエフェクトを出す
    /// </summary>
    public void Play_Jump_Effect() {
        var effect = Instantiate(jump_Effect);
        effect.transform.position = transform.position + new Vector3(0, -8f);
        effect.SetActive(true);
        Destroy(effect, 1.0f);
    }


    /// <summary>
    /// チャージキックのチャージエフェクト
    /// </summary>
    public void Play_Charge_Kick_Charge_Effect() {
        charge_Kick_Charge_Effect.gameObject.SetActive(true);
        charge_Kick_Charge_Effect.Play();
    }

    public void Stop_Charge_Kick_Charge_Effect() {
        charge_Kick_Charge_Effect.gameObject.SetActive(false);        
    }


    /// <summary>
    /// チャージキックのフルチャージ時の点滅開始
    /// </summary>
    public void Start_Full_Charge_Blink() {
        StartCoroutine("Full_Charge_Blink_Cor");
    }


    public void Quit_Full_Charge_Blink() {
        StopCoroutine("Full_Charge_Blink_Cor");
        SpriteRenderer _sprite = transform.parent.GetComponent<SpriteRenderer>();
        _sprite.color = new Color(0.5f, 0.5f, 0.5f, _sprite.color.a);
    }


    private IEnumerator Full_Charge_Blink_Cor() {
        SpriteRenderer _sprite = transform.parent.GetComponent<SpriteRenderer>();
        while (true) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f, _sprite.color.a);
            yield return new WaitForSeconds(0.2f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, _sprite.color.a);
            yield return new WaitForSeconds(0.2f);
        }
    }    
}
