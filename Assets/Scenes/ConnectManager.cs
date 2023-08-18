using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;

public class ConnectManager : MonoBehaviour
{
    string appId = "application-0-rbsuv";
    App app;

    public enum ConnectState
    {
        IsLoading,
        IsLogin,
        IsRegister,
        IsDialog
    }
    [SerializeField] ConnectState connectState = ConnectState.IsLoading;

    public Action OnConnectStateChanged_Login;
    public Action OnConnectStateChanged_Register;
    public Action<string> OnConnectStateChanged_Dialog;
    public Action OnConnectStateChanged_Loading;
    string message;

    public static ConnectManager Instance { get; internal set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ConnectToMongoDB();
    }

    //***************************************************************/
    //***************************************************************/

    public void SetConnectState(ConnectState state)
    {
        connectState = state;

        switch (connectState)
        {
            case ConnectState.IsLoading:
                {
                    OnConnectStateChanged_Loading?.Invoke();
                    break;
                }
            case ConnectState.IsLogin:
                {
                    OnConnectStateChanged_Login?.Invoke();
                    break;
                }
            case ConnectState.IsRegister:
                {
                    OnConnectStateChanged_Register?.Invoke();
                    break;
                }
            case ConnectState.IsDialog:
                {
                    OnConnectStateChanged_Dialog?.Invoke(message);
                    break;
                }
        }
    }

    //***************************************************************/
    //***************************************************************/

    void ConnectToMongoDB()
    {
        SetConnectState(ConnectState.IsLoading);

        try
        {
            app = App.Create(appId);
        }
        catch (Exception e)
        {
            message = e.Message;
            SetConnectState(ConnectState.IsDialog);
        }

        SetConnectState(ConnectState.IsLogin);
    }

    //***************************************************************/
    //***************************************************************/

    internal void CheckRegister(string email, string password)
    {
        if (IsValidEmail(email.Trim()))
        {
            if (password.Length >= 8)
            {
                Register(email, password);
            }
            else
            {
                message = "Password must be at least 8 characters";
                SetConnectState(ConnectState.IsDialog);
            }
        }
        else
        {
            message = "Email Invalid";
            SetConnectState(ConnectState.IsDialog);
        }
    }

    public async void Register(string email, string password)
    {
        SetConnectState(ConnectState.IsLoading);

        await app.EmailPasswordAuth.RegisterUserAsync(email, password);

        message = "Register Success";
        SetConnectState(ConnectState.IsDialog);
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    //***************************************************************/
    //***************************************************************/

    public async void Login(string email, string password)
    {
        SetConnectState(ConnectState.IsLoading);

        Credentials mailCredentials = Credentials.EmailPassword(email, password);

        var user = await app.LogInAsync(mailCredentials);

        SetConnectState(ConnectState.IsLogin);
    }

    //***************************************************************/
    //***************************************************************/

    public async void GetDataUser()
    {
        var user = app.CurrentUser;
        var collection = user.GetMongoClient("mongodb-atlas").GetDatabase("Test").GetCollection("Users");
        var documents = await collection.FindOneAsync(new { uid = user.Id });

        foreach (var document in documents)
        {
            Debug.Log(document.ToString());
        }
    }

    //***************************************************************/
    //***************************************************************/

}
