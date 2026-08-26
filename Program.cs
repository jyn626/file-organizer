namespace fileproj
{
  class FileManager
  {
    private static List<string> Files = new List<string>();

    public static void StoreFile(string fileName)
    {
      Files.Add(fileName);
    }

    public static int TotalFilesFound()
    {
      return Files.Count;
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
        FileManager.StoreFile(Path.GetFileName(file));
      }

      Console.WriteLine($"Total files found: {FileManager.TotalFilesFound()}");

    }
  }
}