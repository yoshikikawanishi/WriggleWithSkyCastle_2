using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//回転しながら拡大する
public class SpreadBombController : MonoBehaviour {


    [SerializeField] private float spread_Speed = 0;
    [SerializeField] private float default_Size = 0;
    [SerializeField] private float max_Size = 1;
    [SerializeField] private float life_Time = 0;
    [SerializeField] private float angler_Velocity = 0;
    [SerializeField] private bool in_Fixed_Update = true;

    private SpriteRenderer _sprite;


    // Use this for initialization
    void Start () {
        if (life_Time > 0) {
            Destroy(gameObject, life_Time);
        }
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        transform.localScale = new Vector3(1, 1, 1) * default_Size;    
    }

    //FixedUpdate
    private void FixedUpdate() {
        if (!in_Fixed_Update) {
            return;
        }
        if (transform.localScale.x < max_Size) {
            transform.localScale += new Vector3(spread_Speed, spread_Speed, 0);
        }
        transform.Rotate(0, 0, angler_Velocity * Time.timeScale);

        if (_sprite == null)
            return;
        if (life_Time > 0) {
            _sprite.color -= new Color(0, 0, 0, 1.1f / life_Time * Time.deltaTime);
        }
    }

    private void Update() {
        if (in_Fixed_Update) {
            return;
        }
        if (transform.localScale.x < max_Size) {
            transform.localScale += new Vector3(spread_Speed, spread_Speed, 0);
        }
        transform.Rotate(0, 0, angler_Velocity * Time.timeScale);

        if (_sprite == null)
            return;
        if (life_Time > 0) {
            _sprite.color -= new Color(0, 0, 0, 1.1f / life_Time * Time.deltaTime);
        }
    }
}
