﻿using System;

namespace Fusee.Engine.Common
{
    /// <summary>
    /// The render state enumeration. This is used in render context implementations to identify the render state to set.
    /// Keep this binary compatible with DirectX version 9.
    /// </summary>
    public enum RenderState
    {
        // Butter and bread states
#pragma warning disable 1591
        FillMode = 8,
        CullMode = 22,
        Clipping = 136,

        // Z states
        ZFunc = 23,
        ZEnable = 7,

        // Blend states
        // Blending assignments (always independent for rgb and alpha: 
        //      OUTrgb = SRCrgb * SourceBlend       <BlendOperation(+,-,-inv,min,max)>        DSTrgb * DestinationBlend;
        //      OUTa   = SRCa   * SourceBlendAlpha  <BlendOperationAlpha(+,-,-inv,min,max)>   DSTa   * DestinationBlendAlpha;
        // OUT: The new pixel written to the output buffer
        // SRC: The pixel generated by the pixel shader
        // DST: The pixel already in the output buffer
        AlphaBlendEnable = 27,
        BlendOperation = 171,
        BlendOperationAlpha = 209,
        SourceBlend = 19,
        DestinationBlend = 20,
        SourceBlendAlpha = 207,
        DestinationBlendAlpha = 208,
        BlendFactor = 193,
        // Texture states
        Wrap0 = 128,
        Wrap1 = 129,
        Wrap2 = 130,
        Wrap3 = 131,

        /*        
        ShadeMode = 9,
        ZWriteEnable = 14,
        AlphaTestEnable = 15,
        LastPixel = 16,
        DitherEnable = 26,
        FogEnable = 28,
        SpecularEnable = 29,
        FogColor = 34,
        FogTableMode = 35,
        FogStart = 36,
        FogEnd = 37,
        FogDensity = 38,
        RangeFogEnable = 48,
        StencilEnable = 52,
        StencilFail = 53,
        StencilZFail = 54,
        StencilPass = 55,
        StencilFunc = 56,
        StencilRef = 57,
        StencilMask = 58,
        StencilWriteMask = 59,
        TextureFactor = 60,

        Wrap4 = 132,
        Wrap5 = 133,
        Wrap6 = 134,
        Wrap7 = 135,
        Lighting = 137,
        Ambient = 139,
        FogVertexMode = 140,
        ColorVertex = 141,
        LocalViewer = 142,
        NormalizeNormals = 143,
        DiffuseMaterialSource = 145,
        SpecularMaterialSource = 146,
        AmbientMaterialSource = 147,
        EmissiveMaterialSource = 148,
        VertexBlend = 151,
        ClipPlaneEnable = 152,
        PointSize = 154,
        PointSizeMin = 155,
        PointSpriteEnable = 156,
        PointScaleEnable = 157,
        PointScaleA = 158,
        PointScaleB = 159,
        PointScaleC = 160,
        MultisampleAntialias = 161,
        MultisampleMask = 162,
        PatchEdgeStyle = 163,
        DebugMonitorToken = 165,
        PointSizeMax = 166,
        IndexedVertexBlendEnable = 167,
        ColorWriteEnable = 168,
        TweenFactor = 170,
        PositionDegree = 172,
        NormalDegree = 173,
        ScissorTestEnable = 174,
        SlopeScaleDepthBias = 175,
        AntialiasedLineEnable = 176,
        MinTessellationLevel = 178,
        MaxTessellationLevel = 179,
        AdaptiveTessX = 180,
        AdaptiveTessY = 181,
        AdaptiveTessZ = 182,
        AdaptiveTessW = 183,
        EnableAdaptiveTessellation = 184,
        TwoSidedStencilMode = 185,
        CcwStencilFail = 186,
        CcwStencilZFail = 187,
        CcwStencilPass = 188,
        CcwStencilFunc = 189,
        ColorWriteEnable1 = 190,
        ColorWriteEnable2 = 191,
        ColorWriteEnable3 = 192,
        SrgbWriteEnable = 194,
        DepthBias = 195,
        Wrap8 = 198,
        Wrap9 = 199,
        Wrap10 = 200,
        Wrap11 = 201,
        Wrap12 = 202,
        Wrap13 = 203,
        Wrap14 = 204,
        Wrap15 = 205,
        SeparateAlphaBlendEnable = 206,
       */
#pragma warning restore 1591
    }


    /// <summary>
    /// Specifies the per-pixel blend-operation to perform when alpha blending is enabled.
    /// </summary>
    public enum BlendOperation
    {
#pragma warning disable 1591
        Add = 1,
        Subtract = 2,
        ReverseSubtract = 3,
        Minimum = 4,
        Maximum = 5,
#pragma warning restore 1591
    }

    /* TODO: Implement texture wrapping rather as a texture property than a "global" render state. This is most
     * convenient to implment with OpenGL/TK and easier to mimic in DirectX than the other way round.
    /// <summary>
    /// Bitwise combinations of these values specify if texure wrapping along the given texture coordinate axes is performed. 
    /// With two- or three-dimensional textures Coordinate0, Coordinate1, and Coordinate2 are typically called U, V, and W, respectively.
    /// With four dimensional textures the coordinate axes are commonly called s, t, r, q.
    /// </summary>
    [Flags]
    public enum TextureWrapping
    {
        WrapCoordinate0 = 1,
        WrapU = 1,
        WrapCoordinate1 = 2,
        WrapV = 2,
        WrapCoordinate2 = 4,
        WrapW = 4,
        WrapCoordinate3 = 8,
        None = 0,
    }
    */

    /// <summary>
    /// Specifies the comparison tests to perform in Z-Buffer and in Stencil tests
    /// </summary>
    public enum Compare
    {
#pragma warning disable 1591
        Never = 1,
        Less = 2,
        Equal = 3,
        LessEqual = 4,
        Greater = 5,
        NotEqual = 6,
        GreaterEqual = 7,
        Always = 8,
#pragma warning restore 1591
    }

    /// <summary>
    /// Specifies if and how triangle culling should be performed. If no culling is specified, triangles are rendered, no matter how they are oriented.
    /// Clockwise and Counterclockwise culling refers to in which order a triangle's vertices appear on the screen. If either is specified, triangles
    /// in the respective order are culled.
    /// </summary>
    public enum Cull
    {
#pragma warning disable 1591
        None = 1,
        Clockwise = 2,
        Counterclockwise = 3,
#pragma warning restore 1591
    }
    
    /// <summary>
    /// Used to define the source and destination blending factors. Together, these specify the blend operation to be used when writing a pixel color value onto the render canvas.
    /// </summary>
    public enum Blend
    {
#pragma warning disable 1591
        Zero = 1,
        One = 2,
        SourceColor = 3,
        InverseSourceColor = 4,
        SourceAlpha = 5,
        InverseSourceAlpha = 6,
        DestinationAlpha = 7,
        InverseDestinationAlpha = 8,
        DestinationColor = 9,
        InverseDestinationColor = 10,
 
        BlendFactor = 14,
        InverseBlendFactor = 15,
        // Ignored by FUSEE
        // SourceAlphaSaturated = 11,
        // Bothsrcalpha = 12,
        // BothInverseSourceAlpha = 13,
        // SourceColor2 = 16,
        // InverseSourceColor2 = 17,
#pragma warning restore 1591
    }

        /// <summary>
        /// Specifies the fill mode to use by the rasterizer. Options are Point (renders vertices as single pixels), Wireframe (renders only triangles' edges as lines), or Solid (fills all pixels covered by triangles).
        /// </summary>
    public enum FillMode
    {
#pragma warning disable 1591
        Point = 1,

        Wireframe = 2,
        Solid = 3,
#pragma warning restore 1591
    }
    // These values are binary compatible with ClearBufferMask
    /// <summary>
    ///Specifies the buffer to use when calling the Clear method. 
    /// </summary>
    [Flags]
    public enum ClearFlags : int
    {
#pragma warning disable 1591
        Depth = ((int)0x00000100),
        Accum = ((int)0x00000200),
        Stencil = ((int)0x00000400),
        Color = ((int)0x00004000),
#pragma warning restore 1591
    }

    //[Flags]
    /// <summary>
    /// Specifies the possible key values on a keyboard. 
    /// </summary>
    public enum KeyCodes : int
    {
#pragma warning disable 1591
        KeyCode = 65535,
        Modifiers = -65536,
        None = 0,
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        XButton1 = 5,
        XButton2 = 6,
        Back = 8,
        Tab = 9,
        LineFeed = 10,
        Clear = 12,
        Return = 13,
        Enter = 13,
        Shift = 16,
        Control = 17,
        Menu = 18,
        Pause = 19,
        Capital = 20,
        CapsLock = 20,
        KanaMode = 21,
        HanguelMode = 21,
        HangulMode = 21,
        JunjaMode = 23,
        FinalMode = 24,
        HanjaMode = 25,
        KanjiMode = 25,
        Escape = 27,
        IMEConvert = 28,
        IMENonconvert = 29| LButton,
        IMEAccept = 30,
        IMEModeChange = 31,
        Space = 32,
        Prior = 31,
        PageUp = 31,
        Next = 34,
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,

        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,

        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,

        LWin = 91,
        RWin = 92,
        Apps = 93,

        Sleep = 95,

        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,

        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,

        NumLock = 144,
        Scroll = 145,
        LShift = 160,
        RShift = 161,
        LControl = 162,
        RControl = 163,
        LMenu = 164,
        RMenu = 165,
        BrowserBack = 166,
        BrowserForward = 167,
        BrowserRefresh = 168,
        BrowserStop = 169,
        BrowserSearch = 170,
        BrowserFavorites = 171,
        BrowserHome = 172,
        VolumeMute = 173,
        VolumeDown = 174,
        VolumeUp = 175,
        MediaNextTrack = 176,
        MediaPreviousTrack = 177,
        MediaStop = 178,
        MediaPlayPause = 179,
        LaunchMail = 180,
        SelectMedia = 181,
        LaunchApplication1 = 182,
        LaunchApplication2 = 183,

        OemSemicolon = 186,
        Oem1 = 186,
        OemPlus = 187,
        OemComma = 188,
        OemMinus = 189,
        OemPeriod = 190,
        OemQuestion = 191,
        Oem2 = 191,
        Oemtilde = 192,
        Oem3 = 192,

        OemOpenBrackets = 219,
        Oem4 = 219,
        OemPipe = 220,
        Oem5 = 220,
        OemCloseBrackets = 221,
        Oem6 = 221,
        OemQuotes = 222,
        Oem7 = 222,
        Oem8 = 223,

        OemBackslash = 226,
        Oem102 = 226,

        Process = 229,

        Packet = 231,

        Attn = 246,
        Crsel = 247,

        Exsel = 252,
        EraseEof = 253,
        Play = 254,
        Zoom = 255,
        NoName = 256,
        Pa1 = 257,
        OemClear = 258,
        ShiftModifier = 65536,
        ControlModifier = 131072,
        AltModifier = 262144,
#pragma warning restore 1591
    }

    /// <summary>
    /// Which mouse button was pressed.
    /// </summary>
    [Flags]
    public enum MouseButtons : int
    {
#pragma warning disable 1591
        Unknown = 0,
        Left = 1,
        Right = 2,
        Middle = 4,
#pragma warning restore 1591
    }


    /// <summary>
    /// Specifies the axis types for mouse devices. 
    /// </summary>
    public enum MouseAxes : int
    {
#pragma warning disable 1591
        Unknown,
        X,
        Y,
        Wheel,
        MinX,
        MaxX,
        MinY,
        MaxY,
#pragma warning restore 1591
    }



    /// <summary>
    /// The different cursor types directly supported by render canvas implementations
    /// Standard pointer and link-hand cursor are currently supported.
    /// </summary>
    public enum CursorType : int
    {
#pragma warning disable 1591
        Standard,
        Hand,
#pragma warning restore 1591
    }

    /// <summary>
    /// Game controller axis ids
    /// </summary>
    public enum ControllerAxis
    {
#pragma warning disable 1591
        LeftX,
        LeftY,
        RightX,
        RightY,
        Z
#pragma warning restore 1591
    }

    /// <summary>
    /// Game controller button Ids.
    /// </summary>
    public enum ControllerButton
    {
#pragma warning disable 1591
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        X = 5,
        Y = 6,
        

        R1,
        R2,
        L1,
        L2,

        Start,
        Back,
        Home,
        //...

        FirstUserButton,
#pragma warning restore 1591
    }

    /// <summary>
    /// Axis types for Touch devices. Use <code>(int) (Touchpoint_0_X/Y + 2*i)</code>
    /// if not enough axes are handled by enum values.
    /// </summary>
    public enum TouchAxes : int
    {
        // ReSharper disable InconsistentNaming
#pragma warning disable 1591
        Unknown,
        ActiveTouchpoints,
        MinX,
        MaxX,
        MinY,
        MaxY,
        Touchpoint_0_X,
        Touchpoint_0_Y,
        Touchpoint_1_X,
        Touchpoint_1_Y,
        Touchpoint_2_X,
        Touchpoint_2_Y,
        Touchpoint_3_X,
        Touchpoint_3_Y,
        Touchpoint_4_X,
        Touchpoint_4_Y,
        Touchpoint_5_X,
        Touchpoint_5_Y,
        // More touchpoints (if supported) can be reached by 
        // (int) Touchpoint0 + i)
#pragma warning restore 1591
        // Resharper restore InconsistentNaming
    }

    /// <summary>
    /// Button Ids for Touch devices. Use <code>(int) (Touchpoint_0 + i)</code>
    /// if not enough axes are handled by enum values.
    /// </summary>
    public enum TouchPoints : int
    {
        // ReSharper disable InconsistentNaming
#pragma warning disable 1591
        Unknown,
        Touchpoint_0,
        Touchpoint_1,
        Touchpoint_2,
        Touchpoint_3,
        Touchpoint_4,
        Touchpoint_5,
        // More touchpoints (if supported) can be reached by 
        // (int) Touchpoint0 + i)
#pragma warning restore 1591
        // Resharper restore InconsistentNaming
    }
}
