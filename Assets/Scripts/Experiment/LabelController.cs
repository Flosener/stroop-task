using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelController : MonoBehaviour
{
    // Define a Text and color object for usage in ShowText -> ShowMessage (in ExperimentManager Script)
    public TextMeshProUGUI label;
    public Color32 labelColor;

    /// Method for assigning a message and a color to the text on the screen.
    public void ShowText(string message)
    {
        label.text = message;
        label.color = labelColor;
    }
}
