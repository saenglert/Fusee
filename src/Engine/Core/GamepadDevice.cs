using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fusee.Engine.Common;

namespace Fusee.Engine.Core
{
    public class GamepadDevice : InputDevice
    {
        public readonly Dictionary<int, int> _axis;
        public readonly Dictionary<int, int> _buttons; 

        public GamepadDevice(IInputDeviceImp inpDeviceImp) : base(inpDeviceImp)
        {
            _axis = new Dictionary<int, int>();
            _buttons = new Dictionary<int, int>();
            foreach (var axis in inpDeviceImp.AxisImpDesc)
            {
                int orgId = axis.AxisDesc.Id;
                int regId = RegisterVelocityAxis(axis.AxisDesc.Id).Id;
                _axis.Add(orgId, regId);
                //Debug.WriteLine("Addded Axis with orgID " + orgId + " and regId " + regId);
            }

            foreach (var button in inpDeviceImp.ButtonImpDesc)
            {
                int orgId = button.ButtonDesc.Id;
                int regId = RegisterSingleButtonAxis(button.ButtonDesc.Id).Id;
                Debug.WriteLine("Key " + orgId + " added with value " + regId);
                _buttons.Add(orgId, regId);
            }
        }

        public float GetAxis(ControllerAxis axis) => GetAxis((int) axis);

        public bool GetButton(ControllerButton button) => GetButton((int) button);

        public bool IsButtonDown(ControllerButton button) => IsButtonDown((int) button);

        public bool IsButtonUp(ControllerButton button) => IsButtonUp((int) button);
    }
}