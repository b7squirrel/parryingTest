using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParrying : MonoBehaviour
{
    public static PlayerParrying instance;

    public GameObject parryingPoint;
    public float timeBetweenParrying;
    private float parryingCounter;

    public bool isParrying; //패링 중 패링이 발동되는 것을 방지하기 위한 플래그, 플레이어헬스컨트롤러에서 패링여부 체크하기 위해 접근

    public Animator anim;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();

        parryingCounter = timeBetweenParrying;
    }
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Parry"))
        {
            isParrying = false;
            ParryingBoxOff();
        }

        if (parryingCounter > 0f)
        {
            parryingCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && PlayerController.instance.dashRollCounter <= 0f && PlayerController.instance.pushedBackCounter <= 0f)
            {
                if (!isParrying)
                {
                    isParrying = true;
                    anim.SetTrigger("Parrying");

                    ParryingBoxOn();
                    parryingCounter = timeBetweenParrying;
                }
            }
        }
    }
    // ANIMATION EVENT 에서 호출
    void ParryingBoxOn()
    {
        parryingPoint.gameObject.SetActive(true);
    }

    void ParryingBoxOff()
    {
        parryingPoint.gameObject.SetActive(false);
    }
}
