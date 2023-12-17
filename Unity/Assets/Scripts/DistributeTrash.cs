using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributeTrash : MonoBehaviour
{
    public Transform[] toDistributeOn;
    public int amount;
    public GameObject[] objects;
    public Vector3 positionOffset;

    public void Distribute()
    {
		for (int i = 0; i < amount; i++)
		{
			Transform randomSurface = toDistributeOn[Random.Range(0, toDistributeOn.Length)];
			Mesh mesh = randomSurface.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] verts = mesh.vertices;
            int[] tris = mesh.triangles;

			// Randomly select a triangle
			int randomTriangleIndex = Random.Range(0, tris.Length / 3);
			int triangleStartIndex = randomTriangleIndex * 3;

			// Get the vertices of the selected triangle
			Vector3 vertex1 = verts[tris[triangleStartIndex]];
			Vector3 vertex2 = verts[tris[triangleStartIndex + 1]];
			Vector3 vertex3 = verts[tris[triangleStartIndex + 2]];

			// Generate random barycentric coordinates
			float rand1 = Random.Range(0f, 1f);
			float rand2 = Random.Range(0f, 1f - rand1);

			// Calculate the position of the random point on the triangle
			Vector3 randomPointOnTriangle = vertex1 + rand1 * (vertex2 - vertex1) + rand2 * (vertex3 - vertex1);

			// Create a Quaternion based on random Euler angles
			Quaternion randomRotation = Random.rotation;

			GameObject randomObject = objects[Random.Range(0, objects.Length)];

			Trash trash = Instantiate(randomObject, randomPointOnTriangle + positionOffset, randomRotation, transform).GetComponent<Trash>();

			trash.id = i;

			Debug.Log(randomPointOnTriangle);
        }
    }
}
