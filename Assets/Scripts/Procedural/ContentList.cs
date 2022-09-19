using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentList", menuName = "ScriptableObjects/ContentList", order = 1)]
[System.Serializable]
public class ContentList : ScriptableObject
{
    [SerializeField]
    private WeightedGameObjectList enemies;
    [SerializeField]
    private WeightedGameObjectList obstacles;
    [SerializeField]
    private WeightedGameObjectList hazards;
    [SerializeField]
    private WeightedGameObjectList pits;

    public GameObject GetRandomContent(GlobalVars.ContentType content)
    {
        //Debug.Log($"Called GetRandomContent on content type [{content.ToString()}]");
        switch(content)
        {
            case GlobalVars.ContentType.ENEMY:
                return enemies.GetRandomObject();
            case GlobalVars.ContentType.OBSTACLE:
                return obstacles.GetRandomObject();
            case GlobalVars.ContentType.HAZARD:
                return hazards.GetRandomObject();
            case GlobalVars.ContentType.PIT:
                return pits.GetRandomObject();
            case GlobalVars.ContentType.DEFAULT:
            default:
                return null;
        }
    }
}
