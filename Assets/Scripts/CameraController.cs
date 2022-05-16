using UnityEngine;

namespace Picking
{
    // カメラの振動を管理します。
    public sealed class CameraController : MonoBehaviour
    {
        public void SetVibrate(float left, float right)
        {
            this.left = left;
            this.right = right;
        }

        private float left;
        private float right;


        void Update()
        {
            // カメラ振動はとりあえず無し
        }
    }
}
