using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyController fovR = (EnemyController)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fovR.transform.position, Vector3.up, Vector3.forward, 360, fovR.radiusRed);

        EnemyController fovY = (EnemyController)target;
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fovY.transform.position, Vector3.up, Vector3.forward, 360, fovY.radiusYellow);

        EnemyController fovG = (EnemyController)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(fovG.transform.position, Vector3.up, Vector3.forward, 360, fovG.radiusGreen);

        EnemyController fovAttack = (EnemyController)target;
        Handles.color = Color.black;
        Handles.DrawWireArc(fovAttack.transform.position, Vector3.up, Vector3.forward, 360, fovAttack.radiusAttack);

        if (fovG.inGreen)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fovG.transform.position, fovG.playerRef.transform.position, 5);
        }

        if (fovG.inYellow)
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(fovG.transform.position, fovG.playerRef.transform.position, 5);
        }

        if (fovG.inRed)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fovG.transform.position, fovG.playerRef.transform.position, 5);
        }

        if (fovG.inAttack)
        {
            Handles.color = Color.gray;
            Handles.DrawLine(fovAttack.transform.position, fovAttack.playerRef.transform.position, 5);
        }
    }
}
