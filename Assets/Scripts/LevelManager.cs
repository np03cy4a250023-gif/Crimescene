using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public int cluesFound = 0;
    public int totalClues = 4;

    public GameObject door;

    void Awake()
    {
        instance = this;
    }

    public void AddClue()
    {
        cluesFound++;

        Debug.Log("Clues: " + cluesFound);

        if (cluesFound >= totalClues)
        {
            Debug.Log("All clues found!");
            door.SetActive(false);
        }
    }
}