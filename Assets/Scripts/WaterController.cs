using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public AudioClip bubbleClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.waterAmount < controller.waterTotal)
            {
                controller.WaterScore(1);
                Destroy(gameObject);

                controller.PlaySound(bubbleClip);
            }
        }
    }
}
