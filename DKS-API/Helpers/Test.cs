using System.Collections.Generic;
using System.IO;

namespace DKS_API.Helpers
{
    public class Test
    {
        public Test()
        {
            Init();
        }

        public void Init()
        {
            string rootdir = Directory.GetCurrentDirectory();
            string folderPath = rootdir + "\\Resources\\ArticlePics";
            DirectoryInfo d = new DirectoryInfo(folderPath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
            List<string> fileNames = new List<string>();
            foreach (FileInfo file in Files)
            {
                fileNames.Add(file.Name);
            }
        }
    }
}