using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUITextMesh : MonoBehaviour
{
    public int endLevel;
    public int maxNextRoomCount;
    public int notAccessibleLevel;

    public List<LevelHolderUI> Stages;

    public bool check;

    public void Awake()
    {
        Stages = new List<LevelHolderUI>();
    }

    public void Update()
    {
        if (check) {
            Stages.Clear();
            ArrangeAllLevels();
            check = false;
            MakeLevelsNotAccessible(notAccessibleLevel);
            //Debug.Log("Calistim");
        }
    }

    private void ArrangeAllLevels()
    {
        for (int i = 0; i < endLevel ; i++) {
            LevelHolderUI levelHolderUI = new LevelHolderUI();
            for (int k = 0; k < maxNextRoomCount; k++)
            {
                string wantedLevel = "Room" + (i + 1) + "." + (k + 1);
                if (GameObject.Find(wantedLevel))
                {
                    levelHolderUI.addToLevel(GameObject.Find(wantedLevel));
                    //Debug.Log(wantedLevel);
                }
            }
            Stages.Add(levelHolderUI);
        }
    }

    public void MakeLevelsNotAccessible(int input)
    {
        //Debug.Log("su kadar level var burada " + Stages[input-1].getlevels().Count);
        foreach (GameObject temp in Stages[input-1].getlevels())
        {
            //Debug.Log(temp.name);
            temp.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.1f);
            temp.GetComponent<Button>().enabled = false;
            temp.GetComponent<Shadow>().enabled = false;
        }
    }
}
public class LevelHolderUI
{
    private List<GameObject> levels;

    public LevelHolderUI()
    {
        levels = new List<GameObject>();
    }

    public void addToLevel(GameObject gameObject)
    {
        levels.Add(gameObject);
    }

    public void clearLevel()
    {
        levels.Clear();
    }

    public List<GameObject> getlevels()
    {
        return levels;
    }
}

