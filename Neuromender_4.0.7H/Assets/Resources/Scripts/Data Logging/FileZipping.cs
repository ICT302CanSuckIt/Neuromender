using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;    // <-- DotNetZip library.

/// <summary>
/// File zipping/compression functionality.
/// 
/// This script uses the open-source DotNetZip third-party library. Site: https://dotnetzip.codeplex.com/
/// 
/// The reason this third-party library is currently required is because as of the time this script was written Unity (version 5.4) uses .NET 2.x / 3.x
/// This is significant because native file compression support (at least with password protection) in .NET is added in version 4.x (4.5 I think?), which makes it unusable (currently) in unity projects.
/// As this project needs password protection for privacy and security reasons, password functionality is required immediately (thus using DotNetZip).
/// 
/// If in the future Unity shifts to .NET 4.5 or higher then consider (if necessary) switching to System.IO.Compression (with an assembly reference to System.IO.Compression.FileSystem).
/// Hopefully this advice is still useful / relevant when a change is required!!!
/// 
/// Also, a friendly reminder to remove the obsolete assembly reference to Ionic.Zip if you do change it :D
/// 
/// </summary>
public static class FileZipping {

    /// <summary>
    /// 
    /// Create a zip folder with a single file inside. The password (if specified) is placed on the zip file itself.
    /// 
    /// Note: By default, the original file is deleted once it has been zipped, because I figured that this was what would happen most often. It can be turned off with the last param.
    /// 
    /// </summary>
    /// <param name="dataFileName">The name of the data file being stored in the zip file created by this function.</param>
    /// <param name="outputFileName">The name of the zip file created by this function (extension optional).</param>
    /// <param name="user">The user associated with the data being stored in this zip file.</param>
    /// <param name="password">OPTIONAL -- The password required to open the zip file that is created in the same function call.</param>
    /// <param name="deleteUnzipped">OPTIONAL -- TRUE: Deletes the original copy of the file that was zipped.</param>
	public static void ZipData(string dataFileName, string outputFileName, string user, string password = "", bool deleteUnzipped = true)
    {
        // Debug.Log("Zipping Data!");

        // Exit early if the password flag is raised but an INVALID password is specified.
        if (password == "" || password == null)
        {
            Debug.LogWarning("No password specified even though password flag was raised. Operation was cancelled before executing.");
            return;
        }

        string userDirectory = user;

        // Ensure that the directory ends with a seperator for the parent folder before saving the zip file.
        if (!userDirectory.EndsWith("/"))
            userDirectory += "/";

        // Ensure that the correct file extension exists before saving the zip file.
        if (!outputFileName.EndsWith(".zip"))
            outputFileName += ".zip";

        // Ensure that the directory where you are saving this zip file exists.
        if (!Directory.Exists(Application.dataPath + "/Data/" + userDirectory))
            Directory.CreateDirectory(Application.dataPath + "/Data/" + userDirectory);

        // Put the whole path in a string because it will be used elsewhere.
        string filePath = Application.dataPath + "/Data/" + userDirectory + dataFileName;

        // If the file doesn't exist, dont bother trying to zip it.
        if (File.Exists(filePath))
        {
            using (ZipFile zip = new ZipFile())
            {
                // Set the password for unzipping the zip file (if one is specified).
                if (password != "" && password != null)
                    zip.Password = password;

                // Add the performance data file to the zip file's root directory.
                zip.AddFile(filePath, "");

                // Save the zip file.
                zip.Save(Application.dataPath + "/Data/" + userDirectory + outputFileName);

                // If the original file is flagged for deletion, delete it.
                if (deleteUnzipped)
                {
                    Debug.Log("Deleting file: " + filePath);
                    File.Delete(filePath);
                }
            }
        }
        else
        {
            Debug.LogWarning("Could not find file from path: " + filePath);
        }
    }

    /// <summary>
    /// 
    /// Create a zip folder with multiple files inside.
    /// Passwords are applied to the individual files only.
    /// 
    /// </summary>
    /// <param name="dataFiles">A list of files to be included in the zip file created by this function.</param>
    /// <param name="outputFileName">The name of the zip file created by this function (extension optional).</param>
    /// <param name="directoryPath">The file path where the zip will be saved.</param>
    public static void ZipData(List<ZipDataEntry> dataFiles, string outputFileName, string directoryPath)
    {
        // Debug.Log("Zipping Data!");

        int fileCheck = 0;  // To check if an empty zip file will be created.

        // Ensure that the correct file extension exists before saving the zip file.
        if (!outputFileName.EndsWith(".zip"))
            outputFileName += ".zip";

        // Ensure that the directory ends with a seperator for the parent folder before saving the zip file.
        if (!directoryPath.EndsWith("/"))
            directoryPath += "/";

        using (ZipFile zip = new ZipFile())
        {
            for(int i = 0; i < dataFiles.Count; ++i)
            {
                // Log an error if a file doesn't exist, otherwise add a new entry to the zip file.
                if(File.Exists(dataFiles[i].filePath))
                {
                    fileCheck++;

                    // Create a handle for the latest entry to the zip file, in case a password is needed.
                    ZipEntry entry = zip.AddFile(dataFiles[i].filePath, dataFiles[i].zipDirectory);

                    // Only apply a password to the data file IF the corresponding flag has been raised.
                    if (dataFiles[i].password != "" && dataFiles[i].password != null)
                        entry.Password = dataFiles[i].password;
                }
                else
                {
                    Debug.LogWarning("Could not find file from path: " + dataFiles[i].filePath);
                }
            }

            if (fileCheck > 0)
                zip.Save(directoryPath + outputFileName);   // Save the new zip file.
            else
                Debug.LogWarning("Empty zip file due to no valid files being found. Zip creation was cancelled.");

            // Remove any original files flagged for deletion.
            for (int i = 0; i < dataFiles.Count; ++i)
                if (dataFiles[i].deleteOriginal && File.Exists(dataFiles[i].filePath))
                {
                    Debug.Log("Deleting file: " + dataFiles[i].filePath);
                    File.Delete(dataFiles[i].filePath);
                }
        }
    }

    /// <summary>
    /// Extract a file from a zip. WIP, currently untested.
    /// </summary>
    /// <param name="zipFile">The full path of the zip file.</param>
    /// <param name="fileZipDirectory">The full path to the file (within the zip file).</param>
    /// <param name="fileName">The name of the file within the zip.</param>
    /// <param name="outDirectory">The directory that the extracted file will be sent to.</param>
    /// <param name="password">The password (if any) required to open the file.</param>
    public static void UnZipDataStream(string zipFile, string fileZipDirectory, string fileName, string outDirectory, string password = "")
    {
        // Mak esure that the zip file extension is attached to the end of the file name specified, before anything else is done.
        if (!zipFile.EndsWith(".zip"))
            zipFile += ".zip";

        // Make sure that the directory path ends with a seperator for the parent folder.
        if (!outDirectory.EndsWith("/"))
            outDirectory += "/";

        // If the zip file exists, open it and continue, otherwise return a null stream.
        if (ZipFile.IsZipFile(zipFile))
        {
            using (ZipFile zip = ZipFile.Read(zipFile))
            {
                foreach (ZipEntry e in zip.Entries)
                {
                    // If the file was found, extract it to the specified output directory, and raise a flag.
                    if (e.FileName == fileName)
                    {
                        // If there is a password attached to the zipped file, use the password extraction method, otherwise just do a normal extraction.
                        if (password != "")
                            e.ExtractWithPassword(outDirectory, password);
                        else
                            e.Extract(outDirectory);
                    }
                }
            }
        }
    }
}

/// <summary>
/// Entry for a single file to be put in a zip file. Simplified to just the filename and password parameters for convienience.
/// </summary>
public class ZipDataEntry
{
    public string filePath = null;      // Name of the file.
    public string zipDirectory = "";    // The directory that this file will be placed under when added to the zip file.
    public string password = "";        // Password associated with this file IF the fag is raised.
    public bool deleteOriginal = false; // TRUE: Delete the original file once it has been stored in the zip file. FALSE: Leave the original file intact.

    /// <summary>
    /// Auxilary Constructor for the ZipDataEntry class. Allows user to set all internal params.
    /// </summary>
    /// <param name="path">Name of the associated file (full path required).</param>
    /// <param name="dir">Directory the file will be under in the zip file.</param>
    /// <param name="Password">Password that will be put on the associated file (an empty or null string means NO password).</param>
    /// <param name="destroyOriginal">TRUE: The original copy of the file is removed once it is zipped.</param>
    public ZipDataEntry(string path, string dir, string Password, bool destroyOriginal = false)
    {
        filePath = path;
        zipDirectory = dir;
        password = Password;
        deleteOriginal = destroyOriginal;
    }
}