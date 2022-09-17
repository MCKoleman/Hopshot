using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentList", menuName = "ScriptableObjects/ContentList", order = 1)]
[System.Serializable]
public class ContentList : ScriptableObject
{
    [SerializeField]
    private WeightedGameObjectList enemies;

    public GameObject GetRandomContent(GlobalVars.ContentType content)
    {
        switch(content)
        {
            case GlobalVars.ContentType.ENEMY:
                return enemies.GetRandomObject();
            case GlobalVars.ContentType.DEFAULT:
            default:
                return null;
        }
    }
}
