using Global;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public System.Object obj;
    private void Reset()
    {
        Debug.Log(gameObject.GetComponentInChildren<Test>());
        Debug.Log(gameObject.GetComponentInParent<Test>());
        TestF(1);
        int a = 1;
        obj = a;
        Debug.Log(obj);
        Debug.Log(obj is int);
        Debug.Log(obj is float);
        Debug.Log((int)obj);
        Vector2 v = Vector2.right;
        obj = v;
        Debug.Log(obj);
        Debug.Log((Vector2)obj);
    }
    public void TestF(System.Object obj)
    {
    }
}