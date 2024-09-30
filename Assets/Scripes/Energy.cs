using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for using Slider

public class Energy : MonoBehaviour
{
    public float energyAmount = 25f; // Amount of energy to charge when the player triggers this prefab

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player triggered this object
        if (other.CompareTag("Player"))
        {
            // Get reference to the Slider and charge it
            Slider slider = FindObjectOfType<Slider>();
            if (slider != null)
            {
                // Increase the slider's value by the energy amount
                slider.value = Mathf.Clamp(slider.value + (energyAmount / 100f), 0f, 1f); // Assuming the slider max value is 1
            }

            // Destroy this prefab after being triggered
            Destroy(gameObject);
        }
    }
}
