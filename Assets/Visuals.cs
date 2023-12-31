using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Visuals : MonoBehaviour
{

    [System.Serializable]
    public struct SandColour {
        public Color MainColor;
        public Color SecondaryColor;
    }

    public Game game;
    public TextMeshProUGUI MiddleText;
    public TMP_InputField GoalInput;
    public TextMeshProUGUI TimerText;

    public Image BackgroundPanel;
    public Image SidePanelL;
    public Image SidePanelR;
    public SandColour[] SandColours;
    public BallManager ballManager;
    public float ColorLerpDuration = 3f;

    // cached values
    private Color targetMainColor;
    private Color targetSecondaryColor;
    private float ColorLerpT;

    private void Start() {
        game.OnSandsChanged += UpdateComponents;
        UpdateComponents();        
    }

    private void Update() {
        TimeSpan t = TimeSpan.FromSeconds(Mathf.RoundToInt(game.CurrentSandElapsed));
        TimerText.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
                t.Hours, 
                t.Minutes, 
                t.Seconds);

        if (ColorLerpT < 1) {
            BackgroundPanel.color = Color.Lerp(BackgroundPanel.color, targetMainColor, ColorLerpT);
            SidePanelL.color = Color.Lerp(SidePanelL.color, targetSecondaryColor, ColorLerpT);
            SidePanelR.color = SidePanelL.color;

            ColorLerpT += Time.deltaTime / ColorLerpDuration;
        } else {
            BackgroundPanel.color = targetMainColor;
            SidePanelL.color = targetSecondaryColor;
            SidePanelR.color = targetSecondaryColor;
        }
    }

    private void UpdateComponents() {
        if (game.CurrentSand.SandType == Game.SandType.Work) {
            GoalInput.gameObject.SetActive(true);
            GoalInput.text = "";
            MiddleText.gameObject.SetActive(false);
        } else {
            GoalInput.gameObject.SetActive(false);
            MiddleText.gameObject.SetActive(true);
        }

        if (game.CurrentSand.SandType == Game.SandType.Work) {
            MiddleText.text = "";
            ballManager.SetParticleMultiplierSpeed(1);
        } else if (game.CurrentSand.SandType == Game.SandType.Break) {
            MiddleText.text = "Takin A Break!";
            ballManager.SetParticleMultiplierSpeed(-2);
        } else if (game.CurrentSand.SandType == Game.SandType.Necessity) {
            MiddleText.text = "Life Stuff...";
            ballManager.SetParticleMultiplierSpeed(0);
        } else if (game.CurrentSand.SandType == Game.SandType.LongBreak) {
            MiddleText.text = "zzzzzzz";
            ballManager.SetParticleMultiplierSpeed(0);
        }

        Debug.Log((int)game.CurrentSand.SandType);

        targetMainColor = SandColours[(int)game.CurrentSand.SandType].MainColor;
        targetSecondaryColor = SandColours[(int)game.CurrentSand.SandType].SecondaryColor;
        ColorLerpT = 0;
    }




}
