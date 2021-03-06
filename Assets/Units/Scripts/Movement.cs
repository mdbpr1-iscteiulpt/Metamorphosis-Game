﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public float MoveSpeed = 4;

    private bool selected;

    public void selectUnit(bool state)
    {
        selected = state;
    }
    
    public void movementDestination(Vector3 point)
    {
        agent.SetDestination(point);
    }

    // Update is called once per frame
    void Update()
    {
        float velocity = agent.velocity.magnitude;
        if (velocity > 0)
        {
            GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walking", false);
        }
        agent.speed = MoveSpeed;
        if (Input.GetMouseButtonDown(1)&&selected)
        {

        }
    }
}
