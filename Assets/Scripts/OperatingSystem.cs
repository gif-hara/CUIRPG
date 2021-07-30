using System.Collections.Generic;
using HK.Framework.EventSystems;
using HK.CUIRPG.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using HK.CUIRPG.Commands;
using HK.CUIRPG.Database;

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

        private CompositeDisposable m_Disposable = new CompositeDisposable();

        void Awake()
        {
            Instance = this;

            CurrentWindow.Value = this.desktop;

            CommandManager.OnRegisteredAliasCommandAsObServable
            .Subscribe(x =>
            {
                var userData = UserData.Instance;
                userData.UserAliases.Add(new UserAlias
                {
                    aliasName = x.aliasName,
                    commandData = x.commandData
                });

                userData.SendUpdateUserDataRequestAsObservable()
                .Subscribe();
            })
            .AddTo(m_Disposable);
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
