using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Diagnostics;

public class CheckController
{
    public string workingPath { get; set; }
    public string commentsFileName { get; set; }
    public string[] DirToOpen { get; set; }
    public CheckController(string i_WorkingPath, string i_CommentsFileName)
    {
        this.workingPath = i_WorkingPath;
        this.commentsFileName = i_CommentsFileName;
    }

    public void Run()
    {
        getAllDirectories();
        if (DirToOpen.Length > 0)
        {
            openCompressedFiles();
            if (!string.IsNullOrEmpty(this.commentsFileName))
            {
                copyCommentsFile();
            }
        }
    }

    private void copyCommentsFile()
    {
        foreach (string directory in this.DirToOpen)
        {
            try
            {
                File.Copy(Path.Combine(workingPath, this.commentsFileName), Path.Combine(directory, this.commentsFileName));
            }
            catch (Exception ignored)
            {
            }   
        }
    }

    private void openCompressedFiles()
    {
        foreach (string direcory in this.DirToOpen)
        {
            string[] FilesInDirectory = Directory.GetFiles(direcory);
            foreach (string  file in FilesInDirectory)
            {
                FileInfo fileInfo = new FileInfo(file);
                string currentFileExtension = fileInfo.Extension.ToLower();

                if (currentFileExtension.Contains("rar"))
                {
                    extractRar(fileInfo);
                }
                else if (currentFileExtension.Contains("zip"))
                {
                    extractZip(fileInfo);
                }
            }
            
        }
    }

    private void extractRar(FileInfo i_File)
    {
        try
        {
            string source = i_File.FullName.ToString();
            string destinationFolder = i_File.DirectoryName;
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "\"C:\\Program Files\\WinRAR\\winrar.exe\"";
            p.StartInfo.Arguments = string.Format(@"x -s ""{0}"" *.* ""{1}\""", source, destinationFolder);
            p.Start();
            p.WaitForExit();
            Console.WriteLine($"extract file : ${i_File.FullName}");
        }
        catch (Exception ignoredExp) { }
    }

    private void extractZip(FileInfo i_File)
    {
        try
        {
            ZipFile.ExtractToDirectory(i_File.FullName, i_File.DirectoryName, Encoding.GetEncoding("Windows-1252"));
            Console.WriteLine($"extract file : ${i_File.FullName}");
        }
        catch (Exception ignoredExp){}
    }

    private void getAllDirectories()
    {
        this.DirToOpen = Directory.GetDirectories(this.workingPath);
    }
}
