using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVTools
{
    /// This method converts our data into comma separated value format.
    public static List<string> GenerateCSV(List<List<string>> data, List<string> csvHeaders, char separator = ',')
    {
        // Initialize a list of strings which will contain a row of data.
        List<string> convertedData = new List<string>();
        string convertedHeaders = "";
        
        // Convert each header to comma separated value format.
        foreach (string header in csvHeaders)
        {
            convertedHeaders = convertedHeaders + header + separator;
        }
        
        // Convert the data to comma separated value format.
        foreach (List<string> rowData in data)
        {
            string convertedRowData = "";
            foreach (string dataItems in rowData)
            {
                convertedRowData = convertedRowData + dataItems + separator;
            }
            convertedData.Add(convertedRowData);
        }
        convertedData.Add(convertedHeaders);
        return convertedData;
    }

    /// This method saves the data in a csv file.
    public static bool SaveCSV(List<string> csvLines, string fileAddress, string extension = ".csv")
    {
        try
        {
            using (StreamWriter csvWriter = new StreamWriter(fileAddress + extension))
            {
                foreach (string csvLine in csvLines)
                {
                    csvWriter.WriteLine(csvLine);
                }
                csvWriter.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }

        return true;
    }
}
