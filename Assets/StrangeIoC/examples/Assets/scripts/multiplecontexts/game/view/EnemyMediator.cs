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

/// Ship mediator
/// =====================
/// Make your Mediator as thin as possible. Its function is to mediate
/// between view and app. Don't load it up with behavior that belongs in
/// the View (listening to/controlling interface), Commands (business logic),
/// Models (maintaining state) or Services (reaching out for data).

using System;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace strange.examples.multiplecontexts.game
{
	public class EnemyMediator : EventMediator
	{
		[Inject]
		public EnemyView view{ get; set;}
		
		public override void OnRegister()
		{
			UpdateListeners(true);
			view.init ();
		}
		
		public override void OnRemove()
		{
			UpdateListeners(false);
		}
		
		private void UpdateListeners(bool value)
		{
			view.dispatcher.UpdateListener(value, EnemyView.CLICK_EVENT, onViewClicked);
			dispatcher.UpdateListener( value, GameEvent.GAME_UPDATE, onGameUpdate);
			dispatcher.UpdateListener( value, GameEvent.GAME_OVER, onGameOver);
			
			dispatcher.AddListener(GameEvent.RESTART_GAME, onRestart);
		}
		
		private void onViewClicked()
		{
			dispatcher.Dispatch(GameEvent.ADD_TO_SCORE, 10);
		}
		
		private void onGameUpdate()
		{
			view.updatePosition();
		}
		
		private void onGameOver()
		{
			UpdateListeners(false);
		}
		
		private void onRestart()
		{
			OnRegister();
		}
	}
}

