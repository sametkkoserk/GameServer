/// Signal fired when the score changes
/// 
/// string The new score (already formatted)

using System;
using StrangeIoC.scripts.strange.extensions.signal.impl;

namespace strange.examples.signals
{
	public class ScoreChangedSignal : Signal<string>
	{
	}
}

