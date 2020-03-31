using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalVibeMotion : MonoBehaviour {

    [SerializeField] private float amplitude = 16f;
    [SerializeField] private float angular_Speed = 10f;
    [SerializeField] private float start_Angular = 0;

    private float angle = 0;
    private float center_Pos;

    private bool is_First_Frame = true;


    private void OnEnable() {
        is_First_Frame = true;
    }


    // Update is called once per frame
    void FixedUpdate () {
        if (is_First_Frame) {
            angle = start_Angular;
            center_Pos = transform.position.y;
            is_First_Frame = false;
        }

        transform.position = new Vector3(transform.position.x, center_Pos + Mathf.Sin(Mathf.Deg2Rad * angle) * amplitude);
        angle = (angle + angular_Speed * Time.timeScale) % 360f;
	}
}
