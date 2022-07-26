using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    [SerializeField] private GameObject meshGenPrefab;
    [SerializeField] private float minNewVertexDistance;

    private Camera Cam;
    private Vector2 _lastPos;
    private MeshGenerator _lastMesh;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //on mouse button down
            _lastMesh = Instantiate(meshGenPrefab).GetComponent<MeshGenerator>();
        }
        if (Input.GetMouseButton(0))
        {
            //on mouse button
            Raycast();
        }
        if (Input.GetMouseButtonUp(0))
        {
            //on mouse button up
            // _lastMesh.OnEndPath();
        }

    }
    public void Raycast()
    {
        var ray = Cam.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit)) return;
        if (!hit.collider.CompareTag("MeshTrigger") && !hit.collider.CompareTag("GeneratedMesh"))
        {
            //_lastMesh.OnEndPath();
            return;
        }

        var hitPoint = new Vector2(hit.point.x, hit.point.z);
        if (Vector2.Distance(_lastPos, hitPoint) < minNewVertexDistance) return;

        _lastPos = hitPoint;
        //_lastMesh.OnAddPoint(hitPoint);
       
    }
}
