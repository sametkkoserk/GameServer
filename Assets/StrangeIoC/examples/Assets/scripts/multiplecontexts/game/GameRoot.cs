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

/// The Root is the entry point to a strange-enabled Unity3D app.
/// ===============
/// 
/// Attach this MonoBehaviour to a GameObject at the top of a scene in game.unity.
/// 
/// Game includes a simple Asteroids-style game.

using System;
using StrangeIoC.scripts.strange.extensions.context.impl;
using UnityEngine;

namespace strange.examples.multiplecontexts.game
{
	public class GameRoot : ContextView
	{
	
		void Awake()
		{
			//Instantiate the context, passing it this instance.
			context = new GameContext(this);
		}
	}
}

