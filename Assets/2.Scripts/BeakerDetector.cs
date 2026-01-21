using UnityEngine;

/// <summary>
/// 비커의 트리거 영역 내 실린더를 감지하는 클래스입니다.
/// 감지된 실린더 정보를 Beaker 클래스로 전달하여 액체 혼합 로직을 실행합니다.
/// </summary>

public class BeakerDetector : MonoBehaviour
{
    private Cylinder cylinder; // 현재 트리거 영역 내에 있는 실린더 컴포넌트
    private Beaker beaker; // 로직을 처리할 부모 비커 컴포넌트

    void Start()
    {
        // 부모 오브젝트의 Beaker 컴포넌트를 찾아 캐싱
        beaker = transform.parent.GetComponent<Beaker>();
        if(beaker == null) return;
    }
    
    // 실린더가 비커의 트리거 영역에 머물러 있는 동안 매 프레임 호출
    public void OnTriggerStay(Collider col)
    {
        if(cylinder == null) // 매 프레임 GetComponent 호출을 방지하기 위한 조건부 캐싱 
           cylinder = col.gameObject.GetComponent<Cylinder>();
        if(cylinder == null) return;
        beaker.HandleCylinder(cylinder); // Beaker클래스에 실린더 정보를 전달해 로직 실행
    }
    
    // 실린더가 트리거 영역을 벗어나면 참조를 해제해 또 다른 실린더를 감지할 수 있도록 준비
    public void OnTriggerExit(Collider col)
    {
        if(cylinder != null)
            cylinder = null;
    }
}
