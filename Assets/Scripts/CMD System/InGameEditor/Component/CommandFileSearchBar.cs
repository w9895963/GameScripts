using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace CMDBundle
{
    namespace Editor
    {
        public class CommandFileSearchBar : MonoBehaviour
        {
            public InputField searchInputField;
            public GameObject resultHolder;
            const string resultItemPrefabPath = "CommandPrefab/Editor/SearchResultItem";
            const string searchBarPrefabPath = "CommandPrefab/Editor/InGameEditorSearchBar";
            const string edittorPrefabPath = "CommandPrefab/Editor/InGameEditor";


            private void Start()
            {
                searchInputField?.onValueChanged.AddListener((str) =>
                {
                    List<CommandFile> commandFiles = CommandFile.AllFiles.FindAll((f) => f.NameBody.Contains(str));
                    resultHolder.GetAllChild().Destroy();
                    commandFiles.ForEach((file) =>
                    {
                        GameObject item = resultHolder.CreateChildFrom(resultItemPrefabPath);
                        item.GetComponentInChildren<Text>().text = file.NameBody;
                        item.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            BuildEditor(file);
                        });
                    });

                    float height = UIF.ChildrenHeight(resultHolder);
                    RectTransform rect = resultHolder.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);


                });
            }




            public static void BuildEditor(CommandFile file)
            {
                GameObject editor = GameObjectF.FindOrCretePrefab(edittorPrefabPath);
                GameObject lineHolder = editor.FindChild("Content");
                file.commandLines.ForEach((line) =>
                {
                    EditorCommandLine.BuildLine(line, lineHolder);
                });
            }


            static bool showup = false;
            public static void ToggleShowUp()
            {
                GameObject bar = GameObjectF.FindOrCretePrefab(searchBarPrefabPath);
                if (CommandFileSearchBar.showup)
                {
                    showup = false;
                    bar.Destroy();
                }
                else
                {
                    showup = true;
                }

            }
        }
    }
}
