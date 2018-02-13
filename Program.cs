using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KsWare {

	static partial class Console {

		class Program {

			private static void Main(string[] args) {
				
				while (true) {
					Write($"{GetPrompt()}");
					var line = ReadLine();
					switch (line.ToLowerInvariant()) {
						case "":break;
						case "exit": Environment.Exit(0); return;
						case "help":
							WriteLine("KsWare.Console v1.0", ConsoleColor.White);
							WriteLine("EXIT		exits the console.",ConsoleColor.Green);
							WriteLine("HELP		shows help text.", ConsoleColor.Green);
							WriteLine("No plugins loads", ConsoleColor.DarkYellow);
							break;
						default:
							WriteLine("Unknown command! Type HELP to get a list of commands.",ConsoleColor.Red);
							break;
					}
				}
			}

			private static string GetPrompt() {
				return Environment.CurrentDirectory + ">";
			}
		}
	}
}
