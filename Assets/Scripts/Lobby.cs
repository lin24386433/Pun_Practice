using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Lin
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        // �C���������s�X, �i�� Photon Server ���P�ڹC�����P�������Ϲj.
        string gameVersion = "1";

        [Tooltip("�C���Ǫ��a�H�ƤW��. ��C���Ǫ��a�H�Ƥw���B, �s���a�u��s�}�C���ǨӶi��C��.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("���/���� �C�����a�W�ٻP Play ���s")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("���/���� �s�u�� �r��")]
        [SerializeField]
        private GameObject progressLabel;

        void Awake()
        {
            // �T�O�Ҧ��s�u�����a�����J�ۦP���C������
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        // �P Photon Cloud �s�u
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // �ˬd�O�_�P Photon Cloud �s�u
            if (PhotonNetwork.IsConnected)
            {
                // �w�s�u, �|���H���[�J�@�ӹC����
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // ���s�u, �|�ջP Photon Cloud �s�u
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN �I�s OnConnectedToMaster(), �w�s�W Photon Cloud.");

            // �T�{�w�s�W Photon Cloud
            // �H���[�J�@�ӹC����
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN �I�s OnDisconnected() {0}.", cause);

            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN �I�s OnJoinRandomFailed(), �H���[�J�C���ǥ���.");

            // �H���[�J�C���ǥ���. �i���]�O 1. �S���C���� �� 2. ���������F.    
            // �n�a, �ڭ̦ۤv�}�@�ӹC����.
            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = maxPlayersPerRoom
            });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN �I�s OnJoinedRoom(), �w���\�i�J�C���Ǥ�.");
        }
    }
}

