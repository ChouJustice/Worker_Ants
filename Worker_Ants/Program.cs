using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker_Ants
{
    public class Program
    {
        static string filepath = System.Configuration.ConfigurationManager.AppSettings["PowerboxPath"];

        public static Stack<string> File_Dir = new Stack<string>();
        public static Stack<string> JSON_Data = new Stack<string>();

        static Dir_Ant dir_Ant = new Dir_Ant();
        static Parser_Ant A_parser_Ant = new Parser_Ant();
        static Parser_Ant B_parser_Ant = new Parser_Ant();
        static Post_Ant post_Ant = new Post_Ant();

        static Thread Dir_Thread = new Thread(dir_Ant.DirSearch);
        static Thread Parser_A_Thread = new Thread(A_parser_Ant.Get);
        static Thread Parser_B_Thread = new Thread(B_parser_Ant.Get);
        public static Thread Post_Thread = new Thread(post_Ant.Post_to_Solr);
        static void Main(string[] args)
        {
            //主執行序
            Console.WriteLine("Main Program Srart ...");

            Dir_Thread.Start(filepath);
            Parser_A_Thread.Start("A");
            Parser_B_Thread.Start("B");
            //Post_Thread.Start("POST");



            Console.ReadLine();

            Console.WriteLine(JSON_Data.Count);

            Console.ReadLine();
        }
    }
}
