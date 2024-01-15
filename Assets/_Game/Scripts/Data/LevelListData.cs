using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelListData", menuName = "ScriptableObjects/LevelListData", order = 3)]

public class LevelListData : ScriptableObject
{
    private static LevelListData instance;
    public static LevelListData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<LevelListData>("LevelListData");
            }
            return instance;
        }
    }
    public List<Level> levelList;
}