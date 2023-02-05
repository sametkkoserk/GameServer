using System;
using UnityEngine;
using strange.extensions.context.impl;

namespace strange.examples.myfirstproject
{
    public class MainRoot : ContextView
    {
        void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new MainContext(this);

        }
    }
}
