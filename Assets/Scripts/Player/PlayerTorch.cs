﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PlayerTorch : MonoBehaviour
{
    public int totalFlame = 130;
    public float currentFlame;
    public float decayRate = 0.1f;
    private bool isConsuming = true;
    public Light2D l;
    public TextMeshProUGUI text;
    private int nframes = 0;
    float desiredIntensity = 2;
    float desiredRange = 15;
    Player p;
    public Image fillbar;
    float dampening = 100;
    private void Update()
    {

            nframes++;
            if (nframes >= 10)
            {
                desiredRange = Random.Range(2.48f, 2.52f);
                desiredIntensity = Random.Range(1.7f, 2.2f);
                nframes = 0;
            }
            if(!p.tutorial)fillbar.fillAmount = currentFlame / totalFlame;
           

            if (p.tutorial)l.pointLightInnerRadius = desiredRange;
            if(!p.tutorial)l.intensity = desiredIntensity;
            if (currentFlame > totalFlame)
            {
                currentFlame = totalFlame;
            }
            if (currentFlame < 0)
            {
                p.Die();
            }
        
    }

  


    // Start is called before the first frame update
    void Start()
    {
        l = GetComponentInChildren<Light2D>();
        this.currentFlame = totalFlame;
        InvokeRepeating("Decay", 1f, 1f);
        p = GetComponent<Player>();
    }
    
    public void Recharge(int x)
    {
        this.currentFlame += x;
    }

    public void SetConsuming(bool consume)
    {
        isConsuming = consume;
    }

    public void Fill()
    {
        currentFlame = totalFlame;
    }

    // Update is called once per frame
    void Decay()
    {
        if (isConsuming)
        {
            this.currentFlame -= decayRate;
        }
    }
}
