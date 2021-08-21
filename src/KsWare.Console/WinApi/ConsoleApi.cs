using System;
using System.Runtime.InteropServices;

namespace KsWare.WinApi {

	internal static class ConsoleApi {

		/// <summary>
		/// Gets the console window.
		/// </summary>
		/// <returns>IntPtr.</returns>
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetConsoleWindow();

		/// <summary>
		/// Shows the window.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="nCmdShow">The n command show.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("Kernel32")]
		internal static extern void AllocConsole();

		[DllImport("Kernel32")]
		internal static extern void FreeConsole();

		[DllImport("Kernel32")]
		internal static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handler, bool add);
	}

	internal delegate bool ConsoleCtrlHandler(CTRL sig);

	internal enum CTRL {
		C_EVENT = 0,
		BREAK_EVENT = 1,
		CLOSE_EVENT = 2, // no cancel available
		LOGOFF_EVENT = 5,
		SHUTDOWN_EVENT = 6
	}
}
