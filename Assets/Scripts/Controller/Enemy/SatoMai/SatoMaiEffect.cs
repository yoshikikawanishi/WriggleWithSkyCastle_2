using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiEffect : MonoBehaviour {

    [SerializeField] private GameObject power_Charge_Effect_Prefab;

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
}
