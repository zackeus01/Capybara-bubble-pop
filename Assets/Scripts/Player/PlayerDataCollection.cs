using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataCollection 
{
    [SerializeField] private float currentLv;
    [SerializeField] private float currenHp;
    [SerializeField] private float currentDef;
    [SerializeField] private float currentRes;
    [SerializeField] private float currentAtk;
    [SerializeField] private float currentCrit;
    [SerializeField] private float currentElDame;
    [SerializeField] private float currentMana;

    public PlayerDataCollection(float currentLv, float currenHp, float currentDef, float currentRes, float currentAtk, float currentCrit, float currentElDame, float currentMana)
    {
        this.currentLv = currentLv;
        this.currenHp = currenHp;
        this.currentDef = currentDef;
        this.currentRes = currentRes;
        this.currentAtk = currentAtk;
        this.currentCrit = currentCrit;
        this.currentElDame = currentElDame;
        this.currentMana = currentMana;
    }

    public PlayerDataCollection()
    {
        this.currentLv = 0;
        this.currenHp = 0;
        this.currentDef = 0;
        this.currentRes = 0;
        this.currentAtk = 0;
        this.currentCrit = 0;
        this.currentElDame = 0;
        this.currentMana = 0;
    }

    public float CurrentLv { get { return currentLv; }  set { currentLv = value; } }
    public float CurrenHp { get {  return currenHp; } set {  currenHp = value; } }
    public float CurrentDef { get {  return currentDef; } set { currentDef = value; } }
    public float CurrentRes { get {  return currentRes; } set { currentRes = value; } }
    public float CurrentAtk { get {  return currentAtk; } set { currentAtk = value; } }
    public float CurrentCrit { get {  return currentCrit; } set { currentCrit = value; } }
    public float CurrentElDame { get {  return currentElDame; } set { currentElDame = value; } }
    public float CurrentMana { get {  return currentMana; } set { currentMana = value; } }
}
