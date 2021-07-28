using HK.Framework.EventSystems;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// 様々な処理を行うタスクのインターフェイス
    /// </summary>
    public interface ITask
    {
        IMessageBroker Broker { get; }
        
        void Enter();

        void Update();

        void Exit();
    }
}
