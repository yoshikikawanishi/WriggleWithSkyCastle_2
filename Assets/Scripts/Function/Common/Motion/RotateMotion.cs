using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMotion : MonoBehaviour {

    [SerializeField] private float rotate_Speed = 0.1f;
    [SerializeField] private bool is_Affected_Timescale = true;
	
	// Update is called once per frame
	void Update () {
        if (is_Affected_Timescale) 
            transform.Rotate(new Vector3(0, 0, rotate_Speed * Time.timeScale));
        else
            transform.Rotate(new Vector3(0, 0, rotate_Speed));
    }
}
