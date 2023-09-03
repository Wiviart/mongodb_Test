using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectFusion : MonoBehaviour
{
    internal void ConnectToRoom(string roomName, string playerId)
    {
        FusionConnector.Instance.StartGame(roomName, playerId);
    }
}
