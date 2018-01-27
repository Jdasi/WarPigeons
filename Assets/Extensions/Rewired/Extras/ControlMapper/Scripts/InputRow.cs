﻿// Copyright (c) 2015 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

namespace Rewired.UI.ControlMapper {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    public class InputRow : MonoBehaviour {
        
        public Text label;
        public ButtonInfo[] buttons { get; private set; }

        private int rowIndex;
        private System.Action<int, ButtonInfo> inputFieldActivatedCallback;

        public void Initialize(int rowIndex, string label, System.Action<int, ButtonInfo> inputFieldActivatedCallback) {
            this.rowIndex = rowIndex;
            this.label.text = label;
            this.inputFieldActivatedCallback = inputFieldActivatedCallback;
            buttons = transform.GetComponentsInChildren<ButtonInfo>(true);
        }

        public void OnButtonActivated(ButtonInfo buttonInfo) {
            if(inputFieldActivatedCallback == null) return;
            inputFieldActivatedCallback(rowIndex, buttonInfo);
        }
    }
}