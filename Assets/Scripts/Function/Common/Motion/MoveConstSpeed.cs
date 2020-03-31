using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveConstSpeed : MonoBehaviour {

    [System.Serializable]
    public class Parameter {
        public string comment;  
        public float speed = 0.1f;        
        public bool is_Local_Position = false;

        public Parameter() {

        }
    }

    [SerializeField] private List<Parameter> param = new List<Parameter> { new Parameter() };

    private int index = 0;
    private bool end_Move = true;


    public void Start_Move(Vector3 next_Pos) {
        end_Move = false;
        this.index = 0;
        StopAllCoroutines();
        StartCoroutine("Move_Cor", next_Pos);
    }


    public void Start_Move(Vector3 next_Pos, int index) {
        end_Move = false;
        this.index = index;
        StopAllCoroutines();
        StartCoroutine("Move_Cor", next_Pos);
    }


    public void Stop_Move() {
        StopAllCoroutines();
        end_Move = true;
    }


    public bool End_Move() {
        if (end_Move) {
            end_Move = false;
            return true;
        }
        return false;
    }


    private IEnumerator Move_Cor(Vector3 next_Pos) {
        float location = 0;
        Vector3 direction;
        float distance;

        //ローカル座標の場合
        if (param[index].is_Local_Position) {
            direction   = (next_Pos - transform.localPosition).normalized;
            distance    = Vector2.Distance(next_Pos, transform.localPosition);

            while (location < distance) {
                transform.localPosition += direction * param[index].speed;
                location += param[index].speed;
                yield return new WaitForSeconds(0.016f);
            }
            transform.localPosition = next_Pos;
        }

        //ワールド座標の場合
        else {
            direction   = (next_Pos - transform.position).normalized;
            distance    = Vector2.Distance(next_Pos, transform.position);

            while (location < distance) {
                transform.position += direction * param[index].speed;
                location += param[index].speed;
                yield return new WaitForSeconds(0.016f);
            }
            transform.position = next_Pos;
        }


        end_Move = true;        
    }

}
