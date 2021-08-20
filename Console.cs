// ***********************************************************************
// Assembly         : KsWare.Console
// Author           : KayS
// Created          : 01-30-2018
//
// Last Modified By : KayS
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="Console.cs" company="">
//     Copyright © 2018 by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace KsWare {

	/// <inheritdoc cref="System.Console"/>
	public static partial class Console {

		private static CursorPos StoredCursor=new CursorPos();
		static EventHandler _handler;

		static Console() {
			_handler=ConsoleCtrlHandler;
			SetConsoleCtrlHandler(_handler, true);
			SystemEvents.SessionEnding += (s, e) => {
				if(ConsoleCtrlHandler(e.Reason == SessionEndReasons.Logoff ? CTRL.LOGOFF_EVENT : CTRL.SHUTDOWN_EVENT))
					e.Cancel=true;
			};
//			SystemEvents.SessionEnded += (s, e) =>
//				ConsoleCtrlHandler(e.Reason == SessionEndReasons.Logoff ? CTRL.LOGOFF_EVENT : CTRL.SHUTDOWN_EVENT);
		}

		/// <inheritdoc cref="System.Console.Write(string)"/>
		/// <param name="foreground">The foreground color.</param>
		/// <param name="background">The background color.</param>
		public static void Write(string value, ConsoleColor foreground, ConsoleColor background) {
			System.Console.ForegroundColor=foreground;
			System.Console.BackgroundColor=background;
			System.Console.Write(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		/// <param name="foreground">The foreground color.</param>
		public static void WriteLine(string value, ConsoleColor foreground) {
			System.Console.ForegroundColor=foreground;
			System.Console.WriteLine(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		/// <param name="foreground">The foreground color.</param>
		/// <param name="background">The background color.</param>
		public static void WriteLine(string value, ConsoleColor foreground, ConsoleColor background) {
			System.Console.ForegroundColor=foreground;
			System.Console.BackgroundColor=background;
			System.Console.WriteLine(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		public static void Write(string value) { System.Console.Write(value); }

		/// <inheritdoc cref="System.Console.WriteLine(object)"/>
		public static void Write(object value) { System.Console.Write(value); }

		/// <inheritdoc cref="System.Console.ReadLine()"/>
		public static string ReadLine() { return System.Console.ReadLine(); }

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		public static void WriteLine(string value) { System.Console.WriteLine(value); }

		/// <inheritdoc cref="System.Console.WriteLine(object)"/>
		public static void WriteLine(object value) { System.Console.WriteLine(value); }

		/// <inheritdoc cref="System.Console.WriteLine()"/>
		public static void WriteLine() { System.Console.WriteLine(); }

		/// <inheritdoc cref="System.Console.ReadKey(bool)"/>
		public static ConsoleKeyInfo ReadKey(bool intercept=false) { return System.Console.ReadKey(intercept); }

		/// <summary>
		/// Saves the position.
		/// </summary>
		public static void SavePos() => StoredCursor.Save();

		/// <summary>
		/// Restores the position.
		/// </summary>
		public static void RestorePos() => StoredCursor.Restore();

		/// <inheritdoc cref="System.Console.KeyAvailable"/>
		public static bool KeyAvailable => System.Console.KeyAvailable;

		/// <summary>
		/// Shows or hides the Console window.
		/// </summary>
		/// <param name="show">true to show; false to hide</param>
		public static void ShowWindow(bool show=true) {
			const int SW_HIDE = 0;
			const int SW_SHOW = 5;
			var       handle  = GetConsoleWindow();
			if (handle == IntPtr.Zero && show) {
				AllocConsole();
				handle = GetConsoleWindow();
			}
			if (handle == IntPtr.Zero) {
				throw new InvalidOperationException("No console window available!");
			}
			ShowWindow(handle, show ? SW_SHOW : SW_HIDE);
		}

		/// <summary>
		/// Hides the Console window.
		/// </summary>
		public static void HideWindow() => ShowWindow(false);

		/// <summary>
		/// Gets the console window.
		/// </summary>
		/// <returns>IntPtr.</returns>
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		/// <summary>
		/// Shows the window.
		/// </summary>
		/// <param name="hWnd">The h WND.</param>
		/// <param name="nCmdShow">The n command show.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("Kernel32")]
		private static extern void AllocConsole();

		[DllImport("Kernel32")]
		private static extern void FreeConsole();

		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

		private delegate bool EventHandler(CTRL sig);


		enum CTRL {
			C_EVENT = 0,
			BREAK_EVENT = 1,
			CLOSE_EVENT = 2, // no cancel available
			LOGOFF_EVENT = 5,
			SHUTDOWN_EVENT = 6
		}

		private static bool ConsoleCtrlHandler(CTRL sig) {
			WriteLine($"Exiting system due to external signal. CTRL_{sig}");
//			File.AppendAllText(@"Console.log", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} CTRL_{sig}");
			var e = new ConsoleExitEventArgs((ConsoleExitReason) sig);
			Exit?.Invoke(null,e);
			if (e.Cancel) {
				WriteLine("Exit canceled.");
				return false;
			}

			WriteLine("Cleanup complete");
			//shutdown right away so there are no lingering threads
			Environment.Exit(0);
			return true;
		}

		public static event EventHandler<ConsoleExitEventArgs> Exit;

	}

}
