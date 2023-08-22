using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using System;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using System.Threading.Tasks;

public class ConnectDB : MonoBehaviour
{
    internal static ConnectDB Instance;

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

    //***************************************************************/
    //***************************************************************/

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (!Instance) Instance = this;
        else Destroy(gameObject);
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
                await GetDataUser();

                if (playerData != null)
                {
                    message = "Login Success";

                    LoadMainMenu();
                }
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
    void LoadMainMenu()
    {
        Scene_Manager.Instance.LoadScene(SceneName.MainMenu);
    }

    //***************************************************************/
    //***************************************************************/

    PlayerData playerData;
    MongoClient.Collection<PlayerData> collections;

    void ConnectToCollection()
    {
        User user = app.CurrentUser;

        collections = user.GetMongoClient(serviceName).GetDatabase(databaseName).GetCollection<PlayerData>(collectionName);
    }

    async Task GetDataUser()
    {
        if (collections == null)
            ConnectToCollection();

        try
        {
            playerData = await collections.FindOneAsync(new { UserID = user.Id });
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    internal PlayerData GetUserData()
    {
        return playerData;
    }

    //***************************************************************/
    //***************************************************************/

    public async void UpdateUserName()
    {
        if (collections == null)
            ConnectToCollection();

        try
        {
            await collections.UpdateOneAsync(new { UserID = user.Id }, new BsonDocument { { "$set", new BsonDocument { { "name", playerData.Name } } } });
            Debug.Log("Update Success");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    internal void UpdateUserName(string name)
    {
        playerData.Name = name;

        UpdateUserName();
    }
}
