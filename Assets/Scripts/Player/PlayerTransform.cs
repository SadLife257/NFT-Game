using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTransform : NetworkBehaviour
{
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;

    private Rigidbody2D rigidBody;
    private Transform weaponContainer;
    private Player player;
    private Image StaminaBar;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = transform.GetComponent<Player>();
        weaponContainer = transform.GetChild(1).GetComponent<Transform>();
        StaminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();

        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    private void Update()
    {
        if (IsOwner) TransmitState();
        else ConsumeState();
    }

    #region Transmit State

    private void TransmitState()
    {
        var state = new PlayerNetworkState
        {
            Position = rigidBody.position,
            Rotation = weaponContainer.rotation.eulerAngles,
            StaminaPercentage = player.staminaPercentage
        };

        if (IsServer || !_serverAuth)
            _playerState.Value = state;
        else
            TransmitStateServerRpc(state);
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        _playerState.Value = state;
    }

    #endregion

    #region Interpolate State

    private Vector3 _posVel;
    private float _rotVelZ;

    private void ConsumeState()
    {
        rigidBody.MovePosition(Vector3.SmoothDamp(rigidBody.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime));

        weaponContainer.rotation = Quaternion.Euler(
            0,
            0,
            Mathf.SmoothDampAngle(weaponContainer.rotation.eulerAngles.z, _playerState.Value.Rotation.z, ref _rotVelZ, _cheapInterpolationTime)
        );

        Vector2 scale = weaponContainer.localScale;
        if (weaponContainer.eulerAngles.z > 90f && weaponContainer.eulerAngles.z < 270f)
            scale.y = -1;
        else
            scale.y = 1;
        weaponContainer.localScale = scale;

        //player.stamina = _playerState.Value.Stamina;
        StaminaBar.fillAmount = _playerState.Value.StaminaPercentage;
    }

    #endregion
}
