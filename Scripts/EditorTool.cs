using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(GameObject))]
public class EditorTool : Editor
{
    Ray ray;
    RaycastHit hit;
    public static Vector3 lastVector;
    public static bool enable = false;
    public static GameObject currentSelect;
    public static List<ModelData> models = new List<ModelData>();

    static EditorTool()
    {
        //EditorApplication.update += Update;
    }

    void OnSceneGUI()
    {
        if (enable)
        {
            Vector3 vert = new Vector3(0, 0, 0);
            ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Handles.color = Color.red;
                //Handles.CubeCap(0, hit.point, hit.transform.rotation, 0.1f);
                Handles.DotCap(1, hit.point, hit.transform.rotation, 0.01f);
                //Get Nearest Vertices

                Mesh mesh = hit.collider.gameObject.GetComponent<MeshFilter>().sharedMesh;
                float dist = float.MaxValue;
                vert = mesh.vertices[0];
                mesh.vertices.ToList().ForEach(x =>
                {
                    Vector3 worldPt = hit.collider.gameObject.transform.TransformPoint(x);
                    if (dist > Vector3.Distance(worldPt, hit.point))
                    {
                        dist = Vector3.Distance(worldPt, hit.point);
                        vert = worldPt;
                    };
                });

                if(vert != lastVector) 
                    Handles.DrawRectangle(2, vert, Quaternion.LookRotation((SceneView.lastActiveSceneView.camera.transform.position - vert), Vector3.up), 0.05f);
            }
            if(lastVector != new Vector3(0,0,0))
            {
                Handles.color = Color.blue;
                Handles.DrawRectangle(3, lastVector, Quaternion.LookRotation((SceneView.lastActiveSceneView.camera.transform.position - lastVector), Vector3.up), 0.05f);
            }

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    break;
                case EventType.MouseDown:
                    if (e.button == 0 && e.alt)
                    {
                        lastVector = vert;
                        currentSelect = hit.collider.gameObject;
                    }     
                    break;
            }
            SceneView.RepaintAll();
        }
    }

    [MenuItem("Tools/Enable")]
    static void Enable()
    {
        enable = !enable;
        SceneView.RepaintAll();
    }
    
    [MenuItem("Tools/Save")]
    static void Save()
    {
        string json = JsonConvert.SerializeObject(new ModelData { Vector = new VectorWrap { x = lastVector.x, y = lastVector.y, z = lastVector.z }, GameObjectName = currentSelect.name }, Formatting.Indented);
        Debug.Log(json);
    }
}

public struct ModelData
{
    public VectorWrap Vector { get; set; }
    public string GameObjectName { get; set; }
}

public struct VectorWrap
{
    public float x, y, z;
}

public enum ModelWeaponTypes
{
    Sword,
    Bow,
    Staff
}

public enum ModelPartTypes
{

}