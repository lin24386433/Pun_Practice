using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lin;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace Lin
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Prefab- 玩家的角色")]
        public GameObject playerPrefab;

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("playerPrefab 遺失, 請在 Game Manager 重新設定",
                    this);
            }
            else
            {
                Debug.LogFormat("動態生成玩家角色 {0}",
                    Application.loadedLevelName);
                PhotonNetwork.Instantiate(this.playerPrefab.name,
                    new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        // 玩家離開遊戲室時, 把他帶回到遊戲場入口
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("{0} 進入遊戲室", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("我是 Master Client 嗎? {0}",
                    PhotonNetwork.IsMasterClient);
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("{0} 離開遊戲室", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("我是 Master Client 嗎? {0}",
                    PhotonNetwork.IsMasterClient);
                //LoadArena();
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("我不是 Master Client, 不做載入場景的動作");
            }
            Debug.LogFormat("載入{0}人的場景",
                PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " +
                PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}

