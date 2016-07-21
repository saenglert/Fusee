using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fusee.Engine.Common;

namespace Fusee.Engine.Core
{
    public class GamepadDevice : InputDevice
    {
        /// <summary>
        /// Contains a dictionary of the IDs used for the gamepads axes. Keys are IDs assigned by OpenTK, Values are assigend by Fusee
        /// </summary>
        public readonly Dictionary<int, int> Axes;
        /// <summary>
        /// Contains a dictionary of the IDs used for the gamepads axes. Keys are IDs assigned by OpenTK, Values are assigend by Fusee
        /// </summary>
        public readonly Dictionary<int, int> Buttons;

        public GamepadDevice(IInputDeviceImp inpDeviceImp) : base(inpDeviceImp)
        {
            Axes = new Dictionary<int, int>();
            Buttons = new Dictionary<int, int>();
            foreach (var axis in inpDeviceImp.AxisImpDesc)
            {
                var orgId = axis.AxisDesc.Id; // Assigned by RCEnum
                var regId = RegisterVelocityAxis(axis.AxisDesc.Id).Id; // Assigned by Fusee

                Axes.Add(orgId, regId);
            }

            foreach (var button in inpDeviceImp.ButtonImpDesc)
            {
                var orgId = button.ButtonDesc.Id;
                var regId = RegisterSingleButtonAxis(button.ButtonDesc.Id).Id;
                
                Buttons.Add(orgId, regId);
            }
        }

        public float GetAxis(ControllerAxis axis) => GetAxis((int) axis);

        public bool GetButton(ControllerButton button) => GetButton((int) button);

        public bool IsButtonDown(ControllerButton button) => IsButtonDown((int) button);

        public bool IsButtonUp(ControllerButton button) => IsButtonUp((int) button);
    }
}