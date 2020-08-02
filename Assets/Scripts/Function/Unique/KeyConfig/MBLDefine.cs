using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UnitTest")]

/// <summary>
/// このゲームで定義する定数などを扱う
/// </summary>
namespace MBLDefine {

    /// <summary>
    /// 入力値の基底クラス
    /// </summary>
    internal class InputValue {
        public readonly string String;

        protected InputValue(string name) {
            String = name;
        }
    }

    /// <summary>
    /// 使用するキーを表すクラス
    /// </summary>
    internal sealed class Key : InputValue {
        public readonly List<KeyCode> DefaultKeyCode;
        public readonly static List<Key> AllKeyData = new List<Key>();

        private Key(string keyName, List<KeyCode> defaultKeyCode)
            : base(keyName) {
            DefaultKeyCode = defaultKeyCode;
            AllKeyData.Add(this);
        }

        public override string ToString() {
            return String;
        }

        public static readonly Key Jump     = new Key("Jump",   new List<KeyCode> { KeyCode.Z, KeyCode.Joystick1Button3 });
        public static readonly Key Attack   = new Key("Attack", new List<KeyCode> { KeyCode.X , KeyCode.Joystick1Button1 });
        public static readonly Key Fly      = new Key("Fly",    new List<KeyCode> { KeyCode.C , KeyCode.Joystick1Button5 });
        public static readonly Key Slow     = new Key("Slow",   new List<KeyCode> { KeyCode.LeftShift, KeyCode.Joystick1Button6 });
        public static readonly Key Shoot    = new Key("Shoot",  new List<KeyCode> { KeyCode.Z, KeyCode.Joystick1Button0 });
        public static readonly Key Pause    = new Key("Pause",  new List<KeyCode> { KeyCode.Escape , KeyCode.Joystick1Button11 });
    }

    /// <summary>
    /// 使用する軸入力を表すクラス
    /// </summary>
    internal sealed class Axes : InputValue {
        public readonly static List<Axes> AllAxesData = new List<Axes>();

        private Axes(string axesName)
            : base(axesName) {
            AllAxesData.Add(this);
        }

        public override string ToString() {
            return String;
        }

        public static Axes Horizontal = new Axes("Horizontal");
        public static Axes Vertical = new Axes("Vertical");
    }

}