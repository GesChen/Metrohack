using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayTransition : MonoBehaviour
{
	public bool learning;
	public Transform board;
	public Transform targetPos;
	Vector3 boardOriginalPos;

	public float smoothness;

	public Cloud[] toMove;
	public Vector3 offsetAmount;
	Vector3[] originalPositions;

	Vector3 smoothedOffset;

	void Start()
	{
		originalPositions = new Vector3[toMove.Length];
		for (int i = 0; i < toMove.Length; i++)
		{
			originalPositions[i] = toMove[i].transform.position;
		}

		boardOriginalPos = board.position;
	}

	void Update()
	{
		if (learning)
		{
			smoothedOffset = Vector3.Lerp(smoothedOffset, offsetAmount, smoothness);
			for (int i = 0;i < toMove.Length; i++)
				toMove[i].startingPos = originalPositions[i] + smoothedOffset;

			board.position = Vector3.Lerp(board.position, targetPos.position, smoothness);
		}
		else
		{
			smoothedOffset = Vector3.Lerp(smoothedOffset, Vector3.zero, smoothness);
			for (int i = 0; i < toMove.Length; i++)
				toMove[i].startingPos = originalPositions[i] + smoothedOffset;

			board.position = Vector3.Lerp(board.position, boardOriginalPos, smoothness);
		}
	}
	public void Learn()
	{
		learning = true;
	}
	public void Return()
	{
		learning = false;
	}
}
