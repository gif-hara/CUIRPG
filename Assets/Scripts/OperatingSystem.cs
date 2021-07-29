using System.Collections.Generic;
using HK.Framework.EventSystems;
using HK.CUIRPG.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using HK.CUIRPG.Commands;

namespace HK.CUIRPG
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
    }
}
