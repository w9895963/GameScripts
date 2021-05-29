using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UIBundle
{




    public class Table
    {
        public string tablePath = "Prefab/UI_Editor_Table";
        public GameObject tableObject;
        public GameObject ItemHolder => tableObject.GetComponent<Component.Table>().ItemHolder;

        public void GetTable()
        {
            GameObject can = UIF.GetCanvas();
            tableObject = GameObjectF.FindOrCretePrefab(tablePath, can);
        }

        public void AddItem(TableItem item)
        {
            item.table = this;
            item.Create();
            item.Setup();
        }
        public void AddItem(IEnumerable<TableItem> items)
        {
            items.ForEach((it) =>
            {
                AddItem(it);

            });

        }

        public void DestroyAllItem()
        {
            ItemHolder.GetDirectChildren().Destroy();
        }

    }

    public class TableItem
    {
        public Table table;
        public GameObject gameObject;
        public virtual string prefabPath => null;
        public void Create()
        {
            gameObject = table.ItemHolder.CreateChildFrom(prefabPath);
        }

        public virtual void Setup()
        {

        }
    }



}
