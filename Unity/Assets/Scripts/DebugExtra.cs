using UnityEngine;

public class DebugExtra : MonoBehaviour
{
	public static void DrawEmpty(Vector3 pos, float size, Color color)
	{
		Debug.DrawLine(pos - size * Vector3.up, pos + size * Vector3.up, color);
		Debug.DrawLine(pos - size * Vector3.right, pos + size * Vector3.right, color);
		Debug.DrawLine(pos - size * Vector3.forward, pos + size * Vector3.forward, color);
	}
	public static void DrawEmpty(Vector3 pos, float size)
	{
		DrawEmpty(pos, size, Color.white);
	}

	public static void DrawSphere(Vector3 pos, float radius, int resolution, Color color)
	{
		float sin0 = Mathf.Sin(0);
		float cos0 = Mathf.Cos(0);
		Vector3 lastX = new Vector3(0, sin0, cos0) * radius + pos;
		Vector3 lastY =	new Vector3(sin0, 0, cos0) * radius + pos;
		Vector3 lastZ = new Vector3(sin0, cos0, 0) * radius + pos;
		for (int i = 0; i < resolution + 1; i++)
		{
			float j = i / (float) resolution * 2 * Mathf.PI;
			float sin = Mathf.Sin(j);
			float cos = Mathf.Cos(j);
			Vector3 xPoint = new Vector3(0, sin, cos) * radius + pos;
			Vector3 yPoint = new Vector3(sin, 0, cos) * radius + pos;
			Vector3 zPoint = new Vector3(sin, cos, 0) * radius + pos;

			Debug.DrawLine(lastX, xPoint, color);
			Debug.DrawLine(lastY, yPoint, color);
			Debug.DrawLine(lastZ, zPoint, color);

			lastX = xPoint;
			lastY = yPoint;
			lastZ = zPoint;
		}
	}
	public static void DrawSphere(Vector3 pos, float radius, int resolution)
	{
		DrawSphere(pos, radius, resolution, Color.white);
	}
	public static void DrawSphere(Vector3 pos, float radius, Color color)
	{
		DrawSphere(pos, radius, 40, color);
	}
	public static void DrawSphere(Vector3 pos, float radius)
	{
		DrawSphere(pos, radius, 40, Color.white);
	}

	public static void DrawPoint(Vector3 pos, float size, Color color)
	{
		Vector3 px = pos + size * Vector3.right;
		Vector3 nx = pos + size * Vector3.left;
		Vector3 py = pos + size * Vector3.up;
		Vector3 ny = pos + size * Vector3.down;
		Vector3 pz = pos + size * Vector3.forward;
		Vector3 nz = pos + size * Vector3.back;

		Debug.DrawLine(px, py, color);
		Debug.DrawLine(px, ny, color);
		Debug.DrawLine(px, pz, color);
		Debug.DrawLine(px, nz, color);

		Debug.DrawLine(nx, py, color);
		Debug.DrawLine(nx, ny, color);
		Debug.DrawLine(nx, pz, color);
		Debug.DrawLine(nx, nz, color);
		
		Debug.DrawLine(px, nx, color);
		Debug.DrawLine(py, ny, color);
		Debug.DrawLine(pz, nz, color);

		Debug.DrawLine(py, pz, color);
		Debug.DrawLine(py, nz, color);
		Debug.DrawLine(ny, pz, color);
		Debug.DrawLine(ny, nz, color);
	}
	public static void DrawPoint(Vector3 pos, float size)
	{
		DrawPoint(pos, size, Color.white);
	}
	public static void DrawPoint(Vector3 pos, Color color)
	{
		DrawPoint(pos, .1f, color);
	}
	public static void DrawPoint(Vector3 pos)
	{
		DrawPoint(pos, .1f);
	}


	public static void DrawGrid(Vector3 pos, Vector3 normal, int gridSize, int cellSize)
	{
		// Calculate the right and forward vectors based on the normal
		Vector3 right = Vector3.Cross(normal, Vector3.up).normalized;
		Vector3 forward = Vector3.Cross(normal, right).normalized;

		// Calculate the size of the grid
		float gridSizeX = gridSize * cellSize;
		float gridSizeY = gridSize * cellSize;

		// Draw horizontal lines
		for (int i = 0; i <= gridSize; i++)
		{
			Vector3 start = pos + i * cellSize * forward - 0.5f * gridSizeX * forward;
			Vector3 end = start + gridSizeX * right;
			Debug.DrawLine(start, end, Color.white);
		}

		// Draw vertical lines
		for (int i = 0; i <= gridSize; i++)
		{
			Vector3 start = pos + i * cellSize * right - 0.5f * gridSizeY * right;
			Vector3 end = start + gridSizeY * forward;
			Debug.DrawLine(start, end, Color.white);
		}
	}

}
