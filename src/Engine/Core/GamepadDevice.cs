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
        public readonly Dictionary<int, int> _axes;
        /// <summary>
        /// Contains a dictionary of the IDs used for the gamepads axes. Keys are IDs assigned by OpenTK, Values are assigend by Fusee
        /// </summary>
        public readonly Dictionary<int, int> _buttons;
        /// <summary>
        /// The ID of the gamepad as in the index of the device in OpenTKs list of connected gamepads
        /// </summary>
        public readonly string _id;

        public GamepadDevice(IInputDeviceImp inpDeviceImp) : base(inpDeviceImp)
        {
            _axes = new Dictionary<int, int>();
            _buttons = new Dictionary<int, int>();
            _id = inpDeviceImp.Id;
            foreach (var axis in inpDeviceImp.AxisImpDesc)
            {
                int orgId = axis.AxisDesc.Id; // Assigned by OpenTK
                int regId = RegisterVelocityAxis(axis.AxisDesc.Id).Id; // Assigned by Fusee

                _axes.Add(orgId, regId);
            }

            foreach (var button in inpDeviceImp.ButtonImpDesc)
            {
                int orgId = button.ButtonDesc.Id;
                int regId = RegisterSingleButtonAxis(button.ButtonDesc.Id).Id;
                
                _buttons.Add(orgId, regId);
            }
        }

        public float GetAxis(ControllerAxis axis) => GetAxis((int) axis);

        public bool GetButton(ControllerButton button) => GetButton((int) button);

        public bool IsButtonDown(ControllerButton button) => IsButtonDown((int) button);

        public bool IsButtonUp(ControllerButton button) => IsButtonUp((int) button);
    }
}