using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker_Ants
{
    class Post_Ant
    {
        public void Post_to_Solr(object threadname)
        {
            Console.WriteLine("Post start ...");
            while(Program.File_Dir.Count != 0 && Program.JSON_Data.Count != 0)
            {
                if (Program.JSON_Data.Count == 0)
                {
                    Console.WriteLine(threadname.ToString() + " ::: " + Program.Post_Thread.ThreadState);
                    #pragma warning disable CS0618 // 類型或成員已經過時
                    Program.Post_Thread.Suspend();
                    #pragma warning restore CS0618 // 類型或成員已經過時
                    Console.WriteLine(threadname.ToString() + " ::: " + Program.Post_Thread.ThreadState);
                }

                Program.JSON_Data.Pop();
            }
            Console.WriteLine("Postover");
        }
    }
}
