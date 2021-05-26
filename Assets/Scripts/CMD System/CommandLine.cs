using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;



namespace CMDBundle
{
    public class CommandLine
    {
        const string CommandActionFolder = "CommandPrefab";


        public CommandFile commandFile;
        public Action<CommandLine> action;
        public string title;
        public int runOrder = 0;
        public List<string> paramaters;



        public GameObject GameObject
        {
            get => commandFile?.currentObject;
            set
            {
                if (commandFile == null)
                { return; }
                commandFile.currentObject = value;
            }
        }

        public bool Empty => title.IsEmpty();
        public CommandLine PreLine => AllLines.FindPrevious((x) => x == this);
        public List<CommandLine> AllLines => commandFile.commandLines.ToList();

        public int lineIndex => AllLines.FindIndex((x) => x == this);

        public string stringLine
        {
            get
            {
                string line = "";
                line += title + " ";
                paramaters.ForEach((p) => line += p + " ");
                return line;
            }
        }

        public string Path => commandFile.path;
        public string SceneName => commandFile.sceneName;
        public string FolderName => commandFile.FolderName;
        public string FileName => commandFile.Name;
        public string FileNameBody => commandFile.NameBody;
        public int ParamsLength => paramaters.IsEmpty() ? 0 : paramaters.Count;




        public T ReadParam<T>(int index)
        {
            return (T)Convert.ChangeType(paramaters[index], typeof(T)); ;
        }
        public T[] ReadParams<T>(int startIndex = 0, int length = -1)
        {
            if (paramaters.IsEmpty())
            {
                return new T[0];
            }
            List<string> list;

            list = paramaters.ToList().GetRange(startIndex, length < 0 ? paramaters.Count : length);

            T[] ts = list.Select((x) => (T)Convert.ChangeType(x, typeof(T))).ToArray();
            return ts;
        }

       


        public void WriteLine()
        {
            FileF.WriteLine(Path, lineIndex, stringLine);
        }



        public void Execute()
        {
            action?.Invoke(this);
        }


        public static CommandLine TryGetCommandLine(string line)
        {

            CommandLine li = new CommandLine();
            (string title, string[] paramaters) p = CommandLineSpliter.Split(line);
            if (p.title.IsEmpty()) { return li; }

            string actionPath = CommandActionFolder + "\\" + p.title;
            GameObject prefab = ResourceLoader.Load<GameObject>(actionPath);
            if (prefab == null) { return li; }

            li.title = p.title;
            li.paramaters = p.paramaters.ToList();

            CommandActionHolder actionHolder = prefab.GetComponent<CommandActionHolder>();
            li.action = actionHolder.Action;
            li.runOrder = actionHolder.RunOrder;
            return li;
        }
        public static class CommandLineSpliter
        {
            const string SplitPartten = @"[\s,，]+";
            const string RemoveParteen = @"[(（].*?[)）]";


            public static (string title, string[] paramaters) Split(string line)
            {
                (string title, string[] paramaters) result = (null, null);
                line = Regex.Replace(line, RemoveParteen, "");
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
