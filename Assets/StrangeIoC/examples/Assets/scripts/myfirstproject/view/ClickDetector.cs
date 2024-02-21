/// Just a simple MonoBehaviour Click Detector

using System;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace strange.examples.myfirstproject
{
	public class ClickDetector : EventView
	{
		public const string CLICK = "CLICK";
		
		void OnMouseDown()
		{
			dispatcher.Dispatch(CLICK);
		}
	}
}

