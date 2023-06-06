namespace Pronia.Helper.FileManager
{
    public static class FileManager
    {
        public static string Save(string rootPath,string folder,IFormFile file)
        {
            string newPath=Guid.NewGuid().ToString()+(file.FileName.Length<=64?file.FileName:(file.FileName.Substring(file.FileName.Length-64)));
            string path = Path.Combine(rootPath,folder,newPath);
            using (FileStream stream=new FileStream(path,FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newPath;
        }
        public static bool Delete(string rootPath,string folder,string fileName)
        {
            string path=Path.Combine(rootPath,folder,fileName);
            if(File.Exists(path))
            {
                File.Delete(path); return true;
            }
            return false;
        }
        public static void DeleteAll(string rootPath, string folder, List<string> fileName)
        {
            foreach (var name in  fileName)
            {
                string path = Path.Combine(rootPath, folder, name);
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

    }
}
