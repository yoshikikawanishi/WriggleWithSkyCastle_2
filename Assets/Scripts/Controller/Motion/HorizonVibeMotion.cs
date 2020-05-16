using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonVibeMotion : MonoBehaviour {

    [SerializeField] private float amplitude = 16f;
    [SerializeField] private float angular_Speed = 10f;
    [SerializeField] private float start_Angular = 0;

    private float angle = 0;
    private float center_Pos;

    // Use this for initialization
    void Start () {
        angle = start_Angular;
        center_Pos = transform.position.x;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = new Vector3(center_Pos + Mathf.Sin(Mathf.Deg2Rad * angle) * amplitude, transform.position.y);
        angle = (angle + angular_Speed * Time.timeScale) % 360f;
    }
}
