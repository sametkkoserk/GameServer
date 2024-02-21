/// Uses a signal instead of an EventDispatcher

using System;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using StrangeIoC.scripts.strange.extensions.signal.impl;
using UnityEngine;

namespace strange.examples.signals
{
	public class ClickDetector : View
	{
		// Note how we're using a signal now
		public Signal clickSignal = new Signal();
		
		void OnMouseDown()
		{
			clickSignal.Dispatch();
		}
	}
}

