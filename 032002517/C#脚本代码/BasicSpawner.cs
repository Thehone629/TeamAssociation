using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicSpawner : MonoBehaviour,INetworkRunnerCallbacks
{
   /* public Text scoreText;
    private int sum = 0;*/


    [SerializeField]
    private NetworkRunner networkRunner = null;


    [SerializeField]
    private NetworkPrefabRef playerPrefab;

   

    


    //这个Dictionary使用PlayerRef作为key来存放刚刚生成的可以操控的角色，这么做事为了记录所有玩家的名单
    private Dictionary<PlayerRef,NetworkObject> playList=new Dictionary<PlayerRef,NetworkObject>();

   

    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);//第一个进入房间的就是host，后面进来的就是client
    }
    async void StartGame(GameMode mode)
    {
        networkRunner.ProvideInput = true;
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Fusion Room",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    //玩家加入时会执行这个函数
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)//
    { 
        Vector3 spawPosition = Vector3.up;//给一个玩家出生位置

        NetworkObject networkPlayerObject =runner.Spawn(playerPrefab, spawPosition, Quaternion.identity, player);//利用预制体生成网络对象

      
        playList.Add(player, networkPlayerObject);

     
        
        
        
     
    }

    //玩家离开时就会执行这个函数
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if(playList.TryGetValue(player, out NetworkObject networkObject))
        {
           
            //如果这个容器中还有这个玩家就把这个玩家从中剔除
            runner.Despawn(networkObject);//删除网络对象
            playList.Remove(player);

        }


    }
    //有输入时就会执行这个函数
    public void OnInput(NetworkRunner runner, NetworkInput input) 
    {
        var data = new NetworkInputData();
        if (Input.GetKey(KeyCode.W))
        {
            data.movementInput += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.movementInput += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            data.movementInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.movementInput += Vector3.right;
        }
        data.buttons.Set(InputButtons.JUM, Input.GetKey(KeyCode.Space));//空格键按下时InputButtons.JUM就为true,相反为false，
        data.buttons.Set(InputButtons.FIRE, Input.GetKey(KeyCode.Mouse0));//把鼠标左键定义为FIRE

        input.Set(data);//把键盘输入的数据传送过去
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
}
