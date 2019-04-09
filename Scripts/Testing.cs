using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject gameObject;
    public Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        if (mesh == null && gameObject != null)
        {
        }
        mesh.vertices.ToList().ForEach(x => Debug.Log(x));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
