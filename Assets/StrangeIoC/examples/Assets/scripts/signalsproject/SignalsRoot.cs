/// If you're new to Strange, start with MyFirstProject.
/// If you're interested in how Signals work, return here once you understand the
/// rest of Strange. This example shows how Signals differ from the default
/// EventDispatcher.

using System;
using StrangeIoC.scripts.strange.extensions.context.impl;
using UnityEngine;

namespace strange.examples.signals
{
	public class SignalsRoot : ContextView
	{
	
		void Awake()
		{
			context = new SignalsContext(this);
		}
	}
}

