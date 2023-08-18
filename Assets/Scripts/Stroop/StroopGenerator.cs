using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class for the stroop objects.
public class StroopGenerator : MonoBehaviour
{
    [Header("UI Components")] 
    public TextMeshProUGUI stroopLabel;
    
    [Space] 
    [Header("Stroop components")] 
    public List<Color32> stroopColors;
    public List<string> stroopTexts;
    
    // Cap the possible range of compatibility chance to [0, 100].
    [Range(0,100)]
    public float chanceOfCompatibility;
    
    // Keep track of the current indices.
    private int _currentStroopColorIndex;
    private int _currentStroopTextIndex;

    /// This method runs once initially.
    private void Start()
    {
        // Initialize the color index.
        _currentStroopColorIndex = 0;
    }

    /// Method to get the stroop color.
    public void GetStroopColor()
    {
        // Assign a random color to our stroop object.
        _currentStroopColorIndex = Mathf.FloorToInt(Random.Range(0, stroopColors.Count));
        stroopLabel.color = stroopColors[_currentStroopColorIndex];
    }

    /// Method to get the text of the stroop.
    public void GetStroopText()
    {
        // If text and color are compatible, assign text to stroop object that equals the color.
        if (Random.Range(0, 100) <= chanceOfCompatibility)
        {
            _currentStroopTextIndex = _currentStroopColorIndex;
        }
        // Else: assign a random text to our stroop object.
        else
        {
            do
            {
                _currentStroopTextIndex = Mathf.FloorToInt(Random.Range(0, stroopTexts.Count));
            } while (_currentStroopTextIndex == _currentStroopColorIndex);
            
        }
        stroopLabel.text = stroopTexts[_currentStroopTextIndex];
    }
    
    /// Method for getting the current color of the stroop object (for ExperimentManager).
    public Color32 GetCurrentStroopColor()
    {
        return stroopColors[_currentStroopColorIndex];
    }
    
    /// Method for getting the current color name of the stroop object (for ExperimentManager).
    public string GetCurrentStroopColorName()
    {
        return stroopTexts[_currentStroopColorIndex];
    }
    
    /// Method for getting the current text of the stroop object (for ExperimentManager).
    public string GetCurrentStroopText()
    {
        return stroopTexts[_currentStroopTextIndex];
    }

    /// Method to check whether the color and text are compatible.
    public bool IsCompatible()
    { 
        // If the text equals the shown color return true, else return false.
        return (_currentStroopTextIndex == _currentStroopColorIndex) ? true : false;
    }
}
