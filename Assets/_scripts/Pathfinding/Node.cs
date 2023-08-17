using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] public List<Node> neighboors;

    [Header("Debug Properties")]
    [SerializeField] public bool showDirection;
    [SerializeField] public float rayLength;

    public Vector3 position => transform.position;
    public Transform parent => transform.parent;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        foreach (Node node in neighboors)
        {
            Gizmos.color = Color.green;
            if (showDirection)
            {
                Gizmos.DrawRay(position, (node.position - position).normalized * rayLength);
            }
            else
            {
                Gizmos.DrawLine(position, node.position);
            }
        }
    }

    public void AddNeighboor(Node node)
    {
        if (neighboors.Contains(node)) return;
        neighboors.Add(node);
    }

    public void DeleteNeighboor(string hash)
    {
        int removeIndex = neighboors.FindIndex(n => n.name == hash);

        if (removeIndex != -1)
        {
            neighboors.RemoveAt(removeIndex);
        }
    }
}
