using System;

namespace strange.examples.signals
{
	public interface IExampleService
	{
		void Request(string url);
		//Instead of an EventDispatcher, we put the actual Signals into the Interface
		FulfillWebServiceRequestSignal fulfillSignal{get;}
	}
}

