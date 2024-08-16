using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolConfigMenu : MonoBehaviour
{

    public Skill[] Skills;


    public void DebugGenerateUI() {
        foreach (Skill skill in Skills) {

        }
    }

}

public struct Skill {

    public string Name;
    public string Description;
    public Sprite Icon;
    public int[] Cost;          // Cost for each level of this skill, in chips. [1,2,3] means 1 chip for the first level, 2 chips for the second level, etc.

}
