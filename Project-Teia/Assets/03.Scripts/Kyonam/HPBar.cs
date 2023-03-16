using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    #region Components
    [SerializeField]
    private Fighter theTarget = null;   // 대상 컴포넌트
    [SerializeField]
    private Image myImage = null;       // 내 HP바 이미지
    [SerializeField]
    private Transform thePivot = null;
    #endregion

    public Fighter TheTarget { get { return theTarget; } }

    //public void MakeConnection(Fighter _target)   // 타겟과 연동
    //{
    //    theTarget = _target;
    //    UpdatePosition();
    //    gameObject.SetActive(true);
    //}

    //public void StopConnection()    // 연동 해제
    //{
    //    theTarget = null;
    //    gameObject.SetActive(false);
    //}
    
    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()   // 타겟을 계속 추적
    {
        transform.localPosition = thePivot.position;
    }

    public void UpdateGauge()
    {
        myImage.fillAmount = theTarget.GetCurrentHPRatio();
    }
}
