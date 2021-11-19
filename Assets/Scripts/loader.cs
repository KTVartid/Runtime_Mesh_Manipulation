using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
//using UnityEditorInternal;

public class loader : MonoBehaviour
{
    public string target;
    Material mat;
    Material originalMat;
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
        go.transform.tag = "IO";
        go.layer = 10;
        go.transform.localScale = new Vector3(0.0000005f, 0.0000005f, 0.0000005f);
        Mesh holderMesh = new Mesh();
        ObjImporter newMesh = new ObjImporter();
        holderMesh = newMesh.ImportFile(target);

        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        filter.mesh = holderMesh;
        go.AddComponent<MeshCollider>();
        go.AddComponent<MeshStudy>();
        go.AddComponent<Activator>();
        renderer.material = mat;
        //renderer.material = originalMat;

    }
}

