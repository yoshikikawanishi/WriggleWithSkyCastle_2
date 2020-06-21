using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiEffects : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect;
    [SerializeField] private GameObject power_Charge_Effect_Red;
    [SerializeField] private ParticleSystem power_Charge_Small_Effect;
    [SerializeField] private ParticleSystem burst_Effect_Blue;
    [SerializeField] private ParticleSystem burst_Effect_Red;
    [SerializeField] private GameObject yellow_Circle_Effect;


       
    public void Play_Power_Charge(float span) {
        power_Charge_Effect.SetActive(true);
        if(span > 0) {
            Invoke("Stop_Power_Charge", span);
        }
    }

    public void Stop_Power_Charge() {
        power_Charge_Effect.SetActive(false);
    }


    public void Play_Power_Charge_Red(float span) {
        power_Charge_Effect_Red.SetActive(true);
        if (span > 0) {
            Invoke("Stop_Power_Charge_Red", span);
        }
    }

    public void Stop_Power_Charge_Red() {
        power_Charge_Effect_Red.SetActive(false);
    }


    public void Play_Power_Charge_Small() {
        power_Charge_Small_Effect.Play();
    }


    public void Play_Burst() {
        burst_Effect_Blue.Play();
    }


    public void Play_Burst_Red() {
        burst_Effect_Red.Play();
    }


    public void Play_Yellow_Circle() {
        var effect = Instantiate(yellow_Circle_Effect);
        effect.transform.position = transform.position;
        effect.SetActive(true);
    }
}
