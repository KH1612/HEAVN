using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;
    public TextMeshProUGUI collectibleText;
    private int count = 0;

    void Awake()
    {
        instance = this;
    }
    public int GetCount()
    {
        return count;
    }
    public void AddCollectible(int value)
    {
        count += value;
        collectibleText.text = ": " + count;
    }
}