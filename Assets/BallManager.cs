using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {

    public ParticleSystem[] BallParticleSystems;

    public float yMultiplier;

    private void Update() {
        SetParticleMultiplierSpeed(yMultiplier);
    }

    public void SetParticleMultiplierSpeed(float myMult) {
        yMultiplier = myMult;
        foreach (ParticleSystem particleSystem in BallParticleSystems) {
            var velocityOverLifetime =  particleSystem.velocityOverLifetime;
            velocityOverLifetime.yMultiplier = yMultiplier;
        }
    }

}
