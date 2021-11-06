using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectile;
    public Transform shootingPoint;

    private float shootingCounter;
    public float timeBetweenShooting;

    private float relativePosToPlayer;

    private Animator anim;

    public float detectRadius;
    private bool isDetectingPlayer;
    public LayerMask detectWhat;

    //Antic Energy
    public GameObject anticEnergy, anticEnergyCircle;
    public Transform anticEnergyPoint;

    //������ ����
    public GameObject energyIn;
    public GameObject energyBall;

    //emotion mark
    private bool exclamationCount;
    public GameObject exclamationMark;
    public Transform emotionPoint;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //�÷��̾ ������ �÷��̾ �����ߴ� ���� ����
        if(RespawnManager.instance.isPlayerDead)
        {
            isDetectingPlayer = false;
            exclamationCount = false;
        }

        //�÷��̾� ����
        isDetectingPlayer = Physics2D.OverlapCircle(transform.position, detectRadius, detectWhat);

        //�÷��̾ �����ϸ� ����ǥ�� ����
        if (isDetectingPlayer && !exclamationCount)
        {
            ExclamtionMark();
            exclamationCount = true;
        }

        //�÷��̾ ���� �� �ְ� ���� ����
        relativePosToPlayer = PlayerController.instance.transform.position.x - transform.position.x;

        // Direction
        if (relativePosToPlayer < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (relativePosToPlayer > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        // Shooting Frequency
        if (shootingCounter < .1f && exclamationCount)
        {
            AttackAnimation();
            AnticEnergy();
            shootingCounter = timeBetweenShooting;
        }
        else
        {
            shootingCounter -= Time.deltaTime;
        }
    }
    void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }

    void ExclamtionMark()
    {
        GameObject clone = Instantiate(exclamationMark, emotionPoint.position, emotionPoint.rotation);
        clone.transform.parent = transform;
    }

    public void Shoot()
    {
        Instantiate(projectile, shootingPoint.position, shootingPoint.rotation);
    }

    void AnticEnergy()
    {
        energyBall = Instantiate(anticEnergyCircle, anticEnergyPoint.position, anticEnergyPoint.rotation);
        energyBall.transform.parent = transform;

        energyIn = Instantiate(anticEnergy, anticEnergyPoint.position, anticEnergyPoint.rotation);
        energyIn.transform.parent = transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
