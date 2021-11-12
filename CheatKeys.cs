using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatKeys : MonoBehaviour
{
    [Header("Red Warlock")]
    private bool WarlockIsLeft;
    public GameObject redWarlock;
    public Transform redWarlockPointLeft;
    public Transform redWarlockPointRight;

    [Header("Goul Fighter")]
    public GameObject goulFighter;
    public Transform goulFighterPointLeft;
    public Transform goulFighterPointRight;
    private bool isLeft;

    [Header("Player")]
    private bool isInvincible;
    public Material matWhite;
    private Material matDefault;
    
    private void Start()
    {
        matDefault = PlayerController.instance.theSR.material;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isLeft = !isLeft;
            if(isLeft)
            {
                Instantiate(goulFighter, goulFighterPointLeft.position, Quaternion.identity);
            }
            else
            {
                Instantiate(goulFighter, goulFighterPointRight.position, Quaternion.identity);
            }
            
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            WarlockIsLeft = !WarlockIsLeft;
            if(WarlockIsLeft)
            {
                Instantiate(redWarlock, redWarlockPointLeft.position, Quaternion.identity);
            }
            else
            {
                Instantiate(redWarlock, redWarlockPointRight.position, Quaternion.identity);
            }

            
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            isInvincible = !isInvincible;
            PlayerHealthController.instance.isInvincible = isInvincible;

            if(isInvincible)
            {
                TurnPlayerMatWhite();
            }else
            {
                TurnPlayerMatDefault();
            }
        }
    }

    void TurnPlayerMatWhite()
    {
        PlayerController.instance.theSR.material = matWhite;
    }

    void TurnPlayerMatDefault()
    {
        PlayerController.instance.theSR.material = matDefault;
    }
}
