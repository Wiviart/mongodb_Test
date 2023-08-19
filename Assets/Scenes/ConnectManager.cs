using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEditor.SearchService;

public class ConnectManager : MonoBehaviour
{
    internal static ConnectManager Instance;

    string appId = "application-0-rbsuv";
    string serviceName = "mongodb-atlas";
    string databaseName = "Test";
    string collectionName = "Users";
    App app;
    User user;

    internal enum ConnectState
    {
        IsLoading,
        IsLogin,
        IsRegister,
        IsDialog
    }
    [SerializeField] ConnectState connectState = ConnectState.IsLoading;

    internal Action OnConnectStateChanged_Login;
    internal Action OnConnectStateChanged_Register;
    internal Action<string> OnConnectStateChanged_Dialog;
    internal Action OnConnectStateChanged_Loading;

    string message;

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

    internal void SetConnectState(ConnectState state)
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

    async void Register(string email, string password)
    {
        SetConnectState(ConnectState.IsLoading);

        try
        {
            await app.EmailPasswordAuth.RegisterUserAsync(email, password);
            message = "Register Success";
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        SetConnectState(ConnectState.IsDialog);
        SetConnectState(ConnectState.IsRegister);
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }

    //***************************************************************/
    //***************************************************************/

    internal async void Login(string email, string password)
    {
        SetConnectState(ConnectState.IsLoading);

        try
        {
            Credentials mailCredentials = Credentials.EmailPassword(email, password);
            user = await app.LogInAsync(mailCredentials);

            if (user != null)
            {
                Scene_Manager.Instance.LoadScene(Scene_Manager.SceneName.SampleScene.ToString());
            }
            else
            {
                message = "Email is not registered or password is incorrect";
            }
        }
        catch (Exception e)
        {
            message = e.Message;
        }

        SetConnectState(ConnectState.IsDialog);
        SetConnectState(ConnectState.IsLogin);
    }

    //***************************************************************/
    //***************************************************************/

    internal async void GetDataUser()
    {
        var user = app.CurrentUser;
        var collection = user.GetMongoClient(serviceName).GetDatabase(databaseName).GetCollection(collectionName);
        var documents = await collection.FindOneAsync(new { uid = user.Id });

        foreach (var document in documents)
        {
            Debug.Log(document.ToString());
        }
    }

    //***************************************************************/
    //***************************************************************/

}
