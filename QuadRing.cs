using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will give a warning when trying to delte mesh filter while object is active
[RequireComponent(typeof(MeshFilter))] //create a mesh filter attached to this.
public class QuadRing : MonoBehaviour
{

    public enum UvProjection
    {
        AngularRadial,
        ProjectZ
    }

    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;
    
    [Range(0.01f, 2)]
    [SerializeField] float thickness;
    [Range(3, 300)]
    [SerializeField] int angularSegments = 3;

    [SerializeField] UvProjection uvProjection = UvProjection.AngularRadial;

    Mesh mesh;

    float RadiusOuter => radiusInner + thickness;
    int VertexCount => angularSegments * 2;

    

    private void OnDrawGizmosSelected()
    {
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, radiusInner, angularSegments );
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, RadiusOuter, angularSegments);
        //Gizmos.DrawWireSphere(transform.position, radiusInner);
        //Gizmos.DrawWireSphere(transform.position, RadiusOuter);


    }

    private void Awake() //make sure the mesh exists
    {
        mesh = new Mesh();
        mesh.name = "QuadRing";

        GetComponent<MeshFilter>().sharedMesh = mesh; //this is a reference to the mesh to it will automatically update 
                                                      //changes to the mesh filter

    }

   void Update() => GenerateMesh(); //This will call generate mesh every single frame
   
    void GenerateMesh() //create mesh variables
    {
        mesh.Clear(); //Have to clear the mish data incase the shape changed

        int vCount = VertexCount;
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < angularSegments + 1; i++)
        {

            float t = i / (float)angularSegments;
            float angRad = t * Mathfs.TAU;
            Vector2 dir = Mathfs.GetUnitVectorByAngle(angRad);

            vertices.Add(dir * RadiusOuter); //this addes inner and outer 
            vertices.Add(dir * radiusInner);//  vertices to the list of vertices
            normals.Add(Vector3.forward); // forward is alias for new Vector3(0,0,1)
            normals.Add(Vector3.forward); // Double Normals because there are two vertices


            switch (uvProjection)
            {

                case UvProjection.AngularRadial:
                    //angular / radial UV cooridnates
                    uvs.Add(new Vector2(t, 1));
                    uvs.Add(new Vector2(t, 0));
                    break;

                case UvProjection.ProjectZ:
                    //Top-down projection

                    uvs.Add(dir * 0.5f + Vector2.one * 0.5f);
                    uvs.Add(dir * (radiusInner / RadiusOuter) * 0.5f + Vector2.one * 0.5f);
                    break;
            }
        }

        //Connect Triangles
        List<int> triangleIndices = new List<int>();
        for (int i = 0; i < angularSegments; i++)
        {
            int indexRoot = i * 2;
            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = (indexRoot + 2) ; //modulo avoids index out of range issues
            int indexInnerNext = (indexRoot + 3) ;

            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexOuterNext);
            triangleIndices.Add(indexInnerNext);

            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexInnerNext);
            triangleIndices.Add(indexInnerRoot);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangleIndices, 0); //0 is the submesh index 
        //When there are multiple materials those materials are assinged
        //to multiple submeshes
        //mesh.RecalculateNormals(); //automatic normal recalculation
        mesh.SetNormals(normals);
        mesh.SetUVs( 0, uvs);

        








    }




}
