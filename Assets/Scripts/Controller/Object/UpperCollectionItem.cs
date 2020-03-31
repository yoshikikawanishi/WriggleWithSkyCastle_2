using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravitatePlayer))]
public class UpperCollectionItem : MonoBehaviour {

    private GameObject player;
    private GravitatePlayer _gravitate;

    private void Start() {
        player = GameObject.FindWithTag("PlayerTag");
        _gravitate = GetComponent<GravitatePlayer>();
    }

    private void OnEnable() {
        _gravitate = GetComponent<GravitatePlayer>();
        _gravitate.DISTANCE_BORDER = 60f;
        _gravitate.GRAVITATE_POWER = 250f;
    }

    private void Update() {
        if(player == null) {
            return;
        }
        if(player.transform.position.y > 80f) {
            _gravitate.DISTANCE_BORDER = 500f;
            _gravitate.GRAVITATE_POWER = 500f;
        }
    }
}
