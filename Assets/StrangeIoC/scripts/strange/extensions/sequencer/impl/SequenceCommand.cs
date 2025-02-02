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

/**
 * @class strange.extensions.sequencer.impl.SequenceCommand
 * 
 * @deprecated
 * 
 * @see strange.extensions.command.api.ICommand
 */

using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.sequencer.api;

namespace StrangeIoC.scripts.strange.extensions.sequencer.impl
{
	public class SequenceCommand : Command, ISequenceCommand
	{
		[Inject]
		public ISequencer sequencer{ get; set;}

		public SequenceCommand ()
		{
		}

		new public void Fail ()
		{
			if (sequencer != null)
			{
				sequencer.Stop (this);
			}
		}

		new virtual public void Execute ()
		{
			throw new SequencerException ("You must override the Execute method in every SequenceCommand", SequencerExceptionType.EXECUTE_OVERRIDE);
		}

		new public void Release ()
		{
			retain = false;
			if (sequencer != null)
			{
				sequencer.ReleaseCommand (this);
			}
		}
	}
}

