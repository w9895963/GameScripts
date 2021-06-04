using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SceneBundle;
using UnityEngine;

namespace CMDBundle
{
    public class CommandFile
    {
        public static List<CommandFile> GlobalAllFiles = new List<CommandFile>();
        public GameObject currentObject;
        public string path;
        public string[] lines;
        public string sceneName;
        public int runOrder = 0;
        public Action afterFileExecute;
        public CommandLine[] commandLines;

        public string FolderName => FileF.GetFolderName(path);
        public string Name => Path.GetFileName(path);

        public string NameBody => Path.GetFileNameWithoutExtension(path);
        public List<CommandFile> AllFiles => GlobalAllFiles;






        public void ExecuteLines()
        {

            List<CommandLine> sortLines = commandLines.ToList();
            sortLines.SortBy(sortLines.Select((x) => x.runOrder).ToArray());
            sortLines.ForEach((line) =>
            {
                line.Execute();
            });
            afterFileExecute?.Invoke();
        }

        public CommandFile Find(string localPath)
        {
            string filePath = FileF.GetFullPathFromDataFolder($"Scripts/{localPath}");
            return GlobalAllFiles.Find((x) => FileF.AreSamePath(x.path, filePath));
        }


        public static CommandFile TryGetCommandFile(string path)
        {
            bool exist = File.Exists(path);
            if (!exist) { return null; }

            CommandFile file = new CommandFile();
            GlobalAllFiles.Add(file);
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



            List<CommandLine> lineCollector = new List<CommandLine>();
            for (int i = 0; i < allLine.Length; i++)
            {
                var line = allLine[i];

                CommandLine commandLine = CommandLine.TryGetCommandLine(line);

                lineCollector.Add(commandLine);
                commandLine.commandFile = file;

            }

            file.commandLines = lineCollector.ToArray();

            return file;
        }





    }















}
