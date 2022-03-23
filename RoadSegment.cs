using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RoadSegment : MonoBehaviour
{
    [SerializeField] Transform[] controlPoints = new Transform[4];

    Vector3 GetPos(int i) => controlPoints[i].position;


}
