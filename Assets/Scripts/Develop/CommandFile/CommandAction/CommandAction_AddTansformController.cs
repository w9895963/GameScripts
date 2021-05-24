using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CommandFileBundle
{
    namespace ActionComponent
    {
        public class CommandAction_AddTansformController : CommandLineActionHolder
        {
            const string prefabPath = "Prefab\\Gismo\\TransformController";
            public override void Action(CommandLine cl)
            {
                GameObject gismo = GameObjectF.CreateFromPrefab(prefabPath);
                GameObject obj = cl.GameObject;
                CommandFile file = cl.commandFile;
                Vector2 p = obj.GetPosition2d();
                gismo.SetPosition(p);
                gismo.SetParent(obj.GetParent());
                ObjectTransformSetter[] sets = gismo.GetComponentsInChildren<ObjectTransformSetter>();
                sets.ForEach((x) =>
                {
                    x.targetObject = obj;
                });

                ObjectDate.OnDateUpdate(obj, ObjectDateType.Position2DLo, (d) =>
                {
                    Vector2 v = (Vector2)d;
                    string line = $"位置 {v.x} {v.y}";
                    file.ReWriteFile("位置", line);
                });
                ObjectDate.OnDateUpdate(obj, ObjectDateType.Scale2D, (d) =>
                {
                    Vector2 v = (Vector2)d;
                    string line = $"缩放 {v.x} {v.y}";
                    file.ReWriteFile("缩放", line);
                });
                ObjectDate.OnDateUpdate(obj, ObjectDateType.Rotation1D, (d) =>
                {
                    float v = (float)d;
                    string line = $"旋转 {v}";
                    file.ReWriteFile("旋转", line);
                });


            }




        }
    }

}
