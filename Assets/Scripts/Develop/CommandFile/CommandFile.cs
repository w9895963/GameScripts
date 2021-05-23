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
        public GameObject gameObject;
        public string path;
        public string[] lines;
        List<CommandLine> commandLines = new List<CommandLine>();

        public string FolderName => FileF.GetFolderName(path);

        public void ReWriteFile(string title, string newLine)
        {
            FileF.ReWriteOrAddLine(path, title, newLine);
        }

        public void AddCommandLinesToScene()
        {
            commandLines.ForEach((cl) =>
           {
               cl.AddToScene();
           });
        }


        public static CommandFile TryGetCommandFile(string path)
        {
            bool exist = File.Exists(path);
            if (!exist) { return null; }

            CommandFile file = new CommandFile();
            file.path = path;
            string[] allLine = File.ReadAllLines(path);
            file.lines = allLine;


            for (int i = 0; i < allLine.Length; i++)
            {
                var line = allLine[i];

                CommandLine commandLine = CommandLine.TryGetCommandLine(line);
                if (commandLine != null)
                {
                    file.commandLines.Add(commandLine);
                    commandLine.commandFile = file;
                    commandLine.lineIndex = i;
                }
            }

            return file;
        }


    }



    public class CommandLine
    {
        const string CommandActionFolder = "CommandPrefab";
        public CommandFile commandFile;
        public Action<CommandLine> action;
        public SceneBundle.SceneBuildEvent sceneBuildEvent;
        public string originLine;
        public int lineIndex;
        public string title;
        string[] paramaters;



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
        public string FolderName => commandFile.FolderName;
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

        public void AddToScene()
        {
            SceneHolder sceneHolder = SceneF.FindOrCreateScene(FolderName);
            sceneHolder.comandLines.Add(this);

        }


        public static CommandLine TryGetCommandLine(string line)
        {

            (string title, string[] paramaters) p = CommandLineSpliter.Split(line);
            if (p.title.IsEmpty()) { return null; }

            string actionPath = CommandActionFolder + "\\" + p.title;
            GameObject prefab = ResourceLoader.Load<GameObject>(actionPath);
            if (prefab == null) { return null; }

            CommandLine li = null;
            li = new CommandLine();
            li.originLine = line;
            li.title = p.title;
            li.paramaters = p.paramaters;

            CommandLineActionHolder actionHolder = prefab.GetComponent<CommandLineActionHolder>();
            li.action = actionHolder.Action;
            li.sceneBuildEvent = actionHolder.onSceneEvent;
            return li;
        }

        public void Execute()
        {
            action?.Invoke(this);
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
