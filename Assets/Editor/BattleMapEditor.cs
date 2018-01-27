using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleMap))]
public class BattleMapEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		if (GUILayout.Button ("Advance")) {
			BattleMap variable = (BattleMap)target;
			variable.Advance ();
		}
		if (GUILayout.Button ("Retreat")) {
			BattleMap variable = (BattleMap)target;
			variable.Retreat ();
		}
	}

}