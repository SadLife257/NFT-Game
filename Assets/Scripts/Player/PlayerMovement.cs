using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 20f;

    private Vector2 axisMovement;

    [SerializeField] float maxStamina = 100f;
    [SerializeField] float stamina;
    [SerializeField] float drainValue;
    [SerializeField] float recoverValue;
    [SerializeField] Image StaminaBar;

    private bool isRunning = false;

    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        axisMovement.x = Input.GetAxisRaw("Horizontal");
        axisMovement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        TogglePlayerSpeed();
    }

    private void TogglePlayerSpeed()
    {
        if (isRunning)
        {
            VectorMoveTowards(runSpeed);
            UseStamina();
            if (stamina < 0)
            {
                stamina = 0;
                isRunning = false;
            }
        }
        else
        {
            HealStamina();
            VectorMoveTowards(walkSpeed);
        }
    }

    /*private void RigidbodyAddForce(float speed)
    {
        rigidbody.AddForce(axisMovement * speed, ForceMode2D.Impulse);
    }*/

    private void VectorMoveTowards(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            transform.position + (Vector3)axisMovement, speed * Time.deltaTime);
    }

    /*private void PositionChange(float speed)
    {
        transform.position += (Vector3)axisMovement * Time.deltaTime * speed;
    }*/

    private void UseStamina()
    {
        stamina -= drainValue * Time.deltaTime;
        StaminaBar.fillAmount = stamina / maxStamina;
    }

    private void HealStamina()
    {
        stamina += recoverValue * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        StaminaBar.fillAmount = stamina / maxStamina;
    }
}
