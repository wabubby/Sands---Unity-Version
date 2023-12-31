using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    
    [System.Serializable]
    public enum SandType {
        Work,
        Break,
        Necessity,
        LongBreak
    }

    [System.Serializable]
    public class Sand {
        // each segment tracks a sand and when that sand started
        public float StartTime;
        public SandType SandType;

        public Sand(float startTime, SandType sandType) {
            StartTime = startTime;
            SandType = sandType;
        }
    }

    public List<Sand> TodaySands;
    public Sand CurrentSand;

    // properties
    public float CurrentSandElapsed {
        get {
            if (CurrentSand == null) {
                return 0f;
            } else {
                return GetTodayTime() - CurrentSand.StartTime;
            }
        }
        
    }

    // cached values
    [SerializeField] private double EpochTime;
    [SerializeField] private double TodayTime;

    // events
    public delegate void OnSandsChangedHandler(); public event OnSandsChangedHandler OnSandsChanged = delegate {};

    private void OnEnable() {
        TodayTime = GetTime();
        CurrentSand = new Sand(GetTodayTime(), SandType.Necessity);
    }

    private double GetTime() {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        double timeElapsed = (DateTime.UtcNow - epochStart).TotalSeconds;
        
        return timeElapsed;
    }

    private float GetTodayTime() {
        return (float) (GetTime() - TodayTime);
    }

    public void StartSand(int sandType) {
        StartSand((SandType) sandType);
    }

    public void StartSand(SandType sandType) {
        TodaySands.Add(CurrentSand);
        CurrentSand = new Sand(GetTodayTime(), sandType);
        OnSandsChanged();
    }

    private void Update() {
        if (CurrentSand.SandType == SandType.Break && CurrentSandElapsed > 15*60) {
            CurrentSand.SandType = SandType.LongBreak;
            OnSandsChanged();
        }
    }

    private void OnDisable() {
        SaveProgress();
    }

}
