using System;
using System.Collections.Generic;
using BestHTTP.JSON;
using Datas;
using UIScripts.Externs;
using Unit;
using Unity.UIWidgets;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using DialogUtils = Unity.UIWidgets.material.DialogUtils;

namespace UIScripts.LoginPage
{
    public class LoginPage : StatelessWidget
    {
        private readonly TextEditingController PasswordEdit = new TextEditingController("");
        private readonly TextEditingController PhoneEdit = new TextEditingController("");
        private BuildContext BuidlContext;


        private string Account;


        private readonly Animation<Color> CircularProgressIndicatorColor =
            new AlwaysStoppedAnimation<Color>(Colors.white);

        public override Widget build(BuildContext context)
        {
            BuidlContext = context;
            return new Scaffold(
                appBar: HelperWidgets._buildCloseAppBar(isCenterTitle: true, title: new Text(""),
                    lealding: new IconButton(
                        icon: new Icon(icon: Icons.close),
                        onPressed: () => { Navigator.pop(context); }
                    )),
                body: _buildBody()
            );
        }

        private Widget _buildBody()
        {
            return new StoreConnector<AppState, AppState>(
                converter: (state) => state,
                builder: ((context, model, dispatcher) => new Container(
                    color: CustomTheme.CustomTheme.EDColor,
                    child: new ListView(
                        children: new List<Widget>
                        {
                            new Container(
                                margin: EdgeInsets.only(top: 100, left: 20),
                                child: new Text("使用手机号码登录",
                                    style: new TextStyle(fontWeight: FontWeight.w500, fontSize: 20))
                            ),

                            new Padding(padding: EdgeInsets.only(top: 20)),

                            new TextFieldExtern("请填写手机号码",
                                margin: EdgeInsets.all(20),
                                obscureText: false, editingController: PhoneEdit, maxLength: 11,
                                regexCondition: @"^[0-9]*$",
                                errorText: string.IsNullOrEmpty(model.Account) ? null :
                                HelperWidgets.IsCellphoneNumber(model?.Account) ? null : string.Empty,
                                onChanged: (text) => { dispatcher.dispatch(new AccountAction {InputResult = text}); }
                            ),

                            new TextFieldExtern("请填写密码(英文字符、数字)",
                                margin: EdgeInsets.all(20),
                                obscureText: true, editingController: PasswordEdit, maxLength: 16,
                                regexCondition: @"^[A-Za-z0-9]*$"
                            ),


                            new Container(
                                margin: EdgeInsets.all(20f),
                                width: MediaQuery.of(context).size.width,
                                child: new FlatButton(color: Colors.green,
                                    child: model.HideCircularProgressIndicator
                                        ? (Widget) new Text(model.LoginState,
                                            style: CustomTheme.CustomTheme.DefaultTextThemen.display2)
                                        : new Row(
                                            mainAxisAlignment: MainAxisAlignment.center,
                                            children: new List<Widget>
                                            {
                                                new CustomCircularProgressIndicator(
                                                    valueColor: CircularProgressIndicatorColor),
                                                new SizedBox(width: 10),
                                                new Text(model.LoginState,
                                                    style: CustomTheme.CustomTheme.DefaultTextThemen
                                                        .display2)
                                            }
                                        ),
                                    onPressed: () =>
                                    {
                                        //防止用户漏空提交
                                        if (!HelperWidgets.IsCellphoneNumber(PhoneEdit.text)
                                            || !HelperWidgets.IsValidPassword(PasswordEdit.text)) return;

                                        //TODO:防止多次点击
                                        dispatcher.dispatch(RequestLogin(context, dispatcher));
                                    }
                                )
                            ),

                            new Container(
                                margin: EdgeInsets.all(40),
                                alignment: Alignment.center,
                                child: new GestureDetector(
                                    onTap: () =>
                                    {
                                        //TODO:找回密码
                                    },
                                    child: new Text("登陆遇到问题？")
                                )
                            )
                        }
                    )
                ))
            );
        }


        private LoginAction RequestLogin(BuildContext context, Dispatcher dispatcher)
        {
            LoginAction tmpLoginAction = new LoginAction();
            tmpLoginAction.webServerApiRequest =
                HelperWidgets.Login(context,
                    autoRequest: false,
                    account: PhoneEdit.text,
                    password: PasswordEdit.text,
                    successedResult: (result) =>
                    {
                        dispatcher.dispatch(new LoginAction
                        {
                            webServerApiRequest = new WebServerApiRequest
                            {
                                RequestResult = RequestResultEnum.LoginSuccessed
                            }
                        });


                        PlayerPrefs.SetString("account", PhoneEdit.text);
                        PlayerPrefs.SetString("password", PasswordEdit.text);
                        PlayerPrefs.SetString("logined", "Yes");
                        HelperWidgets.PopRoute(BuidlContext);
                    }
                );

            return tmpLoginAction;
        }
    }
}