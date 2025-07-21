using UnityEngine;

public class SubmeshDebugger : MonoBehaviour
{
    void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            var mesh = meshFilter.sharedMesh;
            Debug.Log("Submesh Count: " + mesh.subMeshCount);
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                Debug.Log($"Submesh {i} has {mesh.GetSubMesh(i).indexCount} indices.");
            }
        }
    }
}
