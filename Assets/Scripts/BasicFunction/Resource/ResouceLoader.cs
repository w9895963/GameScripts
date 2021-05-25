using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ResourceLoader
{
    public static T Load<T>(string path, bool reload = false) where T : UnityEngine.Object
   => ResourceLoaderBundle.ResourceLoader_Load.Load<T>(path, reload);


    public static void LazyLoad<T>(string path, Action<T> action) where T : UnityEngine.Object
    => ResourceLoaderBundle.ResourceLoader_LazyLoad.LazyLoad<T>(path, action);
}
