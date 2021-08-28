using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

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
        this.DirToOpen = getAllDirectoriesWithDrilling("");
        if (DirToOpen.Length > 0)
        {
            // deleteDirectorisContainsName(subDirectories, "ceg");
            // deleteNotRelevantDirectoriesWithDateNames(dir, subDirectories);
            List<string> fileToCompile = getAllMainCppFileFromAllDirectories();
            compile(fileToCompile);
        }
    }

    private List<string> getAllMainCppFileFromAllDirectories()
    {
        List<string> cppMainFilesPath = new List<string>();
        foreach (string dir in this.DirToOpen)
        {
            string[] subDirectories = getAllDirectoriesWithDrilling(dir);
            foreach (string studentWorkingDir in subDirectories)
            {
                List<string> filesInDirectory = Directory.GetFiles(studentWorkingDir).ToList();
                foreach (string file in filesInDirectory)
                {
                    if (file.ToLower().Contains("main.") || file.ToLower().Contains("source."))
                    {
                        cppMainFilesPath.Add(file.ToLower());
                    }
                }
            }
        }
        
        return cppMainFilesPath;
    }

    private void deleteNotRelevantDirectoriesWithDateNames(string i_Directory, string[] i_SubDirectories)
    {
        List<DateTime> directoriesAsDates = new List<DateTime>();
        foreach (string subDirectory in i_SubDirectories)
        {
            directoriesAsDates.Add(ConvertStrangeDateStringToDateTime(subDirectory.Remove(0, i_Directory.Length + 1)));
        }
        DateTime[] sortedDates = directoriesAsDates.ToArray();
        Array.Sort(sortedDates);
        if (sortedDates.Length > 0)
        {
            string mostRelevant = sortedDates[sortedDates.Length - 1].ToString("yyyy-MM-dd-HH-mm-ss");
            string mostRelevantPath = Path.Combine(i_Directory, mostRelevant);
            foreach (string subDirectory in i_SubDirectories)
            {
                if (!(subDirectory.Equals(mostRelevantPath)))
                {
                    Directory.Delete(subDirectory, true);
                }
            }
        }
    }

    private static void deleteDirectorisContainsName(string[] i_Sub, string i_Name)
    {
        foreach (string subDir in i_Sub)
        {
            if (subDir.Contains(i_Name))
            {
                Directory.Delete(subDir, true);
                Console.WriteLine(subDir);
            }
        }
        Console.WriteLine();
    }

    public DateTime ConvertStrangeDateStringToDateTime(string i_RawDate)
    {
        //e.g, based on structure : "2021-08-14-23-29-21"
        StringBuilder sb = new StringBuilder(i_RawDate);
        sb[10] = ' ';
        string[] dateParts = sb.ToString().Split(' ');
        dateParts[0] = dateParts[0].Replace("-", "/");
        dateParts[1] = dateParts[1].Replace("-", ":");

        string newString = $"{dateParts[0]} {dateParts[1]}";

        return Convert.ToDateTime(newString);
    }

    private void compileCPPFiles()
    {
        getAllDirectoriesWithDrilling("");
        List<string> cppFilesToCompile = collectAllCppFilesToCompile();
        compile(cppFilesToCompile);
    }

    private void compile(List<string> i_CppFilesToCompile)
    {
        // initilize the process with the cl compiler
        ProcessStartInfo psi = new ProcessStartInfo(@"cmd");
        psi.UseShellExecute = false;
        psi.CreateNoWindow = false;
        psi.RedirectStandardInput = true;
        var proc = Process.Start(psi);
        StreamWriter writer = proc.StandardInput;
    
        writer.WriteLine(@"cd C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools");
        writer.WriteLine("VsDevCmd.bat");

        // start the compile process for each file
        foreach (string currentFile in i_CppFilesToCompile)
        {
            string filePath = Path.GetDirectoryName(currentFile);
            writer.WriteLine(@"cd {0}", filePath);
            string destinationFile = Path.GetFileName(currentFile);
            writer.WriteLine(@"cl {0} ", destinationFile);
        }
    }

    private List<string> collectAllCppFilesToCompile()
    {
        List<string> cppFiles = new List<string>();
        foreach (string directory in DirToOpen)
        {
            cppFiles.AddRange(collectCppFilesFromOneDirectory(directory));
        }
        return cppFiles;
    }

    private List<string> collectCppFilesFromOneDirectory(string i_Directory)
    {
        string[] filesInDir = Directory.GetFiles(i_Directory);
        List<string> cppFiles = new List<string>();
        foreach (string file in filesInDir)
        {
            if (Path.GetExtension(file).ToLower().Contains("cpp"))
            {
                cppFiles.Add(file);
            }
        }
        return cppFiles;
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

    private string[] getAllDirectoriesWithDrilling(string i_Directory)
    {
        if (string.IsNullOrEmpty(i_Directory))
        {
            i_Directory = this.workingPath;
        }

        return Directory.GetDirectories(i_Directory, "*.*",SearchOption.AllDirectories);
    }

}
