using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worker_Ants
{
    class Dir_Ant
    {
        public void DirSearch(object str)
        {
            var sDir = Convert.ToString(str);
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    FileInfo fileInfo = new FileInfo(f);
                    if (fileInfo.Attributes.ToString().IndexOf("Hidden") == -1)
                    {
                        //SHA1 sha1 = new SHA1CryptoServiceProvider();
                        //string result = Convert.ToBase64String(sha1.ComputeHash(File.OpenRead(f)));

                        //var g1 = new GetMetadata();
                        //g1.Get(f);
                        Program.File_Dir.Push(f);
                        //Console.WriteLine($"{f}");
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                FileInfo fileInfo = new FileInfo(d);
                if (fileInfo.Attributes.ToString().IndexOf("Hidden") == -1)
                {
                    DirSearch(d);
                }
                //此目錄處理完再針對每個子目錄做處理 
            }
        }
    }
}
