using System;
using UnityEngine;
    
namespace Picking
{
    // 手の情報を保持します。
    public sealed class HandsData : MonoBehaviour
    {
        public Vector2 Left
        {
            get => left;
            set
            {
                left = AdjustLeft != null ? AdjustLeft(value) : value;
                if (left.sqrMagnitude > 1.0f)
                    left.Normalize();
            }
        }
        public Vector2 Right
        {
            get => right;
            set
            {
                right = AdjustRight != null ? AdjustRight(value) : value;
                if (right.sqrMagnitude > 1.0f)
                    right.Normalize();
            }
        }

        // 各値に対して補正を掛ける関数
        public Func<Vector2, Vector2> AdjustLeft { private get; set; } = null;
        public Func<Vector2, Vector2> AdjustRight { private get; set; } = null;


        [Header("確認用")]
        [SerializeField]
        private Vector2 left = new Vector2();
        [SerializeField]
        private Vector2 right = new Vector2();
    }
}
