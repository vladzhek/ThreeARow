using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumScript : MonoBehaviour
{
    public float amplitude = 1.5f; // Амплитуда колебаний маятника
    public float frequency = 2f; // Частота колебаний
    public float offsetY = 3f; // Вертикальное смещение маятника

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * frequency) * amplitude;
        float yPosition = Mathf.Abs(Mathf.Cos(Time.time * frequency) * offsetY);
        transform.position = 
            initialPosition + new Vector3(angle, -yPosition, 0f);
    }
}
