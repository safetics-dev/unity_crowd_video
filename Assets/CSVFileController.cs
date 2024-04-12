

using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using UnityEngine;

public class CSVFileController
{
    private bool isWriting;
    private string filePath;
    public string currentDate;
    private string fileTitle;
    private string fileTotalPath;
    private StreamWriter writer = null;

    public CSVFileController()
    {
        currentDate = DateTime.Now.ToString("yyMMdd-HHmmss-temp");
    }

    public CSVFileController(string filePath, string fileTitle)
    {
        currentDate = DateTime.Now.ToString($"yyMMdd-HHmmss-{fileTitle}");
        this.fileTitle = fileTitle;
        this.filePath = filePath;
    }

    public CSVFileController(string fileTitle)
    {
        currentDate = DateTime.Now.ToString($"yyMMdd-HHmmss-{fileTitle}");
        this.fileTitle = fileTitle;
    }

    public void CreateCSVFile(string title, string path)
    {
        try
        {
            this.filePath = path;

            FileInfo fi = new FileInfo(title);
            if(fi.Extension.ToLower() != ".csv")
            {
                this.fileTitle = title + ".csv";
            }    

            DirectoryInfo di = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), this.filePath));
            if(di.Exists == false)
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), this.filePath));
            }

            this.fileTotalPath = Path.Combine(Directory.GetCurrentDirectory(), this.filePath, $"{DateTime.Now.ToString("yyMMdd-HHmmss")}-{this.fileTitle}");

            File.Create(this.fileTotalPath);


            writer = new StreamWriter(this.fileTotalPath, true);
        }
        catch (Exception ex)
        {
            var st = ex.Message;
        }
        
    }

    public void writeToCSVFile(List<string> rowData, string delimeter = ",")
    {
        if (isWriting)
            return;

        isWriting = true;

        StringBuilder sb = new StringBuilder();
        for(int index = 0; index < rowData.Count; index++)
        {
            sb.AppendLine(string.Join(delimeter, rowData));
        }

        using(var fileStream = new FileStream(this.fileTotalPath, FileMode.Append, FileAccess.Write))
        {
            StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
            outStream.WriteLine(sb);
            outStream.Close();
            fileStream.Close();
        }

        isWriting = false;
    }

    public void appendToCSVFile(List<string> rowData, string delimeter = ",")
    {
        if (isWriting)
            return;

        isWriting = true;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Join(delimeter, rowData));

        try
        {
            //using(var fileStream = new FileStream(this.fileTotalPath, FileMode.Append, FileShare.Write /*FileAccess.Write*/))
            //using (var fileStream = new FileStream(this.fileTotalPath, FileMode.Append, FileShare.Write /*FileAccess.Write*/))
            //{
            //    StreamWriter outStream = new StreamWriter(fileStream, Encoding.UTF8);
            //    outStream.WriteLine(sb);
            //    outStream.Close();
            //    fileStream.Close();
            //}    

            writer.WriteLine("test");

            isWriting = false;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}