using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExampleUseof_MeshCut : MonoBehaviour
{
    MeshStudy mesh;
    public List<GameObject> NewEPList = new List<GameObject>();

    public Material capMaterial;

    // Use this for initialization
    void Start()
    {



    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {

                GameObject victim = hit.collider.gameObject;
                NewEPList = victim.GetComponent<MeshStudy>().EPList;
                Destroy(victim.GetComponent<MeshCollider>());
                Destroy(victim.GetComponent<MeshStudy>());
                Destroy(victim.GetComponent<isSelected>());
                Destroy(victim.GetComponent<Activator>());

                //Destroy(victim.GetComponent<ToggleVisibility>());

                for (int i = 0; i < NewEPList.Count; i++)
                {
                    Destroy(NewEPList[i]);
                }

                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);


                //if (!pieces[1].GetComponent<Rigidbody>())
                //	pieces[1].AddComponent<Rigidbody>();
                //pieces[1].AddComponent<MeshStudy>().ReDraw();

                //Destroy(pieces[1], 1);
            }
        }
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
        Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

        Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * 0.5f);

    }

}
