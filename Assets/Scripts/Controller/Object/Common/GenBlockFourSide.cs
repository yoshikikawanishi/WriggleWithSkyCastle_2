using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//攻撃を喰らうとブロックを四方に生成する
public class GenBlockFourSide : MonoBehaviour {

    [SerializeField] private GameObject gen_Block;
    
    [SerializeField]
    private List<string> attacked_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
        "PlayerBulletTag",
        "PlayerChargeBulletTag",
    };

    [SerializeField] protected int max_Length = 5;
    [SerializeField] protected float block_Size = 32;

    protected GameObject[] blocks;
    private GameObject[] blocks_Copy;

    private bool can_Generate = true;


    // Use this for initialization
    void Start () {
        if (gen_Block == null)
            Destroy(gameObject);
        //ブロックの長さ最大値に対して配列のサイズを定義
        blocks = new GameObject[(int)Mathf.Pow((max_Length * 2 + 1), 2)];
        blocks_Copy = new GameObject[(int)Mathf.Pow((max_Length * 2 + 1), 2)];
    }


    // OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!can_Generate)
            return;

        foreach(string tag in attacked_Tags) {
            if(collision.tag == tag) {
                Attacked();
                can_Generate = false;
                Invoke("Enable_Generate", 0.2f);
            }
        }
    }


    void Enable_Generate() {
        can_Generate = true;
    }


    //攻撃を喰らったときの処理
    protected virtual void Attacked() {                
        //本体と存在するブロックからブロックを生成する
        for (int i = 0; i < blocks.Length; i++) {
            if (blocks_Copy[i] != null) {                
                Gen_Block(Get_Grid_By_Index(i));                
            }
        }
        Gen_Block(new Vector2Int(0, 0));

        Array.Copy(blocks, blocks_Copy, blocks.Length);        
    }
        

    //引数マス目から上下左右にブロックを生成する
    private void Gen_Block(Vector2Int center_Grid) {        
        //ブロックの生成、移動
        GameObject[] gen_Blocks = new GameObject[4];
        Vector2Int grid;
        for (int i = 0; i < 4; i++) {
            //生成先のマス目
            grid = center_Grid + Side_Grid(i);
            if (Mathf.Abs(grid.x) > max_Length || Mathf.Abs(grid.y) > max_Length)
                continue;            

            //生成先に存在しなければ           
            if(Get_Block_By_Grid(grid.x, grid.y) == null) {
                //生成
                gen_Blocks[i] = Instantiate(gen_Block);
                gen_Blocks[i].transform.position = Get_Pos_By_Grid(center_Grid.x, center_Grid.y);
                blocks[Get_Index_By_Grid(grid.x, grid.y)] = gen_Blocks[i];
                //移動
                StartCoroutine(Move_Block_Cor(gen_Blocks[i], Side_Grid(i)));
            }
        }
    }


    //中心を(0, 0)としたとき引数インデックスのマスを返す
    private Vector2Int Get_Grid_By_Index(int index) {
        int range = 2 * max_Length + 1;
        int center_Index = range * max_Length + max_Length;
        Vector2Int center_Grid = new Vector2Int(center_Index % range, center_Index / range);
        Vector2Int grid = new Vector2Int(index % range, index / range);

        return (grid - center_Grid);
    }


    //中心を(0, 0)としたときの引数マス目のインデックスを返す
    private int Get_Index_By_Grid(int x, int y) {
        int range = 2 * max_Length + 1;
        int center_Index = range * max_Length + max_Length;
        int index = range * y + x;

        return (center_Index + index);
    }


    //中心を(0, 0)としたときの引数マス目のブロックを返す
    private GameObject Get_Block_By_Grid(int x, int y) {         
        return blocks[Get_Index_By_Grid(x, y)];
    }


    //引数マス目の座標を返す
    private Vector2 Get_Pos_By_Grid(int x, int y) {
        Vector2 pos = transform.position + new Vector3(x * block_Size, y * block_Size);
        return pos;
    }


    //上下左右を0～3から返す
    private Vector2Int Side_Grid(int i) {
        switch (i) {
            case 0: return new Vector2Int(0, 1);
            case 1: return new Vector2Int(1, 0);
            case 2: return new Vector2Int(0, -1);
            case 3: return new Vector2Int(-1, 0);
        }

        return new Vector2Int(0, 0);
    }


    //ブロックの移動
    private IEnumerator Move_Block_Cor(GameObject block, Vector2Int direction) {
        MoveTwoPoints move = block.GetComponent<MoveTwoPoints>();
        if(move == null) {
            move = block.AddComponent<MoveTwoPoints>();
            move.Change_Transition_Curve(AnimationCurve.Linear(0, 0, 1, 1), 0);
            move.Change_Paramter(0.07f, 0, 0);
        }

        Vector3 d = new Vector3(direction.x, direction.y);
        Vector2 next_Pos = block.transform.position + d * block_Size;        
        move.Start_Move(next_Pos);

        //移動が終わるまで無敵化
        block.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitUntil(move.End_Move);
        block.layer = LayerMask.NameToLayer("Default");
    }   

}
