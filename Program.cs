namespace fileproj
{
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

    }
  }
}