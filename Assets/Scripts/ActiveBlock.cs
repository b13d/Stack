using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBlock : MonoBehaviour
{
    public static int cubeNumber;



    private void Update()
    {
        //Debug.Log(cubeNumber);
    }

    void FixedUpdate()
    {
            if (cubeNumber == 0)
            {
                transform.Translate(Vector3.forward * -1f * Time.deltaTime, Space.Self);


            }
            else
            {

                transform.Translate(Vector3.right * Time.deltaTime, Space.Self);

            }
    }
}
