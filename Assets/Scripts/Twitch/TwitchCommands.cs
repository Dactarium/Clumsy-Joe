using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchCommands : MonoBehaviour
{
    private string _prefix = "!";
    
    public void reciveCommand(string sender, string message){
        if(!message.Substring(0, _prefix.Length).Equals(_prefix)) return;
        
        int commandEnd = message.IndexOf(" ");
        commandEnd = commandEnd < 0 ? message.Length: commandEnd;

        string command = message.Substring(_prefix.Length, commandEnd-_prefix.Length).ToLower();

        switch(command){
            case "join":
                onJoin(sender);
                break;
            default:
                print("Unknown command: " + command);
                break;
        }
    }

    void onJoin(string viewer){
        GameManager.Instance.MapGenerator.fillRoom(viewer);
    }
}
