using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnEffect : MonoBehaviour {

    [SerializeField] private ParticleSystem a_Letter_Effect;
    [SerializeField] private ParticleSystem unn_Letter_Effect;
    [SerializeField] private GameObject power_Charge_Effect;
    [SerializeField] private GameObject jump_Effect;
    [SerializeField] private GameObject yellow_Circle_Effect;
    [SerializeField] private GameObject purple_Circle_Effect;
    [SerializeField] private ParticleSystem burst_Effect_Red;


	public void Play_Battle_Effect() {

    }

    public void Delete_Battle_Effect() {

    }


    public void Play_A_Letter_Effect() {
        a_Letter_Effect.Play();
    }
    

    public void Play_Unn_Letter_Effect() {
        unn_Letter_Effect.Play();
    }


    public void Start_Charge_Effect(float span) {
        power_Charge_Effect.SetActive(true);
        Invoke("Stop_Charge_Effect", span);
    }

    public void Stop_Charge_Effect() {
        power_Charge_Effect.SetActive(false);
    }


    public void Jump_And_Landing_Effect() {
        GameObject effect = Instantiate(jump_Effect);
        effect.SetActive(true);
        effect.transform.position = jump_Effect.transform.position;
        Destroy(effect, 1.0f);
    }


    public void Play_Yellow_Circle_Effect() {
        GameObject effect = Instantiate(yellow_Circle_Effect);
        effect.SetActive(true);
        effect.transform.position = yellow_Circle_Effect.transform.position;        
    }


    public void Play_Purple_Circle_Effect() {
        GameObject effect = Instantiate(purple_Circle_Effect);
        effect.SetActive(true);
        effect.transform.position = purple_Circle_Effect.transform.position;
    }


    public void Play_Burst_Effect_Red() {
        burst_Effect_Red.Play();
    }
}
