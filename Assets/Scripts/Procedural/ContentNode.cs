using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentNode : MonoBehaviour
{
    public GlobalVars.ContentType content;

    // Initializes the content of this node
    public void InitContent()
    {
        SpawnManager.Instance.SpawnContent(content, this.transform.position);
    }
}
