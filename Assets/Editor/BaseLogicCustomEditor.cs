using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseLogic))]
public class BaseLogicCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BaseLogic logic = (BaseLogic)target;


        if(GUILayout.Button("Generate File"))
        {
            logic.GenerateFile();
        }

    }

}

