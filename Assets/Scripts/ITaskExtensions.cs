using System;
using HK.Framework.EventSystems;
using HK.CUIRPG.Events;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace HK.CUIRPG
{
    /// <summary>
    /// <see cref="ITask"/>に関する拡張関数
    /// </summary>
    public static class ITaskExtensions
    {
        public static IObservable<T> TakeExit<T>(this IObservable<T> self, ITask task)
        {
            return self.TakeUntil(Broker.Global.Receive<TaskEvents.Exit>().Where(x => x.Task == task));
        }
    }
}
