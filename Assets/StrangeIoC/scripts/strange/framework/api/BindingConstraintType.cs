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

namespace StrangeIoC.scripts.strange.framework.api
{
	public enum BindingConstraintType
	{
		/// Constrains a SemiBinding to carry no more than one item in its Value
		ONE,
		/// Constrains a SemiBinding to carry a list of items in its Value
		MANY,
		/// Instructs the Binding to apply a Pool instead of a SemiBinding
		POOL,
	}
}

