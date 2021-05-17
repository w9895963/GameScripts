using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BasicEvent
{
    public static class Method
    {
       


        public static void Add<C>(GameObject gameObject, Action action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            c.action += action;
            c.actionCount++;
        }
        public static void Add<C, T>(GameObject gameObject, Action<T> action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                c = gameObject.AddComponent<C>();
            }
            Action<T> ac = c.action as Action<T>;
            ac += action;
            c.action_ = ac;
            c.actionCount++;
        }
        public static void Remove<C>(GameObject gameObject, Action action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                return;
            }

            c.action -= action;
            c.actionCount--;
            if (c.action == null)
            {
                c.destroyed = true;
                c.Destroy();
            }
        }
        public static void Remove<C, T>(GameObject gameObject, Action<T> action) where C : Component.BasicEventMono
        {
            var cs = gameObject.GetComponents<C>().ToList();
            C c = cs.Find((com) => com.destroyed == false);
            if (c == null)
            {
                return;
            }
            Action<T> ac = c.action as Action<T>;
            ac -= action;
            c.actionCount--;
            if (ac == null)
            {
                c.destroyed = true;
                c.Destroy();
            }
        }



    }
}

