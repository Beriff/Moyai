using Moyai.Impl.Graphics;
using Moyai.Impl.Math;

using System.Drawing;
using System.Runtime.InteropServices;

namespace Moyai.Impl.Input
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct WinPoint
	{
		public int x;
		public int y;
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct WinRect
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct Coord
	{
		public short x;
		public short y;
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct CONSOLE_FONT_INFO
	{
		public int nFont;
		public Coord dwFontSize;
	}
	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSE_EVENT_RECORD
	{
		[FieldOffset(0)]
		public Coord MousePosition;
		[FieldOffset(4)]
		public uint ButtonState;
		[FieldOffset(8)]
		public uint ControlKeyState;
		[FieldOffset(12)]
		public uint EventFlags;
	}
	[StructLayout(LayoutKind.Explicit)]
	internal struct KEY_EVENT_RECORD
	{
		[FieldOffset(0)]
		public bool KeyDown;
		[FieldOffset(4)]
		public ushort RepeatCount;
		[FieldOffset(6)]
		public ushort VirtualKeyCode;
		[FieldOffset(8)]
		public ushort VirtualScanCode;
		[FieldOffset(10)]
		public char UnicodeChar;
		[FieldOffset(12)]
		public int ControlKeyState;
	}
	[StructLayout(LayoutKind.Explicit)]
	internal struct INPUT_RECORD
	{
		[FieldOffset(0)]
		public ushort EventType;
		[FieldOffset(4)]
		public KEY_EVENT_RECORD KeyEvent;
		[FieldOffset(4)]
		public MOUSE_EVENT_RECORD MouseEvent;
	}

	public enum Keys
	{
		MouseLeft = 0x01,
		MouseRight = 0x02,
		MouseMiddle = 0x04,
		Backspace = 0x08,
		Tab = 0x09,
		Enter = 0x0D,
		Shift = 0x10,
		Control = 0x11,
		Alt = 0x12,
		CapsLock = 0x14,
		Esc = 0x1B,
		Space = 0x20,
		PageUp = 0x21,
		PageDown = 0x22,
		End = 0x23,
		Home = 0x24,
		ArrowLeft = 0x25,
		ArrowUp = 0x26,
		ArrowRight = 0x27,
		ArrowDown = 0x28,
		Del = 0x2E,
		_0 = 0x30,
		_1 = 0x31,
		_2 = 0x32,
		_3 = 0x33,
		_4 = 0x34,
		_5 = 0x35,
		_6 = 0x36,
		_7 = 0x37,
		_8 = 0x38,
		_9 = 0x39,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47,
		H = 0x48,
		I = 0x49,
		J = 0x4A,
		K = 0x4B,
		L = 0x4C,
		M = 0x4D,
		N = 0x4E,
		O = 0x4F,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5A,
		Num0 = 0x60,
		Num1 = 0x61,
		Num2 = 0x62,
		Num3 = 0x63,
		Num4 = 0x64,
		Num5 = 0x65,
		Num6 = 0x66,
		Num7 = 0x67,
		Num8 = 0x68,
		Num9 = 0x69,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		NumLock = 0x90,
		ScrollLock = 0x91
	}
	public enum InputType
	{
		Pressed, Released,
		JustPressed, JustReleased
	}

	public static class InputHandler
	{
		private const uint ENABLE_QUICK_EDIT = 0x0040;
		private const int STD_INPUT_HANDLE = -10;
		private const int STD_OUTPUT_HANDLE = -11;
		private static IntPtr ConsoleWindowHandle;
		public static Vec2I FontSize { get; private set; }
		public static int TitleBarHeight { get; private set; }

		[DllImport("user32.dll")]
		private static extern bool GetAsyncKeyState(int button);
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);
		[DllImport("kernel32.dll")]
		private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
		[DllImport("kernel32.dll")]
		private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(out WinPoint point);
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, out WinRect lpRect);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool GetCurrentConsoleFont(IntPtr hConsoleOutput, bool bMaximumWindow, out CONSOLE_FONT_INFO lpConsoleCurrentFontInfo);
		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(int nCmdId);
		[DllImport("kernel32.dll")]
		private static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, out int lpcNumberOfEvents);
		[DllImport("kernel32.dll")]
		private static extern bool PeekConsoleInput(IntPtr hconsoleInput, [Out] INPUT_RECORD[] lpBuffer, int nLength, out int lpNumberOfEventsRead);


		public static Dictionary<Keys, bool> PrevKeysState { get; private set; }
		public static Dictionary<Keys, bool> CurrentKeysState {  get; private set; }
		public static int Scroll { get; private set; }

		private static IntPtr InputConsoleHandle { get; set; }
		private static IntPtr OutputConsoleHandle { get; set; }
		public static Vec2I MousePosPx
		{
			get {
				GetCursorPos(out WinPoint pos);
				GetWindowRect(ConsoleWindowHandle, out WinRect wr);
				return new Vec2I(pos.x, pos.y) - new Vec2I(wr.left, wr.top);
			}
		}
		public static Vec2I MousePos(ConsoleBuffer buf)
		{
			var coords = (MousePosPx - new Vec2I(0, TitleBarHeight)) / FontSize;
			return (coords - new Vec2I(1, 0)).Clamp(new(0, 0), buf.Size - new Vec2I(1));
		}
		public static Vec2I MousePos()
		{
			return (MousePosPx - new Vec2I(0, TitleBarHeight)) / FontSize - new Vec2I(1, 0);
		}
		public static Vec2I WindowSize
		{
			get
			{
				GetWindowRect(ConsoleWindowHandle, out WinRect wr);
				return new(wr.right, wr.bottom);
			}
		}

		static InputHandler()
		{
			PrevKeysState = new();
			CurrentKeysState = new();

			// Set windows handle for console
			ConsoleWindowHandle = GetConsoleWindow();


			InputConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
			OutputConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

			if (!GetCurrentConsoleFont(OutputConsoleHandle, false, out CONSOLE_FONT_INFO font))
				throw new MoyaiException("Unable to get console font");

			if (!GetConsoleMode(InputConsoleHandle, out uint consoleMode))
				throw new MoyaiException("Unable to get console mode");

			// Disable console quick edit
			// (that prevents console hanging when user tries to select)

			// Clear the quick edit bit in the mode flags
			consoleMode &= ~ENABLE_QUICK_EDIT;

			// Set the new mode
			if (!SetConsoleMode(InputConsoleHandle, consoleMode))
				throw new MoyaiException("Unable to set console mode");

			// Get console's font size
			FontSize = new(font.dwFontSize.x, (int)font.dwFontSize.y);
			// Get console window's title bar height
			TitleBarHeight = GetSystemMetrics(30);
		}

		public static bool KeyPressed(Keys key)
		{
			return GetAsyncKeyState((int)key);
		}

		public static void Update()
		{
			Scroll = 0;
			PrevKeysState = new(CurrentKeysState);
			foreach(var key in Enum.GetValues(typeof(Keys)).Cast<Keys>())
			{
				CurrentKeysState[key] = KeyPressed(key);
			}

			// getting mouse scroll kinda just doesn't work...
			/*GetNumberOfConsoleInputEvents(InputConsoleHandle, out int event_count);
			var events = new INPUT_RECORD[event_count];
			PeekConsoleInput(InputConsoleHandle, events, event_count, out int _);

			foreach(var @event in events)
			{
				//mouse event
				if(@event.EventType == 0x0002)
				{
					var m_event = @event.MouseEvent;
					//mouse scroll
					if(m_event.EventFlags != 0x0001)
					{
						if ((m_event.ButtonState >> 16) > 0)
							Scroll++;
						else
							Scroll--;
					}
				}
			}*/

		}

		public static InputType KeyState(Keys key)
		{
			if (CurrentKeysState.Count == 0 || PrevKeysState.Count == 0)
				return InputType.Released;

			if (CurrentKeysState[key])
			{
				if (PrevKeysState[key])
					return InputType.Pressed;
				else
					return InputType.JustPressed;
			}
			else
			{
				if (PrevKeysState[key])
					return InputType.JustReleased;
				else
					return InputType.Released;
			}
		}


	}
}
