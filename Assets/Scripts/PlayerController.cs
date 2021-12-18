using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lin;
using Photon.Pun;

namespace Lin
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Rigidbody playerRigi = null;

        // Cinemachine 跟隨座標
        [SerializeField]
        private Transform cameraPos = null;

        // 腳部座標
        [SerializeField]
        private Transform feet = null;

        [SerializeField]
        private Transform stepRayUpper = null;
        [SerializeField]
        private Transform stepRayLower = null;

        [SerializeField]
        private float stepHeight = 0.3f;
        [SerializeField]
        private float stepSmooth = 2f;

        // 地板Layer
        [SerializeField]
        private LayerMask floorLayer = default;

        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private float jumpForce = 10f;

        [SerializeField]
        private float gravity = 10f;

        [SerializeField]
        private float mouseXSensitivity = 1f;
        [SerializeField]
        private float mouseYSensitivity = 1f;

        private Vector3 playerMovementInput = default;

        private float mouseX = 1f;
        private float mouseY = 1f;

        [Tooltip("玩家的血量")]
        public float health = 1f;

        private void Start()
        {
            stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);

            if (photonView.IsMine)
            {
                GameObject cameraFollower = GameObject.Find("Camera Follower");

                cameraFollower.transform.SetParent(cameraPos);
                cameraFollower.transform.localPosition = Vector3.zero;
            }
        }

        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            Jump();
            GetInput();
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            Rotate();
            Move();
            StepClimb();
        }

        #region - GetInput - 

        void GetInput()
        {
            mouseX += Input.GetAxis("Mouse X") * mouseXSensitivity;
            mouseY += Input.GetAxis("Mouse Y") * mouseYSensitivity * -1;

            mouseY = Mathf.Clamp(mouseY, -89f, 89f);

            playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        }

        #endregion

        #region - Basic Movement -

        void Move()
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

            Vector3 moveVector = transform.TransformDirection(playerMovementInput) * speed;

            playerRigi.velocity = new Vector3(moveVector.x, playerRigi.velocity.y, moveVector.z);

            playerRigi.AddForce(Vector3.up * gravity);
        }

        void Rotate()
        {
            playerRigi.AddTorque(0f, mouseX, 0f);
            mouseX = 0f;

            Quaternion cameraPosQuaternion = Quaternion.Euler(mouseY, 0f, 0f);
            cameraPos.localRotation = cameraPosQuaternion;
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround())
            {
                playerRigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        bool isGround() => Physics.CheckSphere(feet.transform.position, 0.1f, floorLayer);

        #endregion

        #region - StepClimb -

        void StepClimb()
        {
            RaycastHit hitLower;
            if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
                {
                    playerRigi.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }

            RaycastHit hitLower45;
            if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
            {

                RaycastHit hitUpper45;
                if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
                {
                    playerRigi.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }

            RaycastHit hitLowerMinus45;
            if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
            {

                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
                {
                    playerRigi.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }
        }

        #endregion

        #region - OnTrigger -

        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            health -= 0.1f;
        }

        void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            health -= 0.1f * Time.deltaTime;
        }

        #endregion
    }
}

