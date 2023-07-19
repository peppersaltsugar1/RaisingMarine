using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour, IPointerClickHandler
{
    private Camera camera;
    private PlayerControl player;

    private void Awake()
    {
        camera = Camera.main;
        player = FindObjectOfType<PlayerControl>();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        camera.transform.position = new Vector3(player.transform.position.x, camera.transform.position.y, player.transform.position.z+10f);
    }
}
