using UnityEngine;
using UnityEditor;

namespace Picking
{
    // ドアの開閉制御を行います。
    public class Door : MonoBehaviour
    {
        [SerializeField][Header("開く角度（度数法）")]
        private float openAngle = 100.0f;
        [SerializeField][Header("開くアニメーションにかかる時間")]
        private float openTime = 2.0f;
        [SerializeField][Header("閉じるアニメーションにかかる時間")]
        private float closeTime = 2.0f;
        [SerializeField][Header("左ヒンジ : 開いたときにY軸が正の方向に回転します。")]
        private Transform leftHinge = null;
        [SerializeField][Header("右ヒンジ : 開いたときにY軸が負の方向に回転します。")]
        private Transform rightHinge = null;

        // ドアの開閉状態
        public enum State
        {
            Closed = 0,
            Open,
            Opening,
            Closing,
        }

        // 現在の開閉状態
        public State CurrentState { get; private set; } = State.Closing;
        // 開閉アニメーションの経過時間
        public float AnimationTime { get; private set; } = 0.0f;
        // メンバアクセス用プロパティ
        public float OpenTime { get => openTime; }
        public float CloseTime { get => closeTime; }

        // ドアを開く処理を行います。
        public void StartOpen()
        {
            CurrentState = State.Opening;
            AnimationTime = 0.0f;
        }

        // ドアを閉じる処理を行います。
        public void StartClose()
        {
            CurrentState = State.Closing;
            AnimationTime = 0.0f;
        }

        private void Update()
        {
            // 開閉アニメーションの更新
            if (CurrentState == State.Opening)
            {
                if (AnimationTime < openTime)
                {
                    // 開き進行中
                    float angle = openAngle * AnimationTime / openTime;
                    leftHinge.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
                    rightHinge.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
                }
                else
                {
                    // 開き完了
                    CurrentState = State.Open;
                    AnimationTime = 0.0f;
                    leftHinge.localRotation = Quaternion.Euler(0.0f, openAngle, 0.0f);
                    rightHinge.localRotation = Quaternion.Euler(0.0f, -openAngle, 0.0f);
                }
            }
            else if (CurrentState == State.Closing)
            {
                if (AnimationTime < closeTime)
                {
                    // 閉じ進行中
                    float angle = openAngle * (1.0f - AnimationTime / closeTime);
                    leftHinge.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
                    rightHinge.localRotation = Quaternion.Euler(0.0f, -angle, 0.0f);
                }
                else
                {
                    // 閉じ完了
                    CurrentState = State.Closed;
                    AnimationTime = 0.0f;
                    leftHinge.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    rightHinge.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
            }
            AnimationTime += Time.deltaTime;
        }

#if UNITY_EDITOR
        // エディター時のみ警告を行います。

        [InitializeOnLoadMethod]
        private static void OnProjectLoadedInEditor()
        {
            EditorApplication.playModeStateChanged +=
                (state) =>
                {
                // プレイモード開始時、フィールド未設定の警告を行います。
                if (state == PlayModeStateChange.EnteredPlayMode)
                    {
                        var instances = FindObjectsOfType<Door>();
                        foreach (var instance in instances)
                        {
                            instance.InvalidFieldAssert();
                        }
                    }
                };
        }

        private void InvalidFieldAssert()
        {
            if (leftHinge == null)
            {
                Debug.LogWarning($"{gameObject.name} の Left Hinge が未設定です！");
            }
            if (rightHinge == null)
            {
                Debug.LogWarning($"{gameObject.name} の Right Hinge が未設定です！");
            }
        }
#endif // UNITY_EDITOR
    }

}
