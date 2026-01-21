using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 2개의 실린더로부터 액체를 받아 혼합하고,
// 특정 조건 충족 시 폭발, 가스 등을 실행하는 클래스입니다.

public class Beaker : MonoBehaviour
{
    public enum BeakerType
    {
        None,
        FluorescentBeaker, // 형광색 비커 (가스 발생)
        PurpleBeaker, // 보라색 비커
        OrangeBeaker // 주황색 비커 (폭발 발생)
    }

    public BeakerType type;

    private Transform liquid; // 액체 양을 증가할 자식 오브젝트 Transform
    private float maxVolume = 0.9f; // 비커가 가득 찼다고 판단하는 최대 높이
    private float fillSpeed; // 액체가 채워지는 속도 
    private const float half = 2f; // 실린더 대비 액체 상승 비율 계수

    [SerializeField] private ParticleSystem explosionParticle; // 폭발 파티클
    [SerializeField] private ParticleSystem gasParticle; // 가스 파티클

    // Boil 컴포넌트는 팀원이 담당한 스크립트 (협업)
    [SerializeField] private Boil boil; // 특정 실린더의 끓임 상태 확인 
    private bool isFull; // 비커가 가득 찼는지 여부

    // 중복 방지 HashSet을 사용해 같은 타입 실린더를 추가해도 카운터 누적 방지
    private HashSet<CylinderType> cylinderTypes = new HashSet<CylinderType>(); 

    private void Start()
    {
        liquid = transform.GetChild(0); // 자식 오브젝트 캐싱 (비커 액체)
    }

    float NextFill(float cyDecreaseSpeed)
    { 
        fillSpeed = cyDecreaseSpeed / half;
        return Mathf.Clamp01(liquid.localScale.y + Time.deltaTime * fillSpeed);
    }

    // 외부(BeakerDetector)에서 호출하여 실린더와 비커의 상호작용을 처리합니다.
    public void HandleCylinder(Cylinder cylinder)
    {
        // 비커 타입별로 실린더 조건을 통과 후 상호작용
        switch(this.type)
        {
            case BeakerType.FluorescentBeaker :
                if(CanUseFluorescent(cylinder))
                   ProcessFill(() => GasleakBeaker(), cylinder);    
                break;
            case BeakerType.PurpleBeaker :
                if(CanUsePurple(cylinder))
                   ProcessFill(null, cylinder);       
                break;
            case BeakerType.OrangeBeaker :
                if(CanUseOrange(cylinder))
                   ProcessFill(() => ExplosionBeaker(this.gameObject), cylinder);             
                break;
        }
    }

    // 폭발 파티클 발생 후 비커 오브젝트 파괴
    void ExplosionBeaker(GameObject explosionOb) 
    {
        explosionParticle.Play();
        Destroy(explosionOb, 1.0f);
    }

    // 가스 파티클 발생
    void GasleakBeaker()
    {
        gasParticle.Play();
    }

    // 실린더 액체를 비커에 옮겨 담아 혼합하는 과정
    void ProcessFill(Action onBeakerFilled, Cylinder cylinder) // (onBeakerFilled) 비커가 가득 찼을 때 실행할 콜백 함수
    {
        if(isFull && cylinder.IsEmpty()) return; // 이미 가득 찼거나 실린더가 비어있으면 중단

        cylinderTypes.Add(cylinder.type); // 실린더 액체 타입 기록

        // 비커가 최대 용량에 도달하고 2종류 액체가 섞였을 때 이벤트 발생
        if(liquid.localScale.y >= maxVolume && cylinderTypes.Count == 2) 
        {
            onBeakerFilled?.Invoke();
            cylinderTypes.Clear();
            isFull = true;
            return;
        }
  
        // 실린더의 양을 줄이고 비커의 양을 늘림
        cylinder.Decrease();
        if(cylinder.IsEmpty()) // 비커의 양이 다 차도 실린더를 계속 감소시켜 소모될 때 까지를 위한 시각적 연출
            return;
        liquid.localScale = new Vector3(1, NextFill(cylinder.DecreaseSpeed), 1);
    }
    
    // 형광색 비커 레시피 : 노란색 또는 초록색 실린더 허용
    bool CanUseFluorescent(Cylinder cylinder)
    {
        return cylinder.IsType(CylinderType.Yellow) || 
            cylinder.IsType(CylinderType.Green);
    }

    // 보라색 비커 레시피 : 끓인 빨간색 또는 파란색 실린더 허용
    bool CanUsePurple(Cylinder cylinder)
    {
        // Boil 컴포넌트는 팀원이 담당한 스크립트 (협업)
        return cylinder.IsType(CylinderType.BoilRed) && boil.Boiled ||
            cylinder.IsType(CylinderType.Blue);
    }
    
    // 주황색 비커 레시피 : 빨간색 또는 노란색 실린더 허용
    bool CanUseOrange(Cylinder cylinder)
    {
        return cylinder.IsType(CylinderType.Red) 
            || cylinder.IsType(CylinderType.Yellow);
    }
}
