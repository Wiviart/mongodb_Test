using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;
using Realms.Sync;
using TMPro;

public class ConnectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject RegisterUI;
    [SerializeField]
    private GameObject LoginUI;
    [SerializeField]
    private GameObject LoadingUI;
    [SerializeField]
    private GameObject DialogUI;
    [SerializeField]
    private TMP_InputField emailRegister;
    [SerializeField]
    private TMP_InputField passwordRegister;
    [SerializeField]
    private TMP_InputField emailLogin;
    [SerializeField]    
    private TMP_InputField passwordLogin;

    private string appId = "gameunityappservice-btnvz";
    private App app;
    private User user;
    public async void OnLogin()
    {
        SwitchConnectState(ConnectState.IsLoading);
        Credentials mailCredentials = Credentials.EmailPassword(emailLogin.text,passwordLogin.text);
        user = await app.LogInAsync(mailCredentials);
        //Loading Sence Lobby
        Debug.Log(user.Id);
        GetDataUser();
        //Xoa khi ma toi hoan thanh sence lobby
        SwitchConnectState(ConnectState.IsDialog);
    }
    public async void GetDataUser()
    {
      var collection =  user.GetMongoClient("mongodb-atlas").GetDatabase("MyGame").GetCollection<UserModel>("Users");
      var userDocument =  await collection.FindOneAsync(new { uid = user.Id});
        Debug.Log("Level" + userDocument.level);
            Debug.Log("Uid" + userDocument.uid);
        Debug.Log("Name" + userDocument.name);
        Debug.Log("ID" + userDocument.Id);
    }
    public async void OnRegister()
    {
        //bat dau dang ki
        SwitchConnectState(ConnectState.IsLoading);
        await app.EmailPasswordAuth.RegisterUserAsync(emailRegister.text, passwordRegister.text);
        SwitchConnectState(ConnectState.IsLogin);
        //dang ki xong
    }
    public enum ConnectState
    {
        IsLoading,
        IsLogin,
        IsRegister,
        IsDialog
    }
    private ConnectState currentState = ConnectState.IsLoading;
    public void SwitchConnectState(ConnectState newState)
    {
        currentState = newState;
        UpdateUI();
    }
    public void GoToRegister()
    {
        SwitchConnectState(ConnectState.IsRegister);
    }
    public void GoToLogin()
    {
        SwitchConnectState(ConnectState.IsLogin);
    }
    private void Awake()
    {
        UpdateUI();
        ConnectToMongo();
    }
    public void ConnectToMongo()
    {
        app = App.Create(appId);
        SwitchConnectState(ConnectState.IsLogin);
    }
    public void UpdateUI()
    {
        switch (currentState)
        {
            case ConnectState.IsLoading: {
                    LoadingUI.SetActive(true);
                    LoginUI.SetActive(false);
                    RegisterUI.SetActive(false);
                    break; }
            case ConnectState.IsLogin: {
                    LoginUI.SetActive(true);
                    LoadingUI.SetActive(false);
                    RegisterUI.SetActive(false);
                    break; }
            case ConnectState.IsRegister: {
                    RegisterUI.SetActive(true);
                    LoginUI.SetActive(false);
                    LoadingUI.SetActive(false);
                    break; }
            case ConnectState.IsDialog: {
                    DialogUI.SetActive(true);
                    break; }
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
}
