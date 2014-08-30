using System;
using System.Windows.Forms;
using OpenTK;

namespace WindViewer.Editor
{
    public class Input
    {
        /// <summary> Mouse position in pixel coordinates. Read only. </summary>
        public static Vector3 MousePosition { get; private set; }
        /// <summary> Delta position in pixel coordinates between frames. Read only. </summary>
        public static Vector3 MouseDelta { get; private set; }

        /// <summary> Keys currently down this frame. </summary>
        private static readonly bool[] _keysDown = new bool[256];
        /// <summary> Keys that were down last frame. </summary>
        private static readonly bool[] _prevKeysDown = new bool[256];

        private static readonly bool[] _mouseBtnsDown = new bool[3];
        private static readonly bool[] _prevMouseBtnsDown = new bool[3];
        private static Vector3 _prevMousePos;

        public static bool GetKey(Keys key)
        {
            return _keysDown[(int)key];
        }

        public static bool GetKeyDown(Keys key)
        {
            return _keysDown[(int)key] && !_prevKeysDown[(int)key];
        }

        public static bool GetKeyUp(Keys key)
        {
            return _prevKeysDown[(int)key] && !_keysDown[(int)key];
        }

        public static bool GetMouseButton(int button)
        {
            return _mouseBtnsDown[button];
        }

        public static bool GetMouseButtonDown(int button)
        {
            return _mouseBtnsDown[button] && !_prevMouseBtnsDown[button];
        }

        public static bool GetMouseButtonUp(int button)
        {
            return _prevMouseBtnsDown[button] && !_mouseBtnsDown[button];
        }

        internal static void Internal_UpdateInputState()
        {
            for (int i = 0; i < 256; i++)
                _prevKeysDown[i] = _keysDown[i];

            for (int i = 0; i < 3; i++)
                _prevMouseBtnsDown[i] = _mouseBtnsDown[i];

            MouseDelta = MousePosition - _prevMousePos;
            _prevMousePos = MousePosition;
        }

        internal static void Internal_SetKeyState(Keys keyCode, bool bPressed)
        {
            _keysDown[(int)keyCode] = bPressed;
        }

        internal static void Internal_SetMouseBtnState(MouseButtons button, bool bPressed)
        {
            _mouseBtnsDown[MouseButtonEnumToInt(button)] = bPressed;
        }

        internal static void Internal_SetMousePos(Vector2 mousePos)
        {
            MousePosition = new Vector3(mousePos.X, mousePos.Y, 0);
        }

        private static int MouseButtonEnumToInt(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return 0;
                case MouseButtons.Right:
                    return 1;
                case MouseButtons.Middle:
                    return 2;
            }

            Console.WriteLine("Unknown Mouse Button enum {0}, returning Left!");
            return 0;
        }

    }
}