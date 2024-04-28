using System.Text.RegularExpressions;
using Scipts;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private TMP_Text ipAddressText;
    [SerializeField] private string lobbySceneName = "SampleScene";

    public void StartGame()
    {
        var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        // if (utpTransport)
        // {
        //     utpTransport.SetConnectionData(ClearIp(ipAddressText.text), 7777);
        // }

        if (NetworkManager.Singleton.StartHost())
        {
            SceneTransitionHandler.Instance.RegisterCallbacks();
            SceneTransitionHandler.Instance.SwitchScene(lobbySceneName);
        }
        else
        {
            Debug.LogError("Failed to start host'");
        }
    }
    
    public void JoinGame()
    {
        // var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        // if (utpTransport)
        // {
        //     utpTransport.SetConnectionData(ClearIp(ipAddressText.text), 7777);
        // }

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.LogError("Failed to start client'");
        }
    }

    static string ClearIp(string ip)
    {
        return Regex.Replace(ip, "[^A-Za-z0-9.]", "");
    }
}
