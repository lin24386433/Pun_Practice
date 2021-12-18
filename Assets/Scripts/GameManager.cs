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
        [Tooltip("Prefab- ���a������")]
        public GameObject playerPrefab;

        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("playerPrefab ��, �Цb Game Manager ���s�]�w",
                    this);
            }
            else
            {
                Debug.LogFormat("�ʺA�ͦ����a���� {0}",
                    Application.loadedLevelName);
                PhotonNetwork.Instantiate(this.playerPrefab.name,
                    new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        // ���a���}�C���Ǯ�, ��L�a�^��C�����J�f
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("{0} �i�J�C����", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("�ڬO Master Client ��? {0}",
                    PhotonNetwork.IsMasterClient);
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("{0} ���}�C����", other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("�ڬO Master Client ��? {0}",
                    PhotonNetwork.IsMasterClient);
                //LoadArena();
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("�ڤ��O Master Client, �������J�������ʧ@");
            }
            Debug.LogFormat("���J{0}�H������",
                PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " +
                PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }
}

