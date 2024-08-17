using System.Collections.Generic;
using System.Linq;
using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class BlocksManager : Singleton<BlocksManager>, IRestart
    {
        private readonly List<Block> _blocks = new List<Block>();

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }
        
        public void RemoveBlock(Block block)
        {
            Destroy(block.gameObject);
            _blocks.Remove(block);
        }

        public float GetBlocksHeight()
        {
            float maxHeight = float.MinValue;

            foreach (var block in _blocks)
            {
                Vector3 highestVertex = FindHighestVertexGlobal(block.gameObject);
                if (highestVertex.y > maxHeight)
                {
                    maxHeight = highestVertex.y;
                }
            }

            return maxHeight;
        }

        public void Restart()
        {
            foreach (var block in _blocks)
            {
                Destroy(block.gameObject);
            }
            _blocks.Clear();
        }

        public bool IsAnyBlockMoving()
        {
            foreach (var block in _blocks)
            {
                if (block.Velocity.magnitude > 0.1f)
                {
                    Debug.Log("Block is moving with velocity " + block.Velocity.magnitude);
                    return true;
                }
            }
            
            return false;
        }
        
        public static Vector3 FindHighestVertexGlobal(GameObject obj)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (!meshFilter)
            {
                return Vector3.zero;
            }

            Mesh mesh = meshFilter.mesh;

            Vector3 highestVertex = obj.transform.TransformPoint(mesh.vertices[0]);

            foreach (Vector3 vertex in mesh.vertices)
            {
                Vector3 worldVertex = obj.transform.TransformPoint(vertex);

                if (worldVertex.y > highestVertex.y)
                {
                    highestVertex = worldVertex;
                }
            }

            return highestVertex;
        }
    }
}