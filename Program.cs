using System.Security.AccessControl;

namespace fileproj
{
  class FileManager
  {
    private static List<string> Files = new List<string>();

    // `root` is our source folder
    public static string root = "";

    // TODO: add more extension categories
    private static Dictionary<string, string> CategoryDict = new Dictionary<string, string>{
      { ".jfif", "Image"},
      { ".pdf", "Documents"},
    };

    public static void StoreFile(string fileName)
    {
      Files.Add(fileName);
    }

    public static void LogFilesMoved()
    {
      string[] files = Directory.GetFiles(root);

      // foreach (string file in files)
      // {
      //   Console.WriteLine("remaining files: ", files);
      // }
      // Console.WriteLine($"remaining files: {files.Length}");

      // if files array is empty and not NULL
      // if (files is [])
      if (files.Length == 0)
      {
        Console.WriteLine("All files are moved successfully.");
      }
      else
      {
        // TODO: implement a better solution in cases where a file isn't moved.
        Console.WriteLine("Some files failed to be moved.");
      }
    }

    public static void LogFilesMovedToCategory()
    {
      List<string> exts = GetExtensions();
      // List<string> categories = new List<string>();
      List<string> categories = new List<string>(CategoryDict.Values);

      Console.WriteLine(exts);

      // foreach (string ext in exts)
      // {
      //   Console.WriteLine(ext);
      //   if (!String.IsNullOrEmpty(ext))
      //   {
      //     categories.Add(GetCategory(ext));
      //   }
      // }

      foreach (string cat in categories)
      {
        string path = $"{root}/{cat}";
        // Console.WriteLine(cat);

        var files = Directory.GetFiles(path);

        foreach (string f in files)
        {
          string filename = Path.GetFileName(f);
          Console.WriteLine($"{filename} -> {cat}");
        }

      }

      // string[] files = Directory.GetFiles(categoryFolderPath);

      // if (files.Length == 0) { Console.WriteLine($"{category} is empty"); }
      // else
      // {
      //   foreach (string file in files)
      //   {
      //     Console.WriteLine($"{file} found in {category}");
      //   }
      // }
    }


    // <summary>
    // Move the file into the chosen category folder.
    // </summary>
    // <params name="categoryFolderPath">Destination file, move the file here.</params>
    public static void StoreToCategoryFolder(string file, string categoryFolderPath)
    {
      try
      {
        Console.WriteLine($"Category Folder Path: {categoryFolderPath}");
        if (!Directory.Exists(categoryFolderPath))
        {
          Directory.CreateDirectory(categoryFolderPath);
          Console.WriteLine($"{categoryFolderPath} created.");
        }

        // -  Modify folder access control
        // Get the current security/ACL settings
        DirectoryInfo dInfo = new DirectoryInfo(categoryFolderPath);
        DirectorySecurity dSecurity = dInfo.GetAccessControl();

        // Define the new access rule (Read & Write permissions in this case)
        FileSystemAccessRule accessRule = new FileSystemAccessRule(
          "Everyone",
          FileSystemRights.Read | FileSystemRights.Write,
          InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
          PropagationFlags.None,
          AccessControlType.Allow
        );

        // Add the rule to the existing security object
        dSecurity.AddAccessRule(accessRule);
        // Commit the security changes back to the folder
        dInfo.SetAccessControl(dSecurity);

        string destination = Path.Combine(categoryFolderPath, Path.GetFileName(file));

        File.Move(file, destination, overwrite: true);
      }
      catch (UnauthorizedAccessException e)
      {
        Console.WriteLine("Error: You do not have permission to create folders or file here.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
      }
    }

    public static int TotalFilesFound()
    {
      return Files.Count;
    }

    // <summary>
    // Gets every extentions for each file in the `Files` list.
    // </summary>
    // <returns>List of extensions.</returns>
    public static List<string> GetExtensions()
    {
      // ! TODO: Handle files with no extension (example: README)
      List<string> exts = new();
      foreach (string file in Files)
      {
        string ext = Path.GetExtension(file);

        // Convert extensions to a consistent format
        ext = ext.ToLower();
        exts.Add(ext);
      }

      return exts;
    }

    // <summary>
    // Gets the category of the file base on its extension, Image, Documents, Music, etc...
    // </summary>
    // <params name="ext">the extension of the given file.</params>
    // <returns>The category of the file from the dictionary, if not found then default to `Others`.</returns>
    public static string GetCategory(string ext)
    {
      return CategoryDict.TryGetValue(ext.ToLower(), out string? category) ? category : "Others";
    }

  }

  class Program
  {
    static void Main(String[] args)
    {
      Console.WriteLine("Enter the folder path to organize.");
      string folderPath = Console.ReadLine();

      // TODO: handle this in a better way
      if (String.IsNullOrEmpty(folderPath))
      {
        Console.WriteLine("Please enter the folder path.");
        return;
      }

      folderPath = folderPath.Trim();
      FileManager.root = folderPath;
      Console.WriteLine($"Folder path to organize: {folderPath}");

      // TODO: maybe start the program again instead of returning and ending the program?
      if (!Directory.Exists(folderPath))
      {
        Console.WriteLine("Path does not exists.");
        return;
      }

      Console.WriteLine("-  Folder found. Ready to scan.");


      // Get a list of files inside the messy folder (files only)
      string[] files = Directory.GetFiles(folderPath);

      // store every file name in the list
      foreach (string file in files)
      {
        string fileName = Path.GetFileName(file);
        string ext = Path.GetExtension(file);
        FileManager.StoreFile(fileName);

        // Print: file name → extension 
        Console.WriteLine($"{fileName} -> {ext}");
        Console.WriteLine($"Category: {FileManager.GetCategory(ext)}");

        FileManager.StoreToCategoryFolder(file, $"{folderPath}/{FileManager.GetCategory(ext)}");
      }

      Console.WriteLine($"Total files found: {FileManager.TotalFilesFound()}");

      // Confirm the file no longer exists in the messy folder root
      FileManager.LogFilesMoved();


      // Confirm the file exists inside the category folder
      // ConfirmFilesMovedToCategory(string category, string categoryFolderPath)
      FileManager.LogFilesMovedToCategory();


    }
  }
}