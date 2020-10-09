using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marisa : MonoBehaviour {

    [SerializeField] private ShootSystem green_Shoot;
    [SerializeField] private ShootSystem yellow_Shoot;
    private MoveConstTime _move;


    public void Start_Battle() {
        _move = GetComponent<MoveConstTime>();
        StartCoroutine("Battle_Cor");
    }


    private IEnumerator Battle_Cor() {
        while (true) {
            PlayerManager.Instance.Set_Life(9);

            _move.Start_Move(new Vector3(108f, 64f));
            yield return new WaitUntil(_move.End_Move);
            green_Shoot.Shoot();
            yield return new WaitForSeconds(1.0f);

            _move.Start_Move(new Vector3(172f, 16f));
            yield return new WaitUntil(_move.End_Move);
            yellow_Shoot.Shoot();
            yield return new WaitForSeconds(2.0f);
        }
    }
}
