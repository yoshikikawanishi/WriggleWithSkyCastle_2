using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiYinball : MonoBehaviour {

    private Renderer _renderer;
    private ShootSystem _shoot;
    private float time = 0;
    private float shoot_Span = 0.5f;
    private float shoot_Probability = 5;
	

	void Start () {
        _renderer = GetComponent<Renderer>();
        _shoot = GetComponentInChildren<ShootSystem>();	
	}
	
	
	void Update () {
        if (!_renderer.isVisible) {
            return;
        }
		if(time < shoot_Span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            if(Random.Range(0, 100) < shoot_Probability) {
                _shoot.Shoot();
            }
        }
	}
}
