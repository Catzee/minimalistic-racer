using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyData : ScriptableObject
{
    public Mesh modelMesh;
    public int health = 100;
    public int damage = 10;

}
