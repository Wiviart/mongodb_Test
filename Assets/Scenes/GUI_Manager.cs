using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] GameObject loadingUI;

    [Header("Login UI")]
    [SerializeField] GameObject loginUI;
    [SerializeField] TMP_InputField login_EmailInput;
    [SerializeField] TMP_InputField login_passwordInput;
    [SerializeField] Button goToRegisterButton;
    [SerializeField] Button loginButton;

    [Header("Register UI")]
    [SerializeField] GameObject registerUI;
    [SerializeField] TMP_InputField register_EmailInput;
    [SerializeField] TMP_InputField register_PasswordInput;
    [SerializeField] Button goToLoginButton;
    [SerializeField] Button registerButton;

    [Header("Dialog UI")]
    [SerializeField] GameObject dialogUI;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Button closeDialogButton;

    void Awake()
    {
        goToRegisterButton.onClick.AddListener(GoToRegisterButton);
        goToLoginButton.onClick.AddListener(GoToLoginButton);
        registerButton.onClick.AddListener(RegisterButton);
        loginButton.onClick.AddListener(LoginButton);
        closeDialogButton.onClick.AddListener(CloseDialogButton);
    }

    void Start()
    {
        ConnectManager.Instance.OnConnectStateChanged_Login += GoToLoginButton;
        ConnectManager.Instance.OnConnectStateChanged_Register += GoToRegisterButton;
        ConnectManager.Instance.OnConnectStateChanged_Dialog += Dialog;
        ConnectManager.Instance.OnConnectStateChanged_Loading += Loading;

        GoToLoginButton();
    }

    void GoToRegisterButton()
    {
        registerUI.SetActive(true);

        loadingUI.SetActive(false);
        loginUI.SetActive(false);
    }
    void GoToLoginButton()
    {
        loginUI.SetActive(true);

        loadingUI.SetActive(false);
        registerUI.SetActive(false);
        dialogUI.SetActive(false);
    }

    void Loading()
    {
        loadingUI.SetActive(true);

        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }

    void Dialog(string message)
    {
        dialogUI.SetActive(true);
        dialogText.text = message;
    }

    void RegisterButton()
    {
        string email = register_EmailInput.text;
        string password = register_PasswordInput.text;

        ConnectManager.Instance.CheckRegister(email, password);
    }

    void LoginButton()
    {
        string email = login_EmailInput.text;
        string password = login_passwordInput.text;

        ConnectManager.Instance.Login(email, password);
    }

    private void CloseDialogButton()
    {
        dialogUI.SetActive(false);
    }
}
