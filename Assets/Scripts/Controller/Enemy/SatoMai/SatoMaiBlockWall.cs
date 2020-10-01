using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoMaiBlockWall : MonoBehaviour {

    public enum Kind {
        kick = 0,
        shoot = 1,
        charge_Kick = 2,
    }

    [SerializeField] private GameObject kick_Crash_Block;
    [SerializeField] private GameObject shoot_Crash_Block;
    [SerializeField] private GameObject charge_Kick_Crash_Block;

    private readonly float block_Size = 32f;
    private readonly int block_Num = 9;
    private readonly float bottom_Height = -128f;
    private readonly float center_Pos = 16f;

    private float move_Speed = 2f;

    private CameraShake camera_Shake;


    void Start() {
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(kick_Crash_Block, block_Num);
        ObjectPoolManager.Instance.Create_New_Pool(shoot_Crash_Block, block_Num);
        ObjectPoolManager.Instance.Create_New_Pool(charge_Kick_Crash_Block, block_Num);
        //取得
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }


    //is_Right == tureで右に生成し、左に移動
    public void Generate_And_Move(bool is_Right, Kind kind) {
        float pos = is_Right ? 260f : -260f;
        int direction = is_Right ? -1 : 1;
        StartCoroutine(Move_Blocks(Generate(pos, kind), direction));
    }


    //pos_Xにkindのブロックを盾一直線に生成
    private GameObject[] Generate(float pos_X, Kind kind) {
        //生成するブロックを選択
        GameObject block_Prefab;
        switch (kind) {
            case Kind.kick: block_Prefab = kick_Crash_Block; break;
            case Kind.shoot: block_Prefab = shoot_Crash_Block; break;
            default: block_Prefab = charge_Kick_Crash_Block; break;
        }
        //生成
        GameObject[] blocks = new GameObject[block_Num];
        for(int i = 0; i < block_Num; i++) {
            GameObject block = ObjectPoolManager.Instance.Get_Pool(block_Prefab).GetObject();
            block.transform.position = new Vector3(pos_X, bottom_Height + block_Size * i);
            blocks[i] = block;
        }
        return blocks;
    }


    //direction == 1で右方向に移動
    private IEnumerator Move_Blocks(GameObject[] blocks, int direction) {

        while (true) {
            //真ん中に着いたとき抜ける
            if(Mathf.Abs(blocks[0].transform.position.x) < center_Pos) {
                break;
            }
            //移動
            foreach(var block in blocks) {
                block.transform.position += new Vector3(move_Speed * direction, 0, 0);
            }
            yield return new WaitForSeconds(0.016f);
        }
        //真ん中に着いた
        camera_Shake.Shake(0.5f, new Vector2(1f, 1f), true);
        yield return new WaitForSeconds(1.0f);

        //ブロックを消す
        foreach(var block in blocks) {
            CrashBlockController c = block.GetComponent<CrashBlockController>();
            if (c == null)
                block.SetActive(false);
            else
                c.Crash();
        }
    }


    
}
