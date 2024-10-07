using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using OSK;

public class CharacterExample : MonoBehaviour
{
    private StatCalculator statCalculator;

    private void Awake()
    {
        InitBaseStats();
    }

    // Giá trị cơ bản cho các chỉ số
    private float DefenseBase = 10;

    // Thuộc tính để lưu giá trị đã lưu
   
    private float DefenseSave
    {
        get => PlayerPrefs.GetFloat("DefenseSave", DefenseBase);
        set => PlayerPrefs.SetFloat("DefenseSave", value);
    }

 
    private void InitBaseStats()
    {
        statCalculator = new StatCalculator();
        statCalculator.AddStat("Defense", DefenseBase);

        // Khôi phục giá trị đã lưu
        statCalculator.UpdateStatValue("Defense", DefenseSave);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ApplyModifier("Defense", new StatsModifier(ModifierType.Percent, 0.255f));
            SaveStats();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ResetStatsToBase();
        }
    }

    public void ApplyModifier(string statName, StatsModifier modifier)
    {
        statCalculator.AddModifier(statName, modifier);
        Debug.Log($"{modifier.modifierType} modifier of {modifier.Value} applied to {statName}. New value: {statCalculator.GetFinalStatValue(statName)}");
    }

    public void RemoveModifier(string statName, StatsModifier modifier)
    {
        statCalculator.RemoveModifier(statName, modifier);
        Debug.Log($"{modifier.modifierType} modifier of {modifier.Value} removed from {statName}. New value: {statCalculator.GetFinalStatValue(statName)}");
    }

    public float GetStat(string statName)
    {
        return statCalculator.GetFinalStatValue(statName);
    }

    // Lưu giá trị hiện tại của các chỉ số
    private void SaveStats()
    {
        DefenseSave = statCalculator.GetFinalStatValue("Defense");
        Debug.Log("DefenseSave."  + DefenseSave);
    }

    // Reset các chỉ số về giá trị cơ bản
    private void ResetStatsToBase()
    {
        statCalculator.UpdateStatValue("Defense", DefenseBase);
        Debug.Log("All stats reset to base values.");
    }
}