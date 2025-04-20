using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewDistance);
        
        Vector3 ang01 = DirDadoAngulo(fov.transform.eulerAngles.y, -fov.viewAngle / 2);
        Vector3 ang02 = DirDadoAngulo(fov.transform.eulerAngles.y, fov.viewAngle / 2);
        
        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + ang01 * fov.viewDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + ang02 * fov.viewDistance);

        if (fov.canSeePlayer)
        {
         GameObject player = GameObject.FindGameObjectWithTag("Player");
         Handles.color = Color.green;
         Handles.DrawLine(fov.transform.position, player.transform.position);
        } 
    }

    private Vector3 DirDadoAngulo(float eulerY, float anguloEmGraus)
    {
        anguloEmGraus += eulerY;
        
        return new Vector3(Mathf.Sin(anguloEmGraus * Mathf.Deg2Rad), 0, Mathf.Cos(anguloEmGraus * Mathf.Deg2Rad));

    }
}