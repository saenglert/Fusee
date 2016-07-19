using System;
using OpenTK;
using OpenTK.Input;
using Fusee.Engine.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Fusee.Engine.Imp.Graphics.Desktop
{
    /// <summary>
    /// Input driver implementation for keyboard and mouse input on Desktop and Android.
    /// </summary>
    public class RenderCanvasInputDriverImp : IInputDriverImp
    {
        /// <summary>
        /// Constructor. Use this in platform specific application projects. 
        /// </summary>
        /// <param name="renderCanvas">The render canvas to provide mouse and keyboard input for.</param>
        public RenderCanvasInputDriverImp(IRenderCanvasImp renderCanvas)
        {
            if (renderCanvas == null)
                throw new ArgumentNullException(nameof(renderCanvas));

            if (!(renderCanvas is RenderCanvasImp))
                throw new ArgumentException("renderCanvas must be of type RenderCanvasImp", "renderCanvas");

            _gameWindow = ((RenderCanvasImp)renderCanvas)._gameWindow;
            if (_gameWindow == null)
                throw new ArgumentNullException(nameof(_gameWindow));

            _keyboard = new KeyboardDeviceImp(_gameWindow);
            _mouse = new MouseDeviceImp(_gameWindow);
        }

        private GameWindow _gameWindow;
        private KeyboardDeviceImp _keyboard;
        private MouseDeviceImp _mouse;

        /// <summary>
        /// Devices supported by this driver: One mouse and one keyboard.
        /// </summary>
        public IEnumerable<IInputDeviceImp> Devices
        {
            get
            {
                yield return _mouse;
                yield return _keyboard;
            }
        }

        /// <summary>
        /// Returns a human readable description of this driver.
        /// </summary>
        public string DriverDesc
        {
            get
            {
#if PLATFORM_DESKTOP
                const string pf = "Desktop";
#elif PLATFORM_ANDROID
                const string pf = "Android";
#endif
                return "OpenTK GameWindow Mouse and Keyboard input driver for " + pf;
            }
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string DriverId
        {
            get { return GetType().FullName; }
        }

        #pragma warning disable 0067
        /// <summary>
        /// Not supported on this driver. Mouse and keyboard are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<DeviceImpDisconnectedArgs> DeviceDisconnected;

        /// <summary>
        /// Not supported on this driver. Mouse and keyboard are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<NewDeviceImpConnectedArgs> NewDeviceConnected;
        #pragma warning restore 0067

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Part of the Dispose pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RenderCanvasInputDriverImp() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Part of the dispose pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }


    /// <summary>
    /// Keyboard input device implementation for Desktop an Android platforms.
    /// </summary>
    public class KeyboardDeviceImp : IInputDeviceImp
    {
        private GameWindow _gameWindow;
        private Keymapper _keymapper;

        /// <summary>
        /// Should be called by the driver only.
        /// </summary>
        /// <param name="gameWindow"></param>
        internal KeyboardDeviceImp(GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
            _keymapper = new Keymapper();
            _gameWindow.Keyboard.KeyDown += OnGameWinKeyDown;
            _gameWindow.Keyboard.KeyUp += OnGameWinKeyUp;

        }

        /// <summary>
        /// Returns the number of Axes (==0, keyboard does not support any axes).
        /// </summary>
        public int AxesCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Empty enumeration for keyboard, since <see cref="AxesCount"/> is 0.
        /// </summary>
        public IEnumerable<AxisImpDescription> AxisImpDesc
        {
            get
            {
                yield break;
            }
        }

        /// <summary>
        /// Returns the number of enum values of <see cref="KeyCodes"/>
        /// </summary>
        public int ButtonCount
        {
            get
            {
                return Enum.GetNames(typeof(KeyCodes)).Length;
            }
        }

        /// <summary>
        /// Returns a description for each keyboard button.
        /// </summary>
        public IEnumerable<ButtonImpDescription> ButtonImpDesc
        {
            get
            {
                return from k in _keymapper orderby k.Value.Id select new ButtonImpDescription {ButtonDesc = k.Value, PollButton = false};
            }
        }

        /// <summary>
        /// This is a keyboard device, so this property returns <see cref="DeviceCategory.Keyboard"/>.
        /// </summary>
        public DeviceCategory Category
        {
            get
            {
                return DeviceCategory.Keyboard;
            }
        }

        /// <summary>
        /// Human readable description of this device (to be used in dialogs).
        /// </summary>
        public string Desc
        {
            get
            {
                return "Standard Keyboard implementation.";
            }
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string Id
        {
            get
            {
                return GetType().FullName;
            }
        }


        #pragma warning disable 0067
        /// <summary>
        /// No axes exist on this device, so listeners registered to this event will never get called.
        /// </summary>
        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;

        /// <summary>
        /// All buttons exhibited by this device are event-driven buttons, so this is the point to hook to in order
        /// to get information from this device.
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;
        #pragma warning restore 0067

        /// <summary>
        /// Called when keyboard button is pressed down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="key">The <see cref="KeyboardKeyEventArgs"/> instance containing the event data.</param>
        protected void OnGameWinKeyDown(object sender, KeyboardKeyEventArgs key)
        {
            ButtonDescription btnDesc;
            if (ButtonValueChanged != null && _keymapper.TryGetValue(key.Key, out btnDesc))
            {
                ButtonValueChanged(this, new ButtonValueChangedArgs
                {
                    Pressed = true,
                    Button = btnDesc
                });
            }
        }

        /// <summary>
        /// Called when keyboard button is released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="key">The <see cref="KeyboardKeyEventArgs"/> instance containing the event data.</param>
        protected void OnGameWinKeyUp(object sender, KeyboardKeyEventArgs key)
        {
            ButtonDescription btnDesc;
            if (ButtonValueChanged != null && _keymapper.TryGetValue(key.Key, out btnDesc))
            {
                ButtonValueChanged(this, new ButtonValueChangedArgs
                {
                    Pressed = false,
                    Button = btnDesc
                });
            }
        }

        /// <summary>
        /// This device does not support any axes at all. Always throws.
        /// </summary>
        /// <param name="iAxisId">No matter what you specify here, you'll evoke an exception.</param>
        /// <returns>No return, always throws.</returns>
        public float GetAxis(int iAxisId)
        {
            throw new InvalidOperationException($"Unsopported axis {iAxisId}. This device does not support any axis at all.");
        }

        /// <summary>
        /// This device does not support to-be-polled-buttons. All keyboard buttons are event-driven. Listen to the <see cref="ButtonValueChanged"/>
        /// event to reveive keyboard notifications from this device.
        /// </summary>
        /// <param name="iButtonId">No matter what you specify here, you'll evoke an exception.</param>
        /// <returns>No return, always throws.</returns>
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException($"Button {iButtonId} does not exist or is no pollable. Listen to the ButtonValueChanged event to receive keyboard notifications from this device.");
        }
    }

    /// <summary>
    /// Mouse input device implementation for Desktop an Android platforms.
    /// </summary>
    public class MouseDeviceImp : IInputDeviceImp
    {
        private GameWindow _gameWindow;
        private ButtonImpDescription _btnLeftDesc, _btnRightDesc, _btnMiddleDesc;

        /// <summary>
        /// Creates a new mouse input device instance using an existing <see cref="OpenTK.GameWindow"/>.
        /// </summary>
        /// <param name="gameWindow">The game window providing mouse input.</param>
        public MouseDeviceImp(GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
            _gameWindow.Mouse.ButtonDown += OnGameWinMouseDown;
            _gameWindow.Mouse.ButtonUp += OnGameWinMouseUp;

            _btnLeftDesc = new ButtonImpDescription
            {
                ButtonDesc = new ButtonDescription
                {
                    Name = "Left",
                    Id = (int) MouseButtons.Left
                },
                PollButton = false
            };
            _btnMiddleDesc = new ButtonImpDescription
            {
                ButtonDesc = new ButtonDescription
                {
                    Name = "Middle",
                    Id = (int) MouseButtons.Middle
                },
                PollButton = false
            };
            _btnRightDesc = new ButtonImpDescription
            {
                ButtonDesc = new ButtonDescription
                {
                    Name = "Right",
                    Id = (int) MouseButtons.Right
                },
                PollButton = false
            };
        }

        /// <summary>
        /// Number of axes. Here seven: "X", "Y" and "Wheel" as well as MinX, MaxX, MinY and MaxY
        /// </summary>
        public int AxesCount => 7;

        /// <summary>
        /// Returns description information for all axes.
        /// </summary>
        public IEnumerable<AxisImpDescription> AxisImpDesc
        {
            get
            {
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "X",
                        Id = (int) MouseAxes.X,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.OtherAxis,
                        MinValueOrAxis = (int) MouseAxes.MinX,
                        MaxValueOrAxis = (int) MouseAxes.MaxX
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Y",
                        Id = (int) MouseAxes.Y,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.OtherAxis,
                        MinValueOrAxis = (int)MouseAxes.MinY,
                        MaxValueOrAxis = (int)MouseAxes.MaxY
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Wheel",
                        Id = (int) MouseAxes.Wheel,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = float.NaN,
                        MaxValueOrAxis = float.NaN
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "MinX",
                        Id = (int)MouseAxes.MinX,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = float.NaN,
                        MaxValueOrAxis = float.NaN
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "MaxX",
                        Id = (int)MouseAxes.MaxX,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = float.NaN,
                        MaxValueOrAxis = float.NaN
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "MinY",
                        Id = (int)MouseAxes.MinY,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = float.NaN,
                        MaxValueOrAxis = float.NaN
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "MaxY",
                        Id = (int)MouseAxes.MaxY,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = float.NaN,
                        MaxValueOrAxis = float.NaN
                    },
                    PollAxis = true
                };
            }
        }

        /// <summary>
        /// Number of buttons exposed by this device. Here three: Left, Middle and Right mouse buttons.
        /// </summary>
        public int ButtonCount => 3;

        /// <summary>
        /// A mouse exposes three buttons: left, middle and right.
        /// </summary>
        public IEnumerable<ButtonImpDescription> ButtonImpDesc
        {
            get
            {
                yield return _btnLeftDesc;
                yield return _btnMiddleDesc;
                yield return _btnRightDesc;
            }
        }

        /// <summary>
        /// Returns <see cref="DeviceCategory.Mouse"/>, just because it's a mouse.
        /// </summary>
        public DeviceCategory Category => DeviceCategory.Mouse;

        /// <summary>
        /// Short description string for this device to be used in dialogs.
        /// </summary>
        public string Desc => "Standard Mouse implementation.";

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string Id => GetType().FullName;

        /// <summary>
        /// No event-based axes are exposed by this device. Use <see cref="GetAxis"/> to akquire mouse axis information.
        /// </summary>
        #pragma warning disable 0067
        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;

        /// <summary>
        /// All three mouse buttons are event-based. Listen to this event to get information about mouse button state changes.
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;
        #pragma warning restore 0067

        /// <summary>
        /// Retrieves values for the X, Y and Wheel axes. No other axes are supported by this device.
        /// </summary>
        /// <param name="iAxisId">The axis to retrieve information for.</param>
        /// <returns>The value at the given axis.</returns>
        public float GetAxis(int iAxisId)
        {
            switch (iAxisId)
            {
                case (int) MouseAxes.X:
                    return _gameWindow.Mouse.X;
                case (int) MouseAxes.Y:
                    return _gameWindow.Mouse.Y;
                case (int) MouseAxes.Wheel:
                    return _gameWindow.Mouse.WheelPrecise;
                case (int)MouseAxes.MinX:
                    return 0;
                case (int)MouseAxes.MaxX:
                    return _gameWindow.Width;
                case (int)MouseAxes.MinY:
                    return 0;
                case (int)MouseAxes.MaxY:
                    return _gameWindow.Height;
            }
            throw new InvalidOperationException($"Unknown axis {iAxisId}. Cannot get value for unknown axis.");
        }

        /// <summary>
        /// This device does not support to-be-polled-buttons. All mouse buttons are event-driven. Listen to the <see cref="ButtonValueChanged"/>
        /// event to reveive keyboard notifications from this device.
        /// </summary>
        /// <param name="iButtonId">No matter what you specify here, you'll evoke an exception.</param>
        /// <returns>No return, always throws.</returns>
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException(
                $"Unsopported axis {iButtonId}. This device does not support any to-be polled axes at all.");
        }

        /// <summary>
        /// Called when the game window's mouse is pressed down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="mouseArgs">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected void OnGameWinMouseDown(object sender, MouseButtonEventArgs mouseArgs)
        {
            if (ButtonValueChanged != null)
            {
                ButtonDescription btnDesc;
                switch (mouseArgs.Button)
                {
                    case MouseButton.Left:
                        btnDesc = _btnLeftDesc.ButtonDesc;
                        break;
                    case MouseButton.Middle:
                        btnDesc = _btnMiddleDesc.ButtonDesc;
                        break;
                    case MouseButton.Right:
                        btnDesc = _btnRightDesc.ButtonDesc;
                        break;
                    default:
                        return;
                }

                ButtonValueChanged(this, new ButtonValueChangedArgs
                {
                    Pressed = true,
                    Button = btnDesc
                });
            }
        }

        /// <summary>
        /// Called when the game window's mouse is released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="mouseArgs">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected void OnGameWinMouseUp(object sender, MouseButtonEventArgs mouseArgs)
        {
            if (ButtonValueChanged != null)
            {
                ButtonDescription btnDesc;
                switch (mouseArgs.Button)
                {
                    case MouseButton.Left:
                        btnDesc = _btnLeftDesc.ButtonDesc;
                        break;
                    case MouseButton.Middle:
                        btnDesc = _btnMiddleDesc.ButtonDesc;
                        break;
                    case MouseButton.Right:
                        btnDesc = _btnRightDesc.ButtonDesc;
                        break;
                    default:
                        return;
                }

                ButtonValueChanged(this, new ButtonValueChangedArgs
                {
                    Pressed = false,
                    Button = btnDesc
                });
            }
        }
    }

    /// <summary>
    /// Driver Implementation using OpentTK for Gamepad Controller Input
    /// </summary>
    public class RenderCanvasGampadInputDriverImp : IInputDriverImp
    {
        private List<GameControllerDeviceImp> _controllers;
        const int MaxNumberGameControllers = 4;

        /// <summary>
        /// 
        /// </summary>
        public RenderCanvasGampadInputDriverImp ()
        {
            _controllers = searchGameControllers();
        }

        private List<GameControllerDeviceImp> searchGameControllers()
        {
            List<GameControllerDeviceImp> controllers = new List<GameControllerDeviceImp>();
            for (int i = 0; i < MaxNumberGameControllers; i++)
            {
                GamePadCapabilities gamePadCapabilities = GamePad.GetCapabilities(i);
                if (gamePadCapabilities.IsConnected)
                    controllers.Add(new GameControllerDeviceImp(i));
            }

            return controllers;
        }

        public IEnumerable<IInputDeviceImp> Devices => _controllers.Cast<IInputDeviceImp>();

        public string DriverId => GetType().FullName;

        public string DriverDesc {
            get
            {
#if PLATFORM_DESKTOP
                const string pf = "Desktop";
#elif PLATFORM_ANDROID
                const string pf = "Android";
#endif
                return "OpenTK Gamepad Controller input driver for " + pf;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<NewDeviceImpConnectedArgs> NewDeviceConnected;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DeviceImpDisconnectedArgs> DeviceDisconnected;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Part of the Dispose pattern.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RenderCanvasInputDriverImp() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        /// <summary>
        /// Part of the dispose pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// Gamepad Controller input device implementation for Desktop and Android platforms
    /// </summary>
    /// <param name="gameCapabilities">The <see cref="OpenTK.Input.GamePadCapabilities"/> capabilities of the controller.</param>
    /// <param name="gameWindow">The game window providing the input.</param>
    public class GameControllerDeviceImp : IInputDeviceImp
   {
        private readonly int _gamepadIndex;
        private GamePadState _state;
        private int _axisCount;
        private readonly List<AxisImpDescription> _axisImpDescriptions; 
        private int _buttonCount;
        private readonly List<ButtonImpDescription> _buttonImpDescriptions;

        /// <summary>
        /// Initializes the gamecontroller
        /// </summary>
        public GameControllerDeviceImp(int index)
       {
            Debug.WriteLine("Creating Gamepad with ID " + index);
           _gamepadIndex = index;
           GamePadCapabilities controller = GamePad.GetCapabilities(_gamepadIndex);
           _state = GamePad.GetState(_gamepadIndex);
            
            if (!controller.IsConnected)
                throw new Exception("Given Controller is not connected");

            _axisCount = 0;
            _axisImpDescriptions = new List<AxisImpDescription>();
            SetAxisImpDescriptions(controller);

            _buttonCount = 0;
            _buttonImpDescriptions = new List<ButtonImpDescription>();
            SetButtonImpDescriptions(controller);
       }

       private void SetAxisImpDescriptions(GamePadCapabilities controller)
       {
            if (controller.HasLeftXThumbStick)
            {
                _axisCount++;
                _axisImpDescriptions.Add(
                    CreateAxisImpDescription("LeftX", (int) ControllerAxis.LeftX, AxisDirection.X, AxisNature.Position, AxisBoundedType.OtherAxis, (int) ControllerAxis.MinX, (int) ControllerAxis.MaxX, true)        
                );
            }

            if (controller.HasLeftYThumbStick)
            {
                _axisCount++;
                _axisImpDescriptions.Add(
                    CreateAxisImpDescription("LeftY", (int) ControllerAxis.LeftY, AxisDirection.Y, AxisNature.Position, AxisBoundedType.OtherAxis, (int) ControllerAxis.MinY, (int) ControllerAxis.MaxY, true)
                );
            }

           if (controller.HasRightXThumbStick)
           {
                _axisCount++;
                _axisImpDescriptions.Add(
                    CreateAxisImpDescription("RightX", (int) ControllerAxis.RightX, AxisDirection.X, AxisNature.Position, AxisBoundedType.OtherAxis, (int) ControllerAxis.MinX, (int) ControllerAxis.MaxX, true)    
                );
           }

           if (controller.HasRightYThumbStick)
           {
                _axisCount++;
                _axisImpDescriptions.Add(
                    CreateAxisImpDescription("RightY", (int) ControllerAxis.RightY, AxisDirection.Y, AxisNature.Position, AxisBoundedType.OtherAxis, (int) ControllerAxis.MinY, (int) ControllerAxis.MaxY, true)
                );
           }
        }

       private AxisImpDescription CreateAxisImpDescription(String name, int id, AxisDirection direction, AxisNature nature, AxisBoundedType boundedType, int minvalue, int maxvalue, bool poll)
       {
           return new AxisImpDescription{
               AxisDesc = new AxisDescription()
               {
                   Name =  name,
                   Id = id,
                   Direction = direction,
                   Nature = nature,
                   Bounded = boundedType,
                   MinValueOrAxis = minvalue,
                   MaxValueOrAxis = maxvalue
               },
               PollAxis = poll
           };
       }

       private void SetButtonImpDescriptions(GamePadCapabilities controller)
       {
            if (controller.HasDPadUpButton)
            {
            CreateButtonImpDescription("DPadUp", (int) ControllerButton.DPadUp, true);
            }

            if (controller.HasDPadDownButton)
            {
                CreateButtonImpDescription("DPadDown", (int) ControllerButton.DPadDown, true);
            }

            if (controller.HasDPadLeftButton)
            {
                CreateButtonImpDescription("DPadLeft", (int) ControllerButton.DPadLeft, true);
            }

            if (controller.HasDPadRightButton)
            {
                CreateButtonImpDescription("DPadRight", (int) ControllerButton.DPadRight, true);
            }

            if (controller.HasAButton)
            {
            CreateButtonImpDescription("A", (int) ControllerButton.A, true);
            }

            if (controller.HasBButton)
            {
                CreateButtonImpDescription("B", (int) ControllerButton.B, true);
            }

            if (controller.HasXButton)
            {
            CreateButtonImpDescription("X", (int) ControllerButton.X, true);
            }

            if (controller.HasYButton)
            {
                CreateButtonImpDescription("Y", (int) ControllerButton.Y, true);
            }

            if (controller.HasLeftStickButton)
            {
                CreateButtonImpDescription("LStickButton", (int) ControllerButton.LStickButton, true);
            }

            if (controller.HasRightStickButton)
            {
                CreateButtonImpDescription("RStickButton", (int)ControllerButton.RStickButton, true);
            }

            if (controller.HasLeftShoulderButton)
            {
                CreateButtonImpDescription("LShoulderButton", (int)ControllerButton.LShoulderButton, true);
            }

            if (controller.HasLeftTrigger)
            {
                CreateButtonImpDescription("LTrigger", (int)ControllerButton.LTrigger, true);
            }

            if (controller.HasRightShoulderButton)
            {
                CreateButtonImpDescription("RShoulderButton", (int)ControllerButton.RShoulderButton, true);
            }

            if (controller.HasRightTrigger)
            {
                CreateButtonImpDescription("RTrigger", (int) ControllerButton.RTrigger, true);
            }

            if (controller.HasBackButton)
            {
                CreateButtonImpDescription("Back", (int) ControllerButton.Back, true);
            }

            if (controller.HasStartButton)
            {
                CreateButtonImpDescription("Start", (int) ControllerButton.Start, true);
            }

            if (controller.HasBigButton)
            {
                CreateButtonImpDescription("Home", (int) ControllerButton.Home, true);
            }
        }
       
       private void CreateButtonImpDescription(String name, int id, bool poll)
       {
            _buttonCount++;
            _buttonImpDescriptions.Add(new ButtonImpDescription
            {
                ButtonDesc = new ButtonDescription
                {
                    Name = name,
                    Id = id
                },
                PollButton = poll
            });
       }
        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string Id => _gamepadIndex.ToString();

        /// <summary>
        /// Short description string for this device to be used in dialogs.
        /// </summary>
        public string Desc => "Standard GameController implementation.";

        /// <summary>
        /// Returns <see cref="DeviceCategory.GameController"/>, just because it's a gamecontroller.
        /// </summary>
        public DeviceCategory Category => DeviceCategory.GameController;

        private void UpdateState()
        {
            GamePadState newState = GamePad.GetState(_gamepadIndex);


            if (!newState.IsConnected)
            {
                
            }
            else if (!_state.Equals(newState))
                _state = newState;
        }

        public float GetAxis(int iAxisId)
        {
            UpdateState();
            //Debug.WriteLine("Call received for Axis with ID " + iAxisId);
            switch (iAxisId)
            {
                case (int)ControllerAxis.LeftX:
                    //Debug.WriteLine(_state.ThumbSticks.Left.X);
                    return _state.ThumbSticks.Left.X;
                case (int)ControllerAxis.LeftY:
                    //Debug.WriteLine(_state.ThumbSticks.Left.Y);
                    return _state.ThumbSticks.Left.Y;
                case (int)ControllerAxis.RightX:
                    //Debug.WriteLine(_state.ThumbSticks.Right.X);
                    return _state.ThumbSticks.Right.X;
                case (int)ControllerAxis.RightY:
                    //Debug.WriteLine(_state.ThumbSticks.Right.Y);
                    return _state.ThumbSticks.Right.Y;
                default:
                    throw new Exception("Axis not found");
            }
        }

        public bool GetButton(int iButtonId)
        {
            UpdateState();
            Debug.WriteLine("Controller " + _gamepadIndex + "  & Button " + iButtonId);
            switch (iButtonId)
            {
                case (int)ControllerButton.A:
                    return _state.Buttons.A == ButtonState.Pressed;
                case (int)ControllerButton.B:
                    return _state.Buttons.B == ButtonState.Pressed;
                case (int)ControllerButton.X:
                    return _state.Buttons.X == ButtonState.Pressed;
                case (int)ControllerButton.Y:
                    return _state.Buttons.Y == ButtonState.Pressed;
                case (int)ControllerButton.DPadUp:
                    return _state.DPad.Up == ButtonState.Pressed;
                case (int)ControllerButton.DPadDown:
                    return _state.DPad.Down == ButtonState.Pressed;
                case (int)ControllerButton.DPadLeft:
                    return _state.DPad.Left == ButtonState.Pressed;
                case (int)ControllerButton.DPadRight:
                    return _state.DPad.Right == ButtonState.Pressed;
                case (int)ControllerButton.LStickButton:
                    return _state.Buttons.LeftStick == ButtonState.Pressed;
                case (int)ControllerButton.RStickButton:
                    return _state.Buttons.RightStick == ButtonState.Pressed;
                case (int)ControllerButton.RShoulderButton:
                    return _state.Buttons.RightShoulder == ButtonState.Pressed;
                case (int)ControllerButton.RTrigger:
                    return _state.Triggers.Left >= 0.5f;
                case (int)ControllerButton.LShoulderButton:
                    return _state.Buttons.LeftShoulder == ButtonState.Pressed;
                case (int)ControllerButton.LTrigger:
                    return _state.Triggers.Left >= 0.5;
                case (int)ControllerButton.Start:
                    return _state.Buttons.Start == ButtonState.Pressed;
                case (int)ControllerButton.Back:
                    return _state.Buttons.Back == ButtonState.Pressed;
                case (int)ControllerButton.Home:
                    return _state.Buttons.BigButton == ButtonState.Pressed;
                default:
                    throw new Exception("Button not found");
            }
        }

        /// <summary>
        /// Number of axes. Here five: "X-Axis" and "Y-Axis" on the left stick, "X-Rotation" and "Y-Rotation" on the right stick
        /// and "Z-Axis" on the left and right triggers.
        /// </summary>
        public int AxesCount => _axisCount;

        public int ButtonCount => _buttonCount;

        public IEnumerable<ButtonImpDescription> ButtonImpDesc => _buttonImpDescriptions;

        public IEnumerable<AxisImpDescription> AxisImpDesc => _axisImpDescriptions;

        /// <summary>
        /// Currently not implemented
        /// </summary>
        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;       

        /// <summary>
        /// Currently not implemented
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;

        /// <summary>
        /// Currently not Implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="buttonArgs"></param>
        protected void OnButtonValueChanged(object sender, ButtonValueChangedArgs buttonArgs)
       {
            ButtonDescription btnDesc;
            if (ButtonValueChanged != null && this.ButtonExistsOnDevice(buttonArgs.Button.Id, out btnDesc))
            {
                ButtonValueChanged(this, new ButtonValueChangedArgs
                {
                    Pressed = false,
                    Button = btnDesc
                });
            }
        }

        private bool ButtonExistsOnDevice(int buttonId, out ButtonDescription btnDesc)
       {
           foreach (var button in ButtonImpDesc)
           {
               if (button.ButtonDesc.Id == buttonId)
               {
                   btnDesc = button.ButtonDesc;
                   return true;
               }
           }

           btnDesc = new ButtonDescription();
           return false;
       }
    }
}
