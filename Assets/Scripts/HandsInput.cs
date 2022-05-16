using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Picking
{
    // 手の入力を行います。
    public class HandsInput : MonoBehaviour
    {
        public Action<Vector2> LeftSetter { private get; set; } = null;
        public Action<Vector2> RightSetter { private get; set; } = null;


        void Update()
        {
            if (Gamepad.current != null)
            {
                LeftSetter?.Invoke(Gamepad.current.leftStick.ReadValue());
                RightSetter?.Invoke(Gamepad.current.rightStick.ReadValue());
            }
        }
    }
}
