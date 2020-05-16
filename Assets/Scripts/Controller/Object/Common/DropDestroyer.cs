using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDestroyer : MonoBehaviour {
    [SerializeField] private bool is_Pooled;

    private void Update() {
        if(transform.position.y < -180f) {
            if (is_Pooled)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }
}
