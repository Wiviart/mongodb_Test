using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] TextMeshProUGUI userId;
    [SerializeField] Button findMatchButton;
    [SerializeField] Button renameButton;

    [Header("Rename Panel")]
    [SerializeField] GameObject renamePanel;
    [SerializeField] TMP_InputField renameInputField;
    [SerializeField] Button renameConfirmButton;
    [SerializeField] Button renameCancelButton;

    [Header("Find Match Panel")]
    [SerializeField] GameObject findMatchPanel;
    [SerializeField] TMP_InputField findMatchInputField;
    [SerializeField] Button joinConfirmButton;
    [SerializeField] Button joinCancelButton;

    [Header("Fusion")]
    [SerializeField] ConnectFusion connectFusion;

    void Awake()
    {
        findMatchButton.onClick.AddListener(FindMatchButton);
        renameButton.onClick.AddListener(RenameButton);

        renameConfirmButton.onClick.AddListener(RenameConfirmButton);
        renameCancelButton.onClick.AddListener(RenameCancelButton);

        joinConfirmButton.onClick.AddListener(JoinConfirmButton);
        joinCancelButton.onClick.AddListener(JoinCancelButton);
    }

    void Start()
    {
        ShowUserName();

        RenameCancelButton();
        JoinCancelButton();
    }

    void Update()
    {

    }

    void ShowUserName()
    {
        PlayerData playerData = ConnectDB.Instance.GetUserData();

        userId.text = playerData.Name;
    }

    private void FindMatchButton()
    {
        findMatchPanel.SetActive(true);
    }

    public void OnRoomNameChanged()
    {
        joinConfirmButton.interactable = findMatchInputField.text.Length > 0;
    }

    void RenameButton()
    {
        renamePanel.SetActive(true);
        renameInputField.text = userId.text;
    }

    void RenameConfirmButton()
    {
        ConnectDB.Instance.UpdateUserName(renameInputField.text);

        RenameCancelButton();
        ShowUserName();
    }

    void RenameCancelButton()
    {
        renamePanel.SetActive(false);
    }

    private void JoinConfirmButton()
    {
        string roomName = findMatchInputField.text;
        string playerId = userId.text;
        connectFusion.ConnectToRoom(roomName, playerId);
    }

    private void JoinCancelButton()
    {
        findMatchPanel.SetActive(false);
    }
}
