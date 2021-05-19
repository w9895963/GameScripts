using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CommandFileBundle
{
    public class CommandFile
    {
        public GameObject gameObject;
        public string path;
        List<CommandLine> commandLines = new List<CommandLine>();




        public static void Execute(string path)
        {
            bool exist = File.Exists(path);
            if (!exist) { return; }
            CommandFile file = new CommandFile();
            file.path = path;
            string[] allLine = File.ReadAllLines(path);
            allLine.ForEach((line) =>
            {
                CommandLine commandLine = CommandLine.TryGetCommandLine(line);
                if (commandLine != null)
                {
                    file.commandLines.Add(commandLine);
                    commandLine.commandFile = file;
                }
            });
            file.commandLines.ForEach((cl) =>
            {
                cl.ExecuteNew();
            });
        }


    }



    public class CommandLine
    {

        public CommandFile commandFile;
        string title;
        public string[] paramaters;



        public GameObject GameObject
        {
            get => commandFile?.gameObject;
            set
            {
                if (commandFile == null)
                { return; }
                commandFile.gameObject = value;
            }
        }

        public string Path => commandFile.path;

        public void Execute()
        {
            Action<CommandLine> action = ActionFinder.FindAction(title);
            action?.Invoke(this);
        }
        public void ExecuteNew()
        {
            GameObject gameObject = GameObjectF.CreateFromPrefab("CommandPrefab" + "\\" + title);
            ActionBaseComponent action = gameObject.GetComponent<ActionBaseComponent>();
            action.commandLine = this;
            action.Execute();
        }

        public T ReadParam<T>(int index)
        {
            return (T)Convert.ChangeType(paramaters[index], typeof(T)); ;
        }
        public T[] ReadParams<T>()
        {
            T[] ts = paramaters.ToList().Select((x) => (T)Convert.ChangeType(x, typeof(T))).ToArray();
            return ts;
        }



        public static CommandLine TryGetCommandLine(string line)
        {
            CommandLine re = null;
            (string title, string[] paramaters) p = CommandLineContentSpliter.Split(line);
            if (!p.title.IsEmpty())
            {
                re = new CommandLine();
                re.title = p.title;
                re.paramaters = p.paramaters;
            }
            return re;
        }

        public static class CommandLineContentSpliter
        {
            const string SplitPartten = @"[\s,，]+";

            public static (string title, string[] paramaters) Split(string line)
            {
                (string title, string[] paramaters) result = (null, null);
                List<string> vs = Regex.Split(line, SplitPartten).ToList();
                vs.RemoveAll((x) => x.IsEmpty());
                if (vs.Count > 0)
                {
                    result.title = vs[0];
                    if (vs.Count > 1)
                    {
                        result.paramaters = vs.GetRange(1, vs.Count - 1).ToArray();
                    }
                }
                return result;
            }


        }

    }




}
