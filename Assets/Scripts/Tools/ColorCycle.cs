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
    public float cycleIncrements = 0.01f;
    public float cycleMax = 1f;
    public float cycleMin = .5f;

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
                cycleColor.g += cycleIncrements;
                if (cycleColor.g >= cycleMax)
                {
                    cycleColor.g = cycleMax;
                    currentCycle = colorCycles.redDown;
                }
                break;
            case colorCycles.redDown:
                cycleColor.r -= cycleIncrements;
                if (cycleColor.r <= cycleMin)
                {
                    cycleColor.r = cycleMin;
                    currentCycle = colorCycles.blueUp;
                }
                break;
            case colorCycles.blueUp:
                cycleColor.b += cycleIncrements;
                if (cycleColor.b >= cycleMax)
                {
                    cycleColor.b = cycleMax;
                    currentCycle = colorCycles.greenDown;
                }
                break;
            case colorCycles.greenDown:
                cycleColor.g -= cycleIncrements;
                if (cycleColor.g <= cycleMin)
                {
                    cycleColor.g = cycleMin;
                    currentCycle = colorCycles.redUp;
                }
                break;
            case colorCycles.redUp:
                cycleColor.r += cycleIncrements;
                if (cycleColor.r >= cycleMax)
                {
                    cycleColor.r = cycleMax;
                    currentCycle = colorCycles.blueDown;
                }
                break;
            case colorCycles.blueDown:
                cycleColor.b -= cycleIncrements;
                if (cycleColor.b <= cycleMin)
                {
                    cycleColor.b = cycleMin;
                    currentCycle = colorCycles.greenUp;
                }
                break;
        }
        GetComponent<Camera>().backgroundColor = cycleColor;
    }
}
