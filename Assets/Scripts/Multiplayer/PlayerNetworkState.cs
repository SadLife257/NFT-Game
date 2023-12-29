using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct PlayerNetworkState : INetworkSerializable
{
    private float _posX, _posY;
    private short _rotZ;

    private float staminaPercentage;

    private WeaponConfigSO weapon;

    internal Vector3 Position
    {
        get => new(_posX, _posY, 0);
        set
        {
            _posX = value.x;
            _posY = value.y;
        }
    }

    internal Vector3 Rotation
    {
        get => new(0, 0, _rotZ);
        set => _rotZ = (short)value.z;
    }

    internal float StaminaPercentage
    {
        get => staminaPercentage;
        set => staminaPercentage = value;
    }

    internal WeaponConfigSO Weapon
    {
        get => weapon;
        set => weapon = value;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _posX);
        serializer.SerializeValue(ref _posY);
        serializer.SerializeValue(ref _rotZ);
        serializer.SerializeValue(ref staminaPercentage);
        serializer.SerializeNetworkSerializable(ref weapon);
    }
}
