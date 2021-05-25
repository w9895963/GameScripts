using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SceneBundle;
using UnityEngine;

namespace CommandFileBundle
{
    public class CommandFile
    {
        public GameObject currentObject;
        public string path;
        public string[] lines;
        public string sceneName;
        public int runOrder = 0;
        public Action afterFileExecute;
        public List<CommandLine> commandLines = new List<CommandLine>();

        public string FolderName => FileF.GetFolderName(path);
        public string Name => Path.GetFileName(path);

        public string NameBody => Path.GetFileNameWithoutExtension(path);

        public void ReWriteFile(string title, string newLine)
        {
            FileF.ReWriteOrAddLine(path, title, newLine);
        }




        public void ExecuteLines()
        {
            commandLines.ForEach((line) =>
            {
                line.Execute();
            });
            afterFileExecute?.Invoke();
        }


        public static CommandFile TryGetCommandFile(string path)
        {
            bool exist = File.Exists(path);
            if (!exist) { return null; }

            CommandFile file = new CommandFile();
            file.path = path;

            string[] allLine = File.ReadAllLines(path);
            file.lines = allLine;
            TrySetRunOrder();
            void TrySetRunOrder()
            {
                if (allLine.Length > 0)
                {
                    int i;
                    bool v = int.TryParse(allLine[0], out i);
                    if (v)
                    {
                        file.runOrder = i;
                    }
                }
            }




            for (int i = 0; i < allLine.Length; i++)
            {
                var line = allLine[i];

                CommandLine commandLine = CommandLine.TryGetCommandLine(line);

                file.commandLines.Add(commandLine);
                commandLine.commandFile = file;

            }

            return file;
        }


    }



    public class CommandLine
    {
        const string CommandActionFolder = "CommandPrefab";


        public CommandFile commandFile;
        public Action<CommandLine> action;
        public string title;
        public string[] paramaters;



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
        public List<CommandLine> AllLines => commandFile.commandLines;

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
        public int ParamsLength => paramaters.IsEmpty() ? 0 : paramaters.Length;




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

            list = paramaters.ToList().GetRange(startIndex, length < 0 ? paramaters.Length : length);

            T[] ts = list.Select((x) => (T)Convert.ChangeType(x, typeof(T))).ToArray();
            return ts;
        }

        public CommandLine GetLine(int lineIndex)
        {
            return commandFile.commandLines.Find((x) => x.lineIndex == lineIndex);
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
            li.paramaters = p.paramaters;

            CommandLineActionHolder actionHolder = prefab.GetComponent<CommandLineActionHolder>();
            li.action = actionHolder.Action;
            return li;
        }

        public void Execute()
        {
            action?.Invoke(this);
        }

        public void WriteLine()
        {
            FileF.WriteLine(Path, lineIndex, stringLine);
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
