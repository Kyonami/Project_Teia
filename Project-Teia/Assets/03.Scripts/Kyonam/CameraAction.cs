using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct CameraStatusInfo
{
    public Vector3 cameraPosition;
    public float size;
}

public enum CameraState
{
    ZOOMOUT,
    ZOOMINTOPLAYER,
    ZOOMINTOENEMY
}

// Default Position 0 0 0,      Size 6
// To Player Position -2 -1 0   Size 4

public class CameraAction : MonoBehaviour {
    
    [SerializeField]
    private Camera myCamera;
    private CameraStatusInfo defaultPosition, normalPlayerPosition;

    [SerializeField]
    private CameraState cameraState;

    [SerializeField]
    private Animator backGroundAnimator;

    [SerializeField]
    private float zoomingSpeed = 0.06f;

    private Coroutine currentPlayCoroutine;

    private void Start()
    {
        defaultPosition.cameraPosition = new Vector2(0, 0);
        defaultPosition.size = 6f;
        normalPlayerPosition.cameraPosition = new Vector2(-2, -1);
        normalPlayerPosition.size = 4f;

        myCamera.transform.position = defaultPosition.cameraPosition;
    }
    public void PlayZoomToLife(bool isPlayer)
    {
        backGroundAnimator.SetInteger("BackGroundState", 0);

        if (currentPlayCoroutine != null)
            StopCoroutine(currentPlayCoroutine);

        currentPlayCoroutine = StartCoroutine(ZoomToLife(isPlayer));
    }
    public void PlayUnZoom()
    {
        backGroundAnimator.SetInteger("BackGroundState", 0);

        if (currentPlayCoroutine != null)
            StopCoroutine(currentPlayCoroutine);

        currentPlayCoroutine = StartCoroutine(UnzoomToDefaultPosition());
    }
    public void PlayDamagedAction()
    {
        StartCoroutine(DamagedAction());
    }
    public void PlayIdleAction()
    {
        backGroundAnimator.SetInteger("BackGroundState", 1);
    }

    private IEnumerator ZoomToLife(bool isPlayer)
    {
        if (isPlayer)
            normalPlayerPosition.cameraPosition.x = -2;
        else
            normalPlayerPosition.cameraPosition.x = 2;

        if (!(isPlayer && cameraState == CameraState.ZOOMINTOPLAYER) || !(!isPlayer && cameraState == CameraState.ZOOMINTOENEMY))
        {
            if (!isPlayer)
                cameraState = CameraState.ZOOMINTOENEMY;
            else
                cameraState = CameraState.ZOOMINTOPLAYER;

            for (int i = 0; i < 100; i++)
            {
                myCamera.transform.localPosition = Vector2.Lerp(myCamera.transform.localPosition, normalPlayerPosition.cameraPosition, zoomingSpeed);
                myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, normalPlayerPosition.size, zoomingSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            myCamera.transform.localPosition = normalPlayerPosition.cameraPosition;
            myCamera.orthographicSize = normalPlayerPosition.size;

        }
    }
    private IEnumerator UnzoomToDefaultPosition()
    {
        if (cameraState != CameraState.ZOOMOUT)
        {
            cameraState = CameraState.ZOOMOUT;

            for (int i = 0; i < 100; i++)
            {
                myCamera.transform.localPosition = Vector2.Lerp(myCamera.transform.localPosition, defaultPosition.cameraPosition, zoomingSpeed);
                myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, defaultPosition.size, zoomingSpeed);
                yield return new WaitForSeconds(0.01f);
            }

            myCamera.transform.localPosition = defaultPosition.cameraPosition;
            myCamera.orthographicSize = defaultPosition.size;

            PlayIdleAction();
        }
    }
    private IEnumerator DamagedAction()
    {
        Vector2 temp;
        for (int i = 0; i < 10; i++)
        {
            temp = Random.insideUnitCircle * 0.25f;
            //temp.z = 0;
            for (int j = 0; j < 2; j++)
            {
                myCamera.transform.localPosition = Vector2.Lerp(myCamera.transform.localPosition, temp, 0.75f);
                yield return new WaitForSeconds(0.001f);
            }
        }
    }

    public void CancelCameraAnimation()
    {
        if(currentPlayCoroutine != null)    StopCoroutine(currentPlayCoroutine);
        backGroundAnimator.SetInteger("BackGroundState", 0);
    }
    public CameraState GetCameraActionState()
    {
        return cameraState;
    }
}
