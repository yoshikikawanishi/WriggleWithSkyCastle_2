using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiEffects : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect;
    [SerializeField] private ParticleSystem power_Charge_Small_Effect;
    [SerializeField] private ParticleSystem burst_Effect;
       
    public void Play_Power_Charge(float span) {
        power_Charge_Effect.SetActive(true);
        if(span > 0) {
            Invoke("Stop_Power_Charge", span);
        }
    }

    public void Stop_Power_Charge() {
        power_Charge_Effect.SetActive(false);
    }


    public void Play_Power_Charge_Small() {
        power_Charge_Small_Effect.Play();
    }


    public void Play_Burst() {
        burst_Effect.Play();
    }
}
