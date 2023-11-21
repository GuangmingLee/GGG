using System.Collections;
using System.Collections.Generic;
using SG;
using UnityEngine;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    public void NetWorkStartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartNewGame()
    {
        StartCoroutine(WorldGameSaveManager.instance.LoadNewGame());
    }
}