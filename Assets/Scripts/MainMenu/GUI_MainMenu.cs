using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userId;
    [SerializeField] Button findMatchButton;
    [SerializeField] Button renameButton;

    [SerializeField] GameObject renamePanel;
    [SerializeField] TMP_InputField renameInputField;
    [SerializeField] Button renameConfirmButton;
    [SerializeField] Button renameCancelButton;

    void Awake()
    {
        findMatchButton.onClick.AddListener(FindMatchButton);
        renameButton.onClick.AddListener(RenameButton);

        renameConfirmButton.onClick.AddListener(RenameConfirmButton);
        renameCancelButton.onClick.AddListener(RenameCancelButton);
    }

    void Start()
    {
        ShowUserName();

        RenameCancelButton();
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
        throw new NotImplementedException();
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
}
