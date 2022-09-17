using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : Singleton<PrefabManager>
{
    public Transform projectileHolder;
    public Transform levelHolder;
    public Transform contentHolder;

    public void InitSingleton()
    {
        ClearContent();
    }

    public void ClearContent()
    {
        DestroyAllChildren(projectileHolder);
        DestroyAllChildren(levelHolder);
        DestroyAllChildren(contentHolder);
    }

    // Destroys all children of the transform
    private void DestroyAllChildren(Transform trans)
    {
        if (trans == null || trans.childCount == 0)
            return;

        for (int i = trans.childCount - 1; i >= 0; i--)
            Destroy(trans.GetChild(i));
    }
}
