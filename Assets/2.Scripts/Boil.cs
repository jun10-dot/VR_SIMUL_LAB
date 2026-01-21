using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boil : MonoBehaviour
{
    [SerializeField]
    private Material mat;   // 용액 메테리얼

[SerializeField]
    private float boilPercent;  // 용액 가열 정도
    private bool boiled;    // 가열된 상태인지 나타내는 변수
    [SerializeField]
    private bool isExplosive;   // 가열하면 폭발하는 용액인지 나타내는 변수

    private ParticleSystem boilParticle;    // 가열 파티클
    private ParticleSystem explosionParticle;   // 폭발 파티클

#region property

    public float BoilPercent
    {
        get
        {
            return boilPercent;
        }

        set
        {
            boilPercent = value;
        }
    }

    public bool Boiled
    {
        get
        {
            return boiled;
        }

        set
        {
            boiled = value;
        }
    }
    #endregion

    void Awake()
    {
        boilParticle = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        explosionParticle = transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
    }

    void Start()
    {
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;

        BoilPercent = 0.0f;
        Boiled = false;
    }

    void Update()
    {
        if (Boiled && isExplosive)  // 가열된 상태이고 폭발성인 경우 폭발처리
        {
            explosionParticle.Play();
            isExplosive = false;
            Destroy(this.gameObject, 1.0f);
            return;
        }
        boilParticle.Stop();    // 가열하고 있지 않은경우 파티클 Stop

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fire") // 불 오브젝트에 닿을때마다 가열수치 상승
        {
            boilParticle.Play();
            BoilPercent += 0.1f;
        }

        if (BoilPercent >= 100.0f)  // 가열수치가 100인경우 가열상태로 전환 및 수치 예외처리
        {
            Boiled = true;
            BoilPercent = 20.0f;
            return;
        }
    }
}
