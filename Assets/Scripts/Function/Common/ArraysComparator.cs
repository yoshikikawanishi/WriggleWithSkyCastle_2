using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraysComparator {


    // 今回はbool配列の比較だけ
    static public bool Is_Equals(bool[] a1, bool[] a2) {
        if (a1.Length != a2.Length)
            return false;
        int size = a1.Length;
        for (int i = 0; i < size; i++) {
            if (a1[i] != a2[i]) {
                return false;
            }
        }
        return true;
    }
}
