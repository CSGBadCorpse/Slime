using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAction : MonoBehaviour
{
    public bool startJump;

    private void Start()
    {
        startJump = false;
    }

    public void Jump()
    {
        startJump = true;
    }

    public void Land()
    {
        startJump = false;
    }
}
