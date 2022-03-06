using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;

public class TwitchClient : MonoBehaviour
{
    public Client client;
    private string _channelName = "dactarium";

    private TwitchCommands _twitchCommands;

    void Awake(){
        _twitchCommands = GetComponent<TwitchCommands>();
        _channelName = PlayerPrefs.GetString("TwitchName");
    }
    void Start(){
        Application.runInBackground = true;

        ConnectionCredentials credentials = new ConnectionCredentials("dactariumbot", Secrets.bot_access_token);
        client = new Client();
        client.Initialize(credentials, _channelName);

        client.OnMessageReceived += Client_OnMessageReceived;

        client.Connect();
    }

    private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e){
        _twitchCommands.reciveCommand(e.ChatMessage.DisplayName, e.ChatMessage.Message);
    }   
}
