﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect_Prefab;
    [SerializeField] private GameObject mai_Cross_Rushing_Effect_Right;
    [SerializeField] private GameObject mai_Cross_Rushing_Effect_Left;
    [SerializeField] private GameObject satono_Cross_Rushing_Effect;

    private List<GameObject> power_Charge_Effects = new List<GameObject>();

    
    public void Play_Power_Charge_Effect(Transform parent) {
        var effect = Instantiate(power_Charge_Effect_Prefab, parent);        
        effect.transform.localPosition = Vector3.zero;
        effect.SetActive(true);
        power_Charge_Effects.Add(effect);
    }


    public void Play_Power_Charge_Effect(Transform parent, float span) {
        Play_Power_Charge_Effect(parent);
        Invoke("Stop_Power_Charge_Effect", span);
    }


    public void Stop_Power_Charge_Effect() {
        foreach(GameObject e in power_Charge_Effects) {
            Destroy(e);
        }
        power_Charge_Effects.Clear();
    }


    public void Play_Mai_Cross_Rushing_Effect(int direction) {
        if (direction > 0)
            mai_Cross_Rushing_Effect_Right.SetActive(true);
        else
            mai_Cross_Rushing_Effect_Left.SetActive(true);
    }


    public void Stop_Mai_Cross_Rushing_Effect() {
        mai_Cross_Rushing_Effect_Right.SetActive(false);
        mai_Cross_Rushing_Effect_Left.SetActive(false);
    }


    public void Play_Satono_Cross_Rushing_Effect() {
        satono_Cross_Rushing_Effect.SetActive(true);
    }


    public void Stop_Satono_Cross_Rushing_Effect() {
        satono_Cross_Rushing_Effect.SetActive(false);
    }
}
