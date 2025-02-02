/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

namespace StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api
{
	public enum EventDispatcherExceptionType
	{
		/// Indicates that an event was fired with null as the key.
		EVENT_KEY_NULL,
		
		/// Indicates that the type of Event in the call and the type of Event in the payload don't match.
		EVENT_TYPE_MISMATCH,

		/// When attempting to fire a callback, the callback was discovered to be casting illegally.
		TARGET_INVOCATION
	}
}

