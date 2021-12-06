using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParrying : MonoBehaviour
{
    public static PlayerParrying instance;

    public GameObject parryingPoint;
    public float timeBetweenParrying;
    private float parryingCounter;

    public bool isParrying; //�и� �� �и��� �ߵ��Ǵ� ���� �����ϱ� ���� �÷���, �÷��̾��ｺ��Ʈ�ѷ����� �и����� üũ�ϱ� ���� ����

    // �и� �� ī���� ������ ���������� �÷���. PlayerParryingPoint���� ������. PlayerParryingPoint�� active�� false�� ���� �ֱ� ������ ���⿡�� ���� ����.
    public bool counterAtkPossible; 

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
    // ANIMATION EVENT ���� ȣ��
    void ParryingBoxOn()
    {
        parryingPoint.gameObject.SetActive(true);
    }

    void ParryingBoxOff()
    {
        parryingPoint.gameObject.SetActive(false);
    }

    void ResetCounterAtkPossible()
    {
        PlayerParrying.instance.counterAtkPossible = false;
    }    
}
