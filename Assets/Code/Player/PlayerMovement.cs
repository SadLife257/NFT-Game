using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    private Vector2 axisMovement;

    void Start()
    {
        
    }

    void Update()
    {
        axisMovement.x = Input.GetAxisRaw("Horizontal");
        axisMovement.y = Input.GetAxisRaw("Vertical");

        PositionChange();
    }

    private void PositionChange()
    {
        transform.position += (Vector3)axisMovement * Time.deltaTime * speed;
    }
}
