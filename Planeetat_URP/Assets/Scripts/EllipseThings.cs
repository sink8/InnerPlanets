using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EllipseThings
{

    public float xAxix;
    public float yAxix;

    public EllipseThings(float xAxix, float yAxix)
    {
        this.xAxix = xAxix;
        this.yAxix = yAxix;
    }

    public Vector2 Evaluate(float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * xAxix;
        float y = Mathf.Cos(angle) * yAxix;

        return new Vector2(x, y);
    }

}
