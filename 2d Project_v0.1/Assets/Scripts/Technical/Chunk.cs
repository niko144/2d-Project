using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chunks
{

    // Written by Lukas Sacher/Camo
    public class Chunk : MonoBehaviour
    {
		public const float CHUNK_SIZE = 12f;

		GameObject container = null;

		GameObject player = null;
		float loadingDist = 0f;

		private void Start()
		{
			player = ChunkMaster.current.player;
			loadingDist = ChunkMaster.current.loadingDistance;

			container = transform.GetChild(0).gameObject;
			if (!container.transform.name.Contains("Container"))
			{
				Debug.LogWarning("The content of a chunks has to be in a seperate container.");
			}
		}

		private void Update()
		{
			CheckForChunkLoad();
		}

		void CheckForChunkLoad()
		{
			container.SetActive(Vector2.Distance(transform.position, player.transform.position) <= loadingDist);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.white;

			Vector2[] points = new Vector2[4];

			float halfSize = CHUNK_SIZE * 0.5f;

			points[0] = new Vector2(transform.position.x - halfSize, transform.position.y + halfSize); 
			points[1] = new Vector2(transform.position.x + halfSize, transform.position.y + halfSize); 
			points[2] = new Vector2(transform.position.x + halfSize, transform.position.y - halfSize); 
			points[3] = new Vector2(transform.position.x - halfSize, transform.position.y - halfSize);

			for (int i = 0; i < points.Length; i++)
			{
				if(i != points.Length - 1)
				{
					Gizmos.DrawLine(points[i], points[i + 1]);
				}
				else
				{
					Gizmos.DrawLine(points[i], points[0]);
				}
			}
		}
	}
}
