using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : GenBlockFourSide {

    //最上段、通常、最下段用の草ブロックのテクスチャーを入れる
    [SerializeField] private Sprite[] glass_Sprites = new Sprite[3];

   
    //一個上のマスにブロックが存在するかどうか
    private bool Is_Exist_Space_Above(int index) {
        int range = 2 * max_Length + 1;
        int under_Index = index + range;

        if (under_Index > blocks.Length - 1)
            return true;
        if (blocks[under_Index] == null)
            return true;

        return false;
    }


    //一個下のマスにブロックが存在するか
    private bool Is_Exist_Space_Under(int index) {        
        int range = 2 * max_Length + 1;
        int above_Index = index - range;

        if (above_Index < 0)
            return true;
        if (blocks[above_Index] == null)
            return true;

        return false;
    }


    //攻撃を受けた時の処理、ブロックを生成する
    //生成したブロックの内、最上部と最下部のブロックのテクスチャを変える
    protected override void Attacked() {
        base.Attacked();

        StartCoroutine("Blink_Cor");

        for(int i = 0; i < blocks.Length; i++) {
            if (blocks[i] == null)
                continue;
            if (Is_Exist_Space_Above(i)) {
                blocks[i].GetComponent<SpriteRenderer>().sprite = glass_Sprites[0];
            }
            else if (Is_Exist_Space_Under(i)) {
                blocks[i].GetComponent<SpriteRenderer>().sprite = glass_Sprites[2];
            }
            else {
                blocks[i].GetComponent<SpriteRenderer>().sprite = glass_Sprites[1];
            }
        }
    }


    //白く点滅
    private IEnumerator Blink_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(0.4f, 0.4f, 0.4f);
    }
}
