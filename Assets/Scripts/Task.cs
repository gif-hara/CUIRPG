using HK.CUIRPG.Events;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// タスクの抽象クラス
    /// </summary>
    public abstract class Task : ITask
    {
        private readonly IMessageBroker broker = new MessageBroker();

        public IMessageBroker Broker => this.broker;

        public virtual void Enter()
        {
            Framework.EventSystems.Broker.Global.Publish(TaskEvents.Enter.Get(this));
        }

        public virtual void Exit()
        {
            Framework.EventSystems.Broker.Global.Publish(TaskEvents.Exit.Get(this));
        }

        public virtual void Update()
        {
        }
    }
}
