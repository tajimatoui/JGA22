using System;
using UnityEngine;
using UnityEditor;

namespace Picking
{
    // 通りすがりの人のデータを管理します。
    public sealed class NearHumanData : MonoBehaviour
    {
        public event Action<int> onPhazeChange = null;

        public Action OnFound { private get; set; } = null;

        public bool Near => startTime <= Time.time;
        public bool Caution => phaze >= 2;

        // 操作時の警告レベル上昇処理を行います。
        public void CheckAtAct()
        {
            lastMoveTime = Time.time;

            if (!Near)
                return;

            if (Caution)
            {
                cautionTime += Time.deltaTime;

                int newPhaze = 2;
                for (int i = cautionStepTimes.Length - 1; i >= 0; i--)
                {
                    if (cautionStepTimes[i] <= cautionTime)
                    {
                        newPhaze = i + 2;
                        break;
                    }
                }
                if (phaze != newPhaze)
                {
                    phaze = newPhaze;
                    onPhazeChange?.Invoke(phaze);
                    if (phaze >= cautionStepTimes.Length + 1)
                        OnFound?.Invoke();
                }
            }
            else
            {
                cautionTime = 0.0f;
                phaze = 2;
                onPhazeChange?.Invoke(phaze);
            }
        }


        [SerializeField][Header("待機時間")]
        private float waitTime = 2.0f;
        [SerializeField][Header("警戒後待機時間")]
        private float cautionWaitTime = 10.0f;
        [SerializeField][Header("出現間隔")]
        private float intervalMin = 10.0f;
        [SerializeField]
        private float intervalMax = 20.0f;
        [SerializeField][Header("警戒レベルアップ間隔")]
        private float[] cautionStepTimes = { 3.0f, 7.0f };


        [Header("確認用")]
        [SerializeField]
        private float startTime = 0.0f;
        [SerializeField]
        private float cautionTime = 0.0f;
        [SerializeField]
        private float lastMoveTime = 0.0f;
        [SerializeField]
        private int phaze = 0;


        private void Awake()
        {
            ResetState();
        }

        private void Update()
        {
            // 警戒終了処理
            if (Caution)
            {
                if (Time.time >= lastMoveTime + cautionWaitTime)
                    ResetState();
            }
            else
            {
                if (Time.time >= startTime && phaze < 1)
                {
                    phaze = 1;
                    onPhazeChange?.Invoke(phaze);
                }

                if (Time.time >= startTime + waitTime)
                    ResetState();
            }
        }

        private void ResetState()
        {
            startTime = Time.time + UnityEngine.Random.Range(intervalMin, intervalMax);
            cautionTime = 0.0f;
            lastMoveTime = 0.0f;
            phaze = 0;
            onPhazeChange?.Invoke(phaze);
        }

#if UNITY_EDITOR // エディター時のみ有効になります。ビルドされたものには反映されません。
        [InitializeOnLoadMethod] // この関数はUnityエディタ上でロードされたタイミングで実行されます。
        private static void OnProjectLoadedInEditor()
        {
            EditorApplication.playModeStateChanged +=
                (state) =>
                {
                    // プレイモード開始前にフィールド未設定の警告を行います。
                    if (state == PlayModeStateChange.ExitingEditMode)
                    {
                        var instances = FindObjectsOfType<NearHumanData>();
                        foreach (var instance in instances)
                        {
                            instance.WarnInvalidField();
                        }
                    }
                };
        }

        // Serializeされたフィールドが不正な場合警告を行います。
        private void WarnInvalidField()
        {
            if (cautionStepTimes.Length <= 0)
                Debug.LogWarning($"{gameObject.name} の Caution Step Times が未設定です");
        }
#endif // UNITY_EDITOR
    }
}
