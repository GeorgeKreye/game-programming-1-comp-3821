using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{

    virtual public void Move(Vector3 moveDir)
    {
        throw new System.NotImplementedException();
    }


    virtual public void RotateCharacter()
    {
        throw new System.NotImplementedException();
    }

    virtual public void Jump()
    {
        throw new System.NotImplementedException();
    }

    virtual public void JumpCanceled()
    {
        throw new System.NotImplementedException();
    }

    #region Unity Functions
    virtual protected void Awake()
    {

    }


    // Start is called before the first frame update
    virtual protected void Start()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {

    }

    virtual protected void FixedUpdate()
    {

    }
    #endregion
}
