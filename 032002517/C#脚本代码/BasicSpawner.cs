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

   

    


    //���Dictionaryʹ��PlayerRef��Ϊkey����Ÿո����ɵĿ��ԲٿصĽ�ɫ����ô����Ϊ�˼�¼������ҵ�����
    private Dictionary<PlayerRef,NetworkObject> playList=new Dictionary<PlayerRef,NetworkObject>();

   

    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);//��һ�����뷿��ľ���host����������ľ���client
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
    //��Ҽ���ʱ��ִ���������
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)//
    { 
        Vector3 spawPosition = Vector3.up;//��һ����ҳ���λ��

        NetworkObject networkPlayerObject =runner.Spawn(playerPrefab, spawPosition, Quaternion.identity, player);//����Ԥ���������������

      
        playList.Add(player, networkPlayerObject);

     
        
        
        
     
    }

    //����뿪ʱ�ͻ�ִ���������
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    {
        if(playList.TryGetValue(player, out NetworkObject networkObject))
        {
           
            //�����������л��������ҾͰ������Ҵ����޳�
            runner.Despawn(networkObject);//ɾ���������
            playList.Remove(player);

        }


    }
    //������ʱ�ͻ�ִ���������
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
        data.buttons.Set(InputButtons.JUM, Input.GetKey(KeyCode.Space));//�ո������ʱInputButtons.JUM��Ϊtrue,�෴Ϊfalse��
        data.buttons.Set(InputButtons.FIRE, Input.GetKey(KeyCode.Mouse0));//������������ΪFIRE

        input.Set(data);//�Ѽ�����������ݴ��͹�ȥ
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
