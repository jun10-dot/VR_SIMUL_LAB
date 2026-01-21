using UnityEngine;

/// <summary>
/// 실린더 내의 액체 양을 감소시키는 클래스입니다.
/// 비커와 상호작용 시 시각적으로 액체가 줄어드는 연출을 관리합니다.
/// </summary>

public enum CylinderType
{
    None,
    Red,
    Yellow,
    Green,
    Blue,
    BoilRed
}

public class Cylinder : MonoBehaviour
{
    public CylinderType type;
    private Transform liquid; // 액체 양을 감소할 자식 오브젝트의 Transform
    [SerializeField] private float decreaseSpeed = 0.2f; // 액체의 높이가 줄어드는 속도
    private const float minVolume = 0.05f; // 액체가 완전히 소모되었다고 판단할 최소 값

#region Property
    public float DecreaseSpeed 
    {
        get{return decreaseSpeed ;}
    }
#endregion

    private void Start()
    {
        liquid = transform.GetChild(0); // 자식 오브젝트(액체) 캐싱
    }
    // 실린더 액체의 Y축 스케일을 서서히 감소
    // 임계값 이하로 떨어지면 액체 오브젝트 비활성화
    public void Decrease()
    {
        liquid.localScale -= Vector3.up * Time.deltaTime * decreaseSpeed; // Y축 스케일을 서서히 감소

        if(liquid.localScale.y <= minVolume)  
            liquid.gameObject.SetActive(false);  // 비활성화 (소모되었음을 표현)
    }
    
    // 현재 실린더가 비어있는 상태인지 확인
    // 비활성화 상태면 true 반환
    public bool IsEmpty()
    {
        if(liquid == null) return false;
        return !liquid.gameObject.activeSelf;
    }

    // 실린더의 타입이 인자로 받은 타입과 일치하는지 확인
    public bool IsType(CylinderType type) => this.type == type;
    
}
