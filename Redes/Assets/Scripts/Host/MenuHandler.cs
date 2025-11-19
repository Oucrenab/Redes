using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] RunnerHandler _runnerHandler;

    [Header("Panels")]
    [SerializeField] GameObject _mainPanel;
    [SerializeField] GameObject _joinPanel;
    [SerializeField] GameObject _sessionPanel;
    [SerializeField] GameObject _hostPanel;

    [Header("Buttons")]
    [SerializeField] Button _joinButton;
    [SerializeField] Button _goHostButton;
    [SerializeField] Button _hostButton;

    [Header("InputField")]
    [SerializeField] TMP_InputField _sessionNameField;

    private void Awake()
    {
        _joinButton.onClick.AddListener(AskToJoinLobby);

        _goHostButton.onClick.AddListener(() =>
        {
            _sessionPanel.SetActive(false);
            _hostPanel.SetActive(true);
        });

        _hostButton.onClick.AddListener(StartGamesAsHost);

        _runnerHandler.OnJoinLobbySuccesfully += () =>
        {
            _joinPanel.SetActive(false);
            _sessionPanel.SetActive(true);
        };
    }

    void AskToJoinLobby()
    {
        _joinButton.interactable = false;
        _runnerHandler.JoinLobby();

        _mainPanel.SetActive(false);
        _joinPanel.SetActive(true);
    }

    void StartGamesAsHost()
    {
        _hostButton.interactable = false;
        _runnerHandler.HostGame(_sessionNameField.text, "HostScene");
    }
}
