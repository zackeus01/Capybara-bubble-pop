using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player/Status")]

public class PlayerBaseStatus : ScriptableObject
{
    [SerializeField] private int maxLv;
    [SerializeField] private int baseHp;
    [SerializeField] private int baseDef;
    [SerializeField] private int baseRes;
    [SerializeField] private int baseAtk;
    [SerializeField] private int baseCrit;
    [SerializeField] private int baseElDame;
    [SerializeField] private int baseMana;
    [SerializeField] private int baseCoin;

    public int MaxLv {  get { return maxLv; } set {  maxLv = value; } }
    public int BaseHp { get {  return baseHp; } set {  baseHp = value; } }
    public int BaseDef { get {  return baseDef; } set { baseDef = value; } }
    public int BaseRes { get {  return baseRes; } set { baseRes = value; } }
    public int BaseAtk { get {  return baseAtk; } set { baseAtk = value; } }
    public int BaseCrit { get {  return baseCrit; } set { baseCrit = value; } }
    public int BaseElDame { get { return baseElDame; } set { baseElDame = value; } }
    public int BaseMana { get {  return baseMana; } set { baseMana = value; } }
    public int BaseCoin { get {  return baseCoin; } set {  BaseCoin = value; } }
}
