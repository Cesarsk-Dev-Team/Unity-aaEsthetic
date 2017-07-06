using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public static float speed;

    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime)); //we have to make sure that it's not framerate dependent
    }
}
