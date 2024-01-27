using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraneControl : MonoBehaviour
{
    private CraneGameInput gameInput;
    private BoxBounder boxBounder;

    [SerializeField]
    private float horizontalMoveSpeed = 5.0f;

    [SerializeField]
    private GameObject box;

    [SerializeField]
    private GameObject arm;

    [SerializeField]
    private GameObject claw;

    private void Awake()
    {
        boxBounder = box.GetComponent<BoxBounder>();

        gameInput = new CraneGameInput();
        gameInput.CraneControls.Enable();
    }

    private void Update()
    {
        float horizontalMove = gameInput.CraneControls.Movement.ReadValue<float>();
        Debug.Log(horizontalMove);
        float delta = horizontalMove * horizontalMoveSpeed * Time.deltaTime;

        if ((delta > 0 && boxBounder.Status == BoxBounder.BlockedStatus.Right)
            || (delta < 0 && boxBounder.Status == BoxBounder.BlockedStatus.Left))
        {
            return;
        }
        else 
        {
            box.transform.position = new Vector3(box.transform.position.x + delta, box.transform.position.y);
        }
    }
}
