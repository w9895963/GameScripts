using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CMDBundle
{
    namespace Editor
    {
        public class FileTitleUIBuilder : MonoBehaviour
        {
            public InputField titleText;
            public Dropdown lineHolder;
            public EditorUIBuilder editor;
            public CommandFile commandFile;


            List<CommandFile> searchResultFiles;
            CommandLine triggerLine => editor.TriggerCommandLine;

            public void Initial()
            {
                UpdateTitle();
                titleText.onValueChanged.AddListener((text) =>
                {
                    GamePlayF.Pause();
                });
                titleText.onEndEdit.AddListener((text) =>
                {
                    GamePlayF.Continue();
                    lineHolder.ClearOptions();
                    lineHolder.AddOptions(GetSearchResult(text).ToList());
                    lineHolder.Show();
                });


                lineHolder.onValueChanged.AddListener((ind) =>
                {
                    SummitSelect(ind);
                });
            }



            public void UpdateTitle()
            {
                titleText.text = commandFile.NameBody;
            }


            public string[] GetSearchResult(string titleChanged)
            {
                string[] searchWords = titleChanged.SplitWhite();
                searchResultFiles = commandFile.AllFiles.Where((x) => x.path.Contains(searchWords)).ToList();
                string scriptsFoldPath = (FileF.DataFolderPath + "/Scripts/").ToPath();
                IEnumerable<string> enumerable = searchResultFiles.Select((x) =>
                {
                    return x.path.TrimStart(scriptsFoldPath.ToCharArray());
                });
                return enumerable.ToArray();
            }


            public void SummitSelect(int ind)
            {
                CommandFile resultFile = searchResultFiles[ind];
                string scriptsFoldPath = (FileF.DataFolderPath + "/Scripts/").ToPath();
                string path = resultFile.path.TrimStart(scriptsFoldPath.ToCharArray());
                triggerLine.paramaters[0] = path;
                triggerLine.WriteLine();
                SceneManager.LoadScene(0);

            }

        }
    }

}
