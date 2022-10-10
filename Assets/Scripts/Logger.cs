using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Logger
{
    public static void CreateFile(string filePath)
    {
        try
        {
            File.Create(filePath);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public static void DeleteFile(string filePath)
    {
        try
        {
            File.Delete(filePath);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public static void Append(string filePath, string textLine)
    {
        try
        {
            using (var fs = new StreamWriter(filePath, true, System.Text.Encoding.GetEncoding("UTF-8")))
            {
                fs.Write(textLine);
                fs.Write("\n");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public static void CreateDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static string FormHeader()
    {
        string header = "";
        header += "baseTrackerPos.x,";
        header += "baseTrackerPos.y,";
        header += "baseTrackerPos.z";
        return header;
    }

    public static string FormBodyLine(ManipulationData manipulationData)
    {
        string bodyLine = "";
        bodyLine += $"{manipulationData.baseTrackerPosition.x},";
        bodyLine += $"{manipulationData.baseTrackerPosition.y},";
        bodyLine += $"{manipulationData.baseTrackerPosition.z}";
        return bodyLine;
    }
}
