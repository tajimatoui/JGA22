using UnityEngine;

namespace Picking
{
    // 鍵の基底クラスです。
    public abstract class LockBase : MonoBehaviour
    {
        public abstract bool Unlocked { get; }

        public virtual Director Director { protected get; set; }

        // 手の位置から開錠を試みます。
        public abstract void TryUnlock(Vector2 left, Vector2 right);

        public virtual Vector2 AdjustLeft(Vector2 left)
        {
            return left;
        }
        public virtual Vector2 AdjustRight(Vector2 right)
        {
            return right;
        }
    }
}
