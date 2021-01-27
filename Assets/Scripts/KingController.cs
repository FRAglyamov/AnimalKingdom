using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    public KingType kingType;
    public float curHP;
    public short playerNumber;
    public short firstSkillCDTimeLeft;
    public short secondSkillCDTimeLeft;
    GameCanvas gameCanvas;

    private void Start()
    {
        gameCanvas = GameObject.FindGameObjectWithTag("Game Canvas").GetComponent<GameCanvas>();
        if (curHP == 0f)
            curHP = kingType.maxHP;
        firstSkillCDTimeLeft = kingType.firstSkill.initialCD;
        secondSkillCDTimeLeft = kingType.secondSkill.initialCD;
        UpdateCooldownInCanvas();
        UpdateHpInCanvas();
    }

    public void MinusCD()
    {
        if (firstSkillCDTimeLeft > 0)
            firstSkillCDTimeLeft -= 1;
        if (secondSkillCDTimeLeft > 0)
            secondSkillCDTimeLeft -= 1;
        UpdateCooldownInCanvas();
    }

    public void UpdateCooldownInCanvas()
    {
        gameCanvas.UpdateCanvasSkillText(playerNumber, firstSkillCDTimeLeft, secondSkillCDTimeLeft);
    }

    public void UpdateHpInCanvas()
    {
        gameCanvas.UpdateCanvasHpText(playerNumber, curHP);
    }
}