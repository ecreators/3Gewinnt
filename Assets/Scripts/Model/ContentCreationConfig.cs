using UnityEngine;

[CreateAssetMenu(fileName = "contentType", menuName = "Game/ContentType")]
public class ContentCreationConfig : ScriptableObject
{
    public ZellInhaltComponent contentTypePrefab;

    public EContentType ContentType => contentTypePrefab.contentType;
}