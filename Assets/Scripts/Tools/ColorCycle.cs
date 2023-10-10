using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCycle : MonoBehaviour
{
    public Color cycleColor;

    public enum colorCycles
    {
        redUp,
        blueDown,
        greenUp,
        redDown,
        blueUp,
        greenDown
    }

    public colorCycles currentCycle;
    

    // Start is called before the first frame update
    void Start()
    {
        cycleColor = new Color(1f, .5f, .5f, 1f);
        currentCycle = colorCycles.greenUp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentCycle)
        {
            case colorCycles.greenUp:
                cycleColor.g += .01f;
                if (cycleColor.g >= 1)
                {
                    cycleColor.g = 1;
                    currentCycle = colorCycles.redDown;
                }
                break;
            case colorCycles.redDown:
                cycleColor.r -= .01f;
                if (cycleColor.r <= .5f)
                {
                    cycleColor.r = .5f;
                    currentCycle = colorCycles.blueUp;
                }
                break;
            case colorCycles.blueUp:
                cycleColor.b += .01f;
                if (cycleColor.b >= 1f)
                {
                    cycleColor.b = 1f;
                    currentCycle = colorCycles.greenDown;
                }
                break;
            case colorCycles.greenDown:
                cycleColor.g -= .01f;
                if (cycleColor.g <= .5f)
                {
                    cycleColor.g = .5f;
                    currentCycle = colorCycles.redUp;
                }
                break;
            case colorCycles.redUp:
                cycleColor.r += .01f;
                if (cycleColor.r >= 1f)
                {
                    cycleColor.r = 1f;
                    currentCycle = colorCycles.blueDown;
                }
                break;
            case colorCycles.blueDown:
                cycleColor.b -= .01f;
                if (cycleColor.b <= .5f)
                {
                    cycleColor.b = .5f;
                    currentCycle = colorCycles.greenUp;
                }
                break;
        }
        GetComponent<Camera>().backgroundColor = cycleColor;
    }
}
