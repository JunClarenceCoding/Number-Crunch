using UnityEngine;
using System.Collections.Generic;
public enum StatType
{
    Currency,
    Level,
    MaxWave,
    StagesReached
}
public enum SpriteType
{
    TypeA,
    TypeB
}
[System.Serializable]
public class Achievement
{
    public string achievementId;    
    public string description;   
    public int targetValue;         
    public StatType statType;      
    public Sprite sprite1;              
    public Sprite sprite2;            
    public SpriteType spriteType;       
    public int rewardAmount;   
    public bool isUnlocked = false;
    public bool claimed = false;
    public RewardItem reward; 
}