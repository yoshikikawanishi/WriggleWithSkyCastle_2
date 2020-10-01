using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect;
    [SerializeField] private ParticleSystem power_Charge_Effect_Small;

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
}
