using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class RoadSegment : MonoBehaviour
{

    [SerializeField] Mesh2d shape2D;

    [Range(0,1)]
    [SerializeField] float tTest = 0;
    [SerializeField] Transform[] controlPoints = new Transform[4];

    Vector3 GetPos(int i) => controlPoints[i].position;

    public void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPos(i), 0.05f);
        }


        Handles.DrawBezier(
            GetPos(0),
            GetPos(3),
            GetPos(1),
            GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);

        Gizmos.color = Color.red;

        OrientedPoint testPoint = GetBezierPoint(tTest);
        Handles.PositionHandle(testPoint.pos, testPoint.rot);

        float radius = 0.1f;


        void DrawPoint( Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorld( localPos), radius);

        //this uses Linq 
        Vector3[] verts = shape2D.vertices.Select(v => testPoint.LocalToWorld(v.point)).ToArray();

        for (int i = 0; i < shape2D.lineIndices.Length; i+=2) {

            Vector3 a = verts[shape2D.lineIndices[i]];
            Vector3 b = verts[shape2D.lineIndices[i+1]];

            Gizmos.DrawLine(a, b);


            DrawPoint(shape2D.vertices[i].point);

                }

        //DrawPoint(Vector3.zero);
        //DrawPoint(Vector3.right * 0.2f);
        //DrawPoint(Vector3.right * 0.1f);
      //  DrawPoint(Vector3.right * -0.1f);
    //    DrawPoint(Vector3.right * -0.2f);
  //      DrawPoint(Vector3.up * 0.1f);
//        DrawPoint(Vector3.up * 0.2f);

        Gizmos.color = Color.white;

        
    }


    OrientedPoint GetBezierPoint(float t) {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p0, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new OrientedPoint(pos, tangent);


    }



}
