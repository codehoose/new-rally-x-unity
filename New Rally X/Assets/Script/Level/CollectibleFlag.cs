using UnityEngine;

public class CollectibleFlag : MonoBehaviour
{
    [SerializeField]
    private string _flagType;

    public string FlagType => _flagType;
}
