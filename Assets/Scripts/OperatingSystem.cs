using System.Collections.Generic;
using HK.Framework.EventSystems;
using HK.MineTerminal.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;

namespace HK.MineTerminal
{
    /// <summary>
    /// ゲームの中核を担うクラス
    /// </summary>
    public sealed class OperatingSystem : MonoBehaviour
    {
        [SerializeField]
        private Window desktop = default;

        [SerializeField]
        private StringAssetHolder localizedMessages = default;
        public StringAssetHolder LocalizedMessages => this.localizedMessages;

        public static OperatingSystem Instance { get; private set; }

        /// <summary>
        /// ポイント
        /// </summary>
        public int Point { get; private set; }

        /// <summary>
        /// 熱量
        /// </summary>
        public float Heat { get; private set; }

        /// <summary>
        /// 冷却量
        /// </summary>
        public int CoolPower { get; private set; } = 1;

        /// <summary>
        /// CPUのパワー
        /// </summary>
        public int CpuPower { get; private set; } = 1;

        /// <summary>
        /// 冷却量の割合
        /// </summary>
        public float CoolPowerRate => this.CoolPower / 10.0f;

        public readonly CommandManager CommandManager = new CommandManager();

        private readonly List<ITask> tasks = new List<ITask>();

        public static readonly ReactiveProperty<Window> CurrentWindow = new ReactiveProperty<Window>();

        void Awake()
        {
            Instance = this;

            CurrentWindow.Value = this.desktop;
        }

        void Start()
        {
            var session = new Session();
            this.AddTask(session);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
            {
                var session = new Session();
                this.AddTask(session);
            }

            foreach (var t in this.tasks)
            {
                t.Update();
            }

            this.Heat -= this.CoolPowerRate * Time.deltaTime;
            this.Heat = Mathf.Max(this.Heat, 0.0f);
        }

        public void AddPoint(int value)
        {
            this.Point += value;
        }

        public void AddPointFromCpuPower()
        {
            this.Point += this.CpuPower;
            this.Heat += (this.CpuPower - this.CoolPowerRate) / 10.0f;
        }

        public void AddTask(ITask task)
        {
            this.tasks.Add(task);
            task.Enter();
        }

        public void RemoveTask(ITask task)
        {
            Assert.IsTrue(this.tasks.Contains(task));
            this.tasks.Remove(task);
            task.Exit();
        }

        public bool CanAddCoolPower()
        {
            return true;
        }

        public void AddCoolPower()
        {
            this.CoolPower++;
        }

        public bool CanAddCpuPower()
        {
            return true;
        }

        public void AddCpuPower()
        {
            this.CpuPower++;
        }
    }
}
