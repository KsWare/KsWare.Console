using System.ComponentModel;

namespace KsWare {

	public class ConsoleExitEventArgs : CancelEventArgs {

		public ConsoleExitEventArgs(ConsoleExitReason exitReason) {
			ExitReason = exitReason;
		}
		public ConsoleExitReason ExitReason { get; }
	}

}