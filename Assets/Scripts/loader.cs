using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class loader : MonoBehaviour
{
    public string target;
    Material mat;
    private MeshStudy mesh;

    void Start()
    {
        target = "C:/work/cube.obj";
        mat = Resources.Load <Material > ("mesh");
    }

    public void load()
    {
        GameObject go = new GameObject();
        go.transform.name = "loadedObject";
        go.transform.localScale = new Vector3(0.0000005f, 0.0000005f, 0.0000005f);
        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(target);

        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        filter.mesh = holderMesh;
        go.AddComponent<MeshCollider>();
        renderer.material = mat;
        go.AddComponent<MeshStudy>();

    }
}

