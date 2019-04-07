﻿using System;
using System.Collections.Generic;
using Unity.UIWidgets.widgets;
using Datas;
using UIScripts.LoginPage;
using Unity.UIWidgets.Redux;

namespace UIScripts
{
    public class Home : StatelessWidget
    {
        public override Widget build(BuildContext buildContext)
        {
            return new StoreConnector<AppState, bool>(
                converter: (state) => state.WasLogined,
                builder: ((context, model, dispatcher) =>
                    model ? new SizedBox(child: new MainPage()) : new SizedBox(child: new WelcomePage())
                )
            );     
        }
    }
}