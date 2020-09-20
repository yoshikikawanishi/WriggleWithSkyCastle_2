using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect_Obj;
    [SerializeField] private ParticleSystem small_Power_Charge_Effect;
    [SerializeField] private ParticleSystem burst_Effect_Green;
    [SerializeField] private GameObject ban_Player_Flying_Effect_Obj;
    [SerializeField] private Animator disable_Flying_Screen_Effect;


    public void Play_Power_Charge_Effect() {
        power_Charge_Effect_Obj.SetActive(true);
    }

    public void Play_Power_Charge_Effect(float span) {
        power_Charge_Effect_Obj.SetActive(true);
        Invoke("Stop_Power_Charge_Effect", span);
    }

    public void Stop_Power_Charge_Effect() {
        power_Charge_Effect_Obj.SetActive(false);
    }


    public void Play_Small_Power_Charge_Effect() {
        small_Power_Charge_Effect.Play();
    }


    public void Play_Burst_Effect_Green() {
        burst_Effect_Green.Play();
    }


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
