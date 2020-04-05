using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AunnEffect : MonoBehaviour {

    [SerializeField] private ParticleSystem a_Letter_Effect;
    [SerializeField] private ParticleSystem unn_Letter_Effect;


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


}
