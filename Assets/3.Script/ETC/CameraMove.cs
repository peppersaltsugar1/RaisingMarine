using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    public void CameraUp()
    {
        StartCoroutine(CameraMove_co(Vector3.up));
    }

    public void Exit()
    {
        StopCoroutine("CameraMove_co");
    }

    private IEnumerator CameraMove_co(Vector3 dir)
    {
        while (true)
        {
            Vector3 newPosition = camera.transform.position;
            newPosition.z -= 1;
            camera.transform.position = newPosition;
            yield return null;
        }
    }
}
