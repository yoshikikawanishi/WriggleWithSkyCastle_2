using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect;
    [SerializeField] private ParticleSystem power_Charge_Effect_Small;
    [SerializeField] private ParticleSystem burst_Effect_White;
    [SerializeField] private GameObject ban_Player_Flying_Effect_Obj;
    [SerializeField] private Animator disable_Flying_Screen_Effect;

    //=========================== Power Charge Effect ============================
    public void Play_Power_Charge_Effect(float span) {
        power_Charge_Effect.SetActive(true);
        Invoke("Stop_Power_Charge_Effect", span);
    }


    public void Stop_Power_Charge_Effect() {
        power_Charge_Effect.SetActive(false);
    }
    //=========================== Power Charge Effect Small ============================
    public void Play_Power_Charge_Effect_Small() {
        power_Charge_Effect_Small.Play();
    }
    //============================= Burst Effect ================================
    public void Play_Burst_Effect_White() {
        burst_Effect_White.Play();
    }
    //================================飛行不可エフェクト====================================
    public void Play_Ban_Flying_Effect() {
        ban_Player_Flying_Effect_Obj.GetComponent<AudioSource>().Play();
        ban_Player_Flying_Effect_Obj.GetComponent<ParticleSystem>().Play();
        disable_Flying_Screen_Effect.gameObject.SetActive(true);
        disable_Flying_Screen_Effect.SetTrigger("AppearTrigger");
    }


    public void Release_Ban_Flying_Effect() {
        disable_Flying_Screen_Effect.SetTrigger("DisappearTrigger");
        disable_Flying_Screen_Effect.gameObject.SetActive(false);
    }
}
