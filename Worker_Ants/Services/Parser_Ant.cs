using java.io;
using org.apache.tika.metadata;
using org.apache.tika.parser;
using org.apache.tika.sax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Worker_Ants
{
    class Parser_Ant
    {
        public void Get(object threadname)
        {
            System.Console.WriteLine("Wait for file path 3 second ..");
            Thread.Sleep(3000);
            Stopwatch s = new Stopwatch();
            s.Start();//開始計時

            Dictionary<string, string> value;

            while (Program.File_Dir.Count != 0)
            {
                Parser parser = new AutoDetectParser();
                Metadata metadata = new Metadata();
                ParseContext pcontext = new ParseContext();
                BodyContentHandler handler = new BodyContentHandler(-1);


                //System.Console.WriteLine(threadname.ToString());
                string filename = "";

                System.Console.WriteLine();
                try
                {
                    filename = Program.File_Dir.Pop();
                    java.io.File document = new java.io.File(filename);
                    //System.Console.WriteLine("========Read======="+filename);
                    parser.parse(new FileInputStream(document), handler, metadata, pcontext);
                }
                catch (InvalidOperationException)
                {
                    System.Console.WriteLine("堆疊為空");
                    break;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(filename + ": parser error" + ex);
                    System.Console.WriteLine();
                    continue;
                }

                value = new Dictionary<string, string>();
                value.Add("id", filename);
                foreach (var prop in metadata.names())
                {
                    if (prop.Contains("TRC") || prop.Contains("Byte")) continue;
                    value.Add(prop, metadata.get(prop).ToString());
                    //System.Console.WriteLine($"{prop} =  {metadata.get(prop)}");
                }
                if (handler.toString() != "")
                {
                    var str = Regex.Replace(handler.ToString(), @"\s", "");
                    //string str = handler.ToString();
                    if (str.Length < 65535)
                        value.Add("content", str);
                }
                //PostData(filename, value);
                Program.JSON_Data.Push(JsonConvert.SerializeObject(value));
                if(Program.Post_Thread.ThreadState == System.Threading.ThreadState.Unstarted && Program.JSON_Data.Count > 10)
                {
                    //System.Console.WriteLine("POST" + " : " + Program.Post_Thread.ThreadState);
                    Program.Post_Thread.Start("POST");
                    //System.Console.WriteLine("POST" + " : " + Program.Post_Thread.ThreadState);
                }
                if(Program.Post_Thread.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    //System.Console.WriteLine("POST" + " : " + Program.Post_Thread.ThreadState);
                    Program.Post_Thread.Resume();
                    //System.Console.WriteLine("POST" + " : " + Program.Post_Thread.ThreadState);
                }
            }
            s.Stop();
            System.Console.WriteLine(threadname.ToString() + "," + (s.Elapsed).ToString());
        }
    }
}
