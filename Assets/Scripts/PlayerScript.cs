﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public float jumpHeight = 2.0f;

    private bool isGrounded = false;
    private Rigidbody rigid;

    void Move(KeyCode _key)
    {
        Vector3 position = rigid.position;
        Quaternion rotation = rigid.rotation;
        switch (_key)
        {
            case KeyCode.W:
                position += transform.forward * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.S:
                position += -transform.forward * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.A:
                rotation *= Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                break;
            case KeyCode.D:
                rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                break;
            case KeyCode.Space:
                if (isGrounded)
                {
                    rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                    isGrounded = false;
                }
                break;
        }
        rigid.MovePosition(position);
        rigid.MoveRotation(rotation);
    }

    void HandleInput()
    {
        KeyCode[] keys =
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space
        };

        foreach(var key in keys)
        {
            if (Input.GetKey(key))
            {
                Move(key);
            }
        }
    }

    void OnCollisionEnter(Collision _col)
    {
        isGrounded = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // Get Audio Listener from camera
        AudioListener audioListener = GetComponentInChildren<AudioListener>();
        // Get Camera
        Camera cam = GetComponentInChildren<Camera>();

        // If the current instance is local player
        if (isLocalPlayer)
        {
            // Enable everything
            cam.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            // Disable everything
            cam.enabled = false;
            audioListener.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }
}
