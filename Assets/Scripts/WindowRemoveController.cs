﻿using HK.MineTerminal.Events;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace HK.MineTerminal
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WindowRemoveController : MonoBehaviour
    {
        [SerializeField]
        private Window window = default;

        [SerializeField]
        private Button button = default;

        void Awake()
        {
            this.button.onClick.AddListener(() =>
            {
                this.window.Broker.Publish(WindowEvents.RequestRemove.Get());
            });
        }
    }
}
