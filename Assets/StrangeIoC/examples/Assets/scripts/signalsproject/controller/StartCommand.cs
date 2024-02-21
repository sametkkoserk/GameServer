/// The only change in StartCommand is that we extend Command, not EventCommand

using System;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace strange.examples.signals
{
	public class StartCommand : Command
	{
		
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView{get;set;}
		
		public override void Execute()
		{
			GameObject go = new GameObject();
			go.name = "ExampleView";
			go.AddComponent<ExampleView>();
			go.transform.parent = contextView.transform;
		}
	}
}

