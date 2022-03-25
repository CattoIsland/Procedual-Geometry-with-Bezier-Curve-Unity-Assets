using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu]
public class Mesh2d : ScriptableObject
{
    [System.Serializable]
    public class Vertex {
        public  Vector2 point;
        public Vector2 normal;
        public float u;  //uv but not :thinking_face:
        //bitangent
        //vertex color
        //multiple uv's
      
    }

    public Vertex[] vertices;
    public int[] lineIndices;


}

