using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator playerAnimator;
    private Vector3 direction;
    private Camera Cam;
    public float playerSpeed;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                direction = ray.GetPoint(distance);
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, 0f, direction.z), playerSpeed * Time.deltaTime);
            transform.LookAt(direction);
        }

        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("run", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("run", false);
        }
    }
}

