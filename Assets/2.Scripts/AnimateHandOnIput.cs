using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimateHandOnIput : MonoBehaviour
{
    public InputActionProperty punchAnimation;
    public InputActionProperty gripAnimation;
    public Animator handAnimator;

    // Update is called once per frame
    void Update()
    {
        float triggerValue = punchAnimation.action.ReadValue<float>(); //트리거 버튼 감지 오른쪽 컨트롤러 트리거 버튼.
        handAnimator.SetFloat("Trigger",triggerValue); //애니메이터에 트리거 값 전달.

        float gripValue =gripAnimation.action.ReadValue<float>(); //그립 버튼 감지 오른쪽 컨트롤러 그립 버튼.
        handAnimator.SetFloat("Grip",gripValue); //애니메이터에 그립 값 전달.
    }
}
