// ***********************************************************************
// Assembly         : KsWare.Console
// Author           : KayS
// Created          : 01-30-2018
//
// ***********************************************************************
// <copyright file="Console.cs" company="">
//     Copyright © 2018 by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.CodeAnalysis;
using KsWare.Internal;
using KsWare.WinApi;
using Microsoft.Win32;

// INFO Microsoft.Win32.SystemEvents is not in netstandard

namespace KsWare {

	/// <inheritdoc cref="System.Console"/>
	public static partial class Console {

		private static readonly CursorPos StoredCursor = new CursorPos();

		[SuppressMessage("ReSharper", "PrivateFieldCanBeConvertedToLocalVariable")] 
		static readonly ConsoleCtrlHandler Handler;

		static Console() {
			Handler = ConsoleCtrlHandler;
			ConsoleApi.SetConsoleCtrlHandler(Handler, true);
			SystemEvents.SessionEnding += SystemEvents_SessionEnding;
		}

		private static void SystemEvents_SessionEnding(object s, SessionEndingEventArgs e) {
			if /*cancel?*/ (ConsoleCtrlHandler(e.Reason == SessionEndReasons.Logoff ? CTRL.LOGOFF_EVENT : CTRL.SHUTDOWN_EVENT)) e.Cancel = true;
			else SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
		}

		/// <inheritdoc cref="System.Console.Write(string)"/>
		/// <param name="foreground">The foreground color.</param>
		/// <param name="background">The background color.</param>
		public static void Write(string value, ConsoleColor foreground, ConsoleColor background) {
			System.Console.ForegroundColor = foreground;
			System.Console.BackgroundColor = background;
			System.Console.Write(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		/// <param name="foreground">The foreground color.</param>
		public static void WriteLine(string value, ConsoleColor foreground) {
			System.Console.ForegroundColor = foreground;
			System.Console.WriteLine(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		/// <param name="foreground">The foreground color.</param>
		/// <param name="background">The background color.</param>
		public static void WriteLine(string value, ConsoleColor foreground, ConsoleColor background) {
			System.Console.ForegroundColor = foreground;
			System.Console.BackgroundColor = background;
			System.Console.WriteLine(value);
			System.Console.ResetColor();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		public static void Write(string value) {
			System.Console.Write(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(object)"/>
		public static void Write(object value) {
			System.Console.Write(value);
		}

		/// <inheritdoc cref="System.Console.ReadLine()"/>
		public static string ReadLine() {
			return System.Console.ReadLine();
		}

		/// <inheritdoc cref="System.Console.WriteLine(string)"/>
		public static void WriteLine(string value) {
			System.Console.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine(object)"/>
		public static void WriteLine(object value) {
			System.Console.WriteLine(value);
		}

		/// <inheritdoc cref="System.Console.WriteLine()"/>
		public static void WriteLine() {
			System.Console.WriteLine();
		}

		/// <inheritdoc cref="System.Console.ReadKey(bool)"/>
		public static ConsoleKeyInfo ReadKey(bool intercept = false) {
			return System.Console.ReadKey(intercept);
		}

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
		public static void ShowWindow(bool show = true) {
			const int SW_HIDE = 0;
			const int SW_SHOW = 5;
			var handle = ConsoleApi.GetConsoleWindow();
			if (handle == IntPtr.Zero && show) {
				ConsoleApi.AllocConsole();
				handle = ConsoleApi.GetConsoleWindow();
			}

			if (handle == IntPtr.Zero) {
				throw new InvalidOperationException("No console window available!");
			}

			ConsoleApi.ShowWindow(handle, show ? SW_SHOW : SW_HIDE);
		}

		/// <summary>
		/// Hides the Console window.
		/// </summary>
		public static void HideWindow() => ShowWindow(false);



		private static bool ConsoleCtrlHandler(CTRL sig) {
			WriteLine($"Exiting system due to external signal. CTRL_{sig}");
//			File.AppendAllText(@"Console.log", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} CTRL_{sig}");
			var e = new ConsoleExitEventArgs((ConsoleExitReason)sig);
			Exit?.Invoke(null, e);
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
