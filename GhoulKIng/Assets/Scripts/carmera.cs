using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carmera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    float xrotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * sensHori *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensVert * Time.deltaTime;

        //invert look control
        if (invertY)
        {
            xrotation += mouseY;
        }
        else
        {
            xrotation -= mouseY;
        }

        //clamp angle of camera rotation
        xrotation = Mathf.Clamp(xrotation, lockVertMin, lockVertMax);

        //rotate the camera on x axis
        transform.localRotation = Quaternion.Euler(xrotation, 0, 0);

        //rotate the transform
        transform.parent.Rotate(Vector3.up * mouseX);



    }
}
