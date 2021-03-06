using System.Collections.Generic;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace UIScripts.LoginPage
{
    public class LoginPage : StatelessWidget
    {
        private readonly TextEditingController PasswordEdit = new TextEditingController("");
        private readonly TextEditingController PhoneEdit = new TextEditingController("");

        public override Widget build(BuildContext context)
        {
            return new Scaffold(
                appBar: HelperWidgets._buildCloseAppBar(isCenterTitle: true, title: new Text(""),
                    lealding: new StoreConnector<AppState, object>(
                        converter: (state) => null,
                        builder: ((buildContext, model, dispatcher) => new IconButton(
                            icon: new Icon(icon: Icons.close),
                            onPressed: () =>
                            {
                                dispatcher.dispatch(new LoginState()
                                {
                                    userOpCode = UserOpCodeEnum.Close,
                                    Context = context
                                });
                            })),
                        pure: true
                    )),
                body: _buildBody()
            );
        }

        private Widget _buildBody()
        {
            return new Container(
                color: CustomTheme.CustomTheme.EDColor,
                child: new ListView(
                    children: new List<Widget>
                    {
                        new Container(
                            margin: EdgeInsets.only(top: 100, left: 20),
                            child: new Text("使用手机号码登录", style: new TextStyle(fontWeight: FontWeight.w500, fontSize: 20))
                        ),

                        new Padding(padding: EdgeInsets.only(top: 20)),

                        new StoreConnector<AppState, string>(
                            converter: (state) => state.Account,
                            builder: ((context, model, dispatcher) =>
                                new TextFieldExtern("请填写手机号码",
                                    margin: EdgeInsets.all(20),
                                    obscureText: false, editingController: PhoneEdit, maxLength: 11,
                                    regexCondition: @"^[0-9]*$",
                                    onChanged: (text) =>
                                    {
                                        dispatcher.dispatch(new AccountState() {InputResult = text});
                                    })
                            )
                        ),


                        new StoreConnector<AppState, string>(
                            converter: (state) => state.Password,
                            builder: ((context, model, dispatcher) =>
                                new TextFieldExtern("请填写密码(英文字符、数字)",
                                    margin: EdgeInsets.all(20),
                                    obscureText: true, editingController: PasswordEdit, maxLength: 16,
                                    regexCondition: @"^[A-Za-z0-9]+$",
                                    onChanged: (text) =>
                                    {
                                        dispatcher.dispatch(new PasswordState() {InputResult = text});
                                    }))
                        ),


                        new StoreConnector<AppState, AppState>(
                            converter: (state) => state,
                            builder: ((context, model, dispatcher) =>
                                new Container(
                                    margin: EdgeInsets.all(20f),
                                    width: MediaQuery.of(context).size.width,
                                    child: new FlatButton(color: Colors.green,
                                        child: new Text("登录",
                                            style: CustomTheme.CustomTheme.DefaultTextThemen.display2),
                                        onPressed: () =>
                                        {
                                            //防止用户漏空提交
                                            if (string.IsNullOrEmpty(model.Account) ||
                                                string.IsNullOrEmpty(model.Password))
                                            {
                                                return;
                                            }

                                            dispatcher.dispatch(new LoginState()
                                            {
                                                Context = context,
                                                userOpCode = UserOpCodeEnum.None,
                                                RequestOpCode = RequestOpCodeEnum.RequestLogin
                                            });
                                        }
                                    )
                                )
                            ),
                            pure: true
                        ),

                        new StoreConnector<AppState, object>(
                            converter: (state) => null,
                            builder: ((context, model, dispatcher) => new Container(
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
                            )
                        )
                    }
                )
            );
        }
    }
}