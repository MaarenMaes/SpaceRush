using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenOutOfScreen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        //Debug.Log("Invisible");
        Destroy(this.gameObject);
    }
}
