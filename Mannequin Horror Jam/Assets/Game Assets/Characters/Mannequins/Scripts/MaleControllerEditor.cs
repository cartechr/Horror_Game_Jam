using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaleController))]
public class MaleControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        MaleController fovR = (MaleController)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(fovR.transform.position, Vector3.up, Vector3.forward, 360, fovR.radiusRed, 2);

        MaleController fovY = (MaleController)target;
        Handles.color = Color.yellow;
        Handles.DrawWireArc(fovY.transform.position, Vector3.up, Vector3.forward, 360, fovY.radiusYellow, 2);

        MaleController fovG = (MaleController)target;
        Handles.color = Color.green;
        Handles.DrawWireArc(fovG.transform.position, Vector3.up, Vector3.forward, 360, fovG.radiusGreen, 2);

        MaleController fovAttack = (MaleController)target;
        Handles.color = Color.black;
        Handles.DrawWireArc(fovAttack.transform.position, Vector3.up, Vector3.forward, 360, fovAttack.radiusAttack, 2);

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
            Handles.color = Color.black;
            Handles.DrawLine(fovAttack.transform.position, fovAttack.playerRef.transform.position, 5);
        }
    }
}
