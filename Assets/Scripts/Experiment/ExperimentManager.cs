using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Main class for execution of the experiment.
public class ExperimentManager : MonoBehaviour
{
    // Get objects the classes StroopGenerator and LabelController to use their methods.
    [Header("Experiment components")]
    public StroopGenerator stroopGenerator;
    public LabelController labelController;

    // Define variables for maximum number of tasks and blocks for the experiment.
    [Header("Block variables")] 
    public int numberOfBlocks;
    public int numberOfTasks;

    // Variables to keep track of the current task/block number.
    private int _currentBlockNumber;
    private int _currentTaskNumber;

    // Boolean to check if we are currently running a block.
    private bool _isBlockRunning;
    
    // Float variables to keep track of the participants reaction time.
    private float _stimuliOnsetTime;
    private float _reactionTime;
    
    // Variables to save data that is important for the experiment.
    private List<List<string>> _data;
    private List<string> _csvHeaders;

    /// This method runs once initially at the beginning of the experiment.
    private void Start()
    {
        ShowMessage(Color.white, "Welcome to the experiment! Press 'space' to begin.");

        // Initialize our variables.
        _currentBlockNumber = 0;
        _currentTaskNumber = 0;
        _data = new List<List<string>>();
        _csvHeaders = new List<string>();
        _csvHeaders.Add("block_number");
        _csvHeaders.Add("task_number");
        _csvHeaders.Add("color_name");
        _csvHeaders.Add("text");
        _csvHeaders.Add("compatibility");
        _csvHeaders.Add("correctness");
        _csvHeaders.Add("reaction_time");
        
        // The block is not running initially.
        _isBlockRunning = false;
    }

    /// This (main) method runs every frame.
    private void Update()
    {
        // Check if the block is running.
        if (_isBlockRunning)
        {
            // Participant answered 'yes' (color matches the text).
            if (Input.GetKeyDown(KeyCode.Y))
            {
                Answer(true);
            }
            // Participant answered 'no' (color does not match the text).
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Answer(false);
            }
            
        }
        // If the block currently is not running, hit space to start the next block.
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetNextBlock();
            }
        }
    }

    /// Method to get the next task.
    private void GetNextTask()
    {
        // Except the last task (because we start with 0), print the current task number.
        if (_currentTaskNumber != numberOfTasks)
        {
            Debug.Log(string.Format("The current task number is {0}", _currentTaskNumber+1));   
        }
        
        // If we did not answered all tasks, get a new one and keep track of the time since start-up.
        if (_currentTaskNumber < numberOfTasks)
        {
            stroopGenerator.GetStroopColor();
            stroopGenerator.GetStroopText();
            _stimuliOnsetTime = Time.realtimeSinceStartup;
            _currentTaskNumber++;
        }
        // If we answered all the tasks in the block switch to next block if there is another one.
        else
        {
            if (_currentBlockNumber < numberOfBlocks)
            {
                ShowMessage(Color.white, "You reached the end of the block. Press space to continue.");
            }
            else
            {
                GetNextBlock();
            }
            _isBlockRunning = false;
        }
    }

    /// Method to get the next block.
    private void GetNextBlock()
    {
        /*
        if (_currentBlockNumber != numberOfBlocks)
        {
            Debug.Log(string.Format("The current block number is {0}", _currentBlockNumber+1));   
        }
        */
        
        // If there are blocks left, reset the task number, increment block number and get to the first task of the new block.
        if (_currentBlockNumber < numberOfBlocks)
        {
            ResetTaskNumber();
            _currentBlockNumber++;
            _isBlockRunning = true;
            GetNextTask();
        }
        // Else: End the experiment.
        else
        {
            ShowMessage(Color.white, "You reached the end of the experiment. Thank you for participating!");
            List<string> csvLines = CSVTools.GenerateCSV(_data, _csvHeaders);
            foreach (string line in csvLines)
            {
                Debug.Log(line);
            }
            // Creates the csv data file in the corresponding data folder.
            CSVTools.SaveCSV(csvLines, Application.dataPath + "/Data/" + GUID.Generate());
        }
    }

    /// Method to show a message on the screen.
    private void ShowMessage(Color32 color, string message)
    {
        labelController.labelColor = color;
        labelController.ShowText(message);
    }
    
    private void ResetTaskNumber()
    {
        _currentTaskNumber = 0;
    }
    
    /// Main method for console messages after the participant answered yes/no.
    private void Answer(bool answer)
    {
        // Define local variable to save the data in question.
        List<string> taskData = new List<string>();
        
        // Keep track of users reaction time.
        _reactionTime = Time.realtimeSinceStartup;
        
        // Add the data in question to our local list.
        taskData.Add(_currentBlockNumber.ToString());
        taskData.Add(_currentTaskNumber.ToString());
        taskData.Add(stroopGenerator.GetCurrentStroopText().ToString());
        taskData.Add(stroopGenerator.GetCurrentStroopColorName().ToString());
        taskData.Add(stroopGenerator.IsCompatible().ToString());
        taskData.Add((stroopGenerator.IsCompatible() == answer).ToString());
        taskData.Add((_reactionTime - _stimuliOnsetTime).ToString());
        _data.Add(taskData);
        
        //Debug.Log(string.Format("Current color is {0}", stroopGenerator.GetCurrentStroopColor()));
        //Debug.Log(string.Format("Current color is {0}", stroopGenerator.GetCurrentStroopColorName()));
        //Debug.Log(string.Format("Current text is {0}", stroopGenerator.GetCurrentStroopText()));
        //Debug.Log(string.Format("Is stroop compatible? {0}", stroopGenerator.IsCompatible()));
        Debug.Log(string.Format("Did participant answer correctly? {0}", stroopGenerator.IsCompatible() == answer));
        Debug.Log(string.Format("It took you {0} seconds to answer.", (_reactionTime - _stimuliOnsetTime)));
        GetNextTask();
    }
}
