using System;
using System.Collections.Generic;
using UnityEngine;

namespace Picking
{
    // 振動などの演出を管理します。
    public sealed class Director : MonoBehaviour
    {
        public event Action<float, float> onVibrate = null;

        public float Left { get => left; set => left = value; }
        public float Right { get => right; set => right = value; }

        // 左の振動をスケジュールします。
        public void ScheduleLeft(float value, float time)
        {
            if (leftSchedules == null)
                leftSchedules = new List<(float value, float time)>();

            AddScheduleToList(value, time, leftSchedules);
        }
        // 右の振動をスケジュールします。
        public void ScheduleRight(float value, float time)
        {
            if (rightSchedules == null)
                rightSchedules = new List<(float value, float time)>();

            AddScheduleToList(value, time, rightSchedules);
        }

        // スケジュールされた振動を全てキャンセルします。
        public void ClearSchedules()
        {
            leftSchedules?.Clear();
            rightSchedules?.Clear();
        }


        [Header("確認用")]
        [SerializeField]
        private float left = 0.0f;
        [SerializeField]
        private float right = 0.0f;

        private List<(float value, float time)> leftSchedules;
        private List<(float value, float time)> rightSchedules;


        private void OnDisable()
        {
            ClearSchedules();
            onVibrate(0.0f, 0.0f);
        }

        private void Update()
        {
            onVibrate?.Invoke(left + SumListValue(leftSchedules), right + SumListValue(rightSchedules));
        }

        private float SumListValue(List<(float value, float time)> list)
        {
            if (list == null)
                return 0.0f;

            int removeCount = 0;
            float result = 0.0f;
            foreach (var schedule in list)
            {
                result += schedule.value;

                if (Time.time > schedule.time)
                    removeCount++;
            }
            list.RemoveRange(0, removeCount);

            return result;
        }

        // スケジュールをリストにソートして追加します。
        private void AddScheduleToList(float value, float time, List<(float value, float time)> list)
        {
            int index = 0;
            foreach (var schedule in list)
            {
                if (schedule.time > time + Time.time)
                {
                    break;
                }
                index++;
            }
            list.Insert(index, (value, time + Time.time));
        }
    }
}
