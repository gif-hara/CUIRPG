﻿using System;
using System.Collections.Generic;
using HK.Framework.EventSystems;
using HK.CUIRPG.Events;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="OperatingSystem"/>を操作するクラス
    /// </summary>
    public sealed class Session : Task, IInteractor
    {
        public readonly Queue<string> Histories = new Queue<string>();

        public readonly Queue<string> buffer = new Queue<string>();

        /// <summary>
        /// <see cref="Histories"/>が更新された回数
        /// </summary>
        public int HistoryUpdateCount { get; private set; }

        private int busyCount = 0;

        private readonly IReactiveProperty<bool> isInteractable = new ReactiveProperty<bool>(true);

        public IReadOnlyReactiveProperty<bool> IsInteractable => this.isInteractable;

        private const int HistoryLimit = 50;

        public override void Enter()
        {
            base.Enter();
            this.Send($"Login: {DateTime.Now.ToString()}");
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void EnqueueHistory(string message)
        {
            this.Histories.Enqueue(message);
            this.HistoryUpdateCount++;

            if(this.Histories.Count > HistoryLimit)
            {
                this.Histories.Dequeue();
            }
        }

        public void Send(string message)
        {
            this.EnqueueHistory(message);
        }

        public void Receive(string message)
        {
            this.EnqueueHistory(message);
            if(!this.isInteractable.Value)
            {
                this.buffer.Enqueue(message);
            }
            else
            {
                OperatingSystem.Instance.CommandManager.Invoke(message, this);
                this.Broker.Publish(IInteractorEvents.Received.Get(new CommandData(message)));
            }
        }

        public void Accept()
        {
            this.busyCount--;
            Assert.IsTrue(this.busyCount >= 0);
            if(this.busyCount <= 0)
            {
                this.isInteractable.Value = true;
                if (this.buffer.Count > 0)
                {
                    this.Receive(this.buffer.Dequeue());
                }
            }
        }

        public void Busy()
        {
            this.busyCount++;
            this.isInteractable.Value = false;
        }

        public void Abort()
        {
            this.busyCount = 1;
            this.Accept();
            this.Send("Aborted");
        }

        public void ClearHistory()
        {
            this.Histories.Clear();
            this.HistoryUpdateCount++;
        }
    }
}