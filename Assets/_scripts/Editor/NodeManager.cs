using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeManager : EditorWindow
{

    private GameObject selectedGameObject = default;
    public GameObject nodePrefab;
    public Node[] allSelectedNodes;

    [MenuItem("Custom Script/Node Manager")]
    public static void ShowWindow()
    {
        GetWindow<NodeManager>("My Editor Window");
    }

    void OnEnable()
    {
        selectedGameObject = Selection.activeGameObject;
        EditorApplication.update += CheckSelection;
    }

    void OnDisable()
    {
        EditorApplication.update -= CheckSelection;
    }

    void CheckSelection()
    {

        List<Node> nodes = new List<Node>();

        if (Selection.activeGameObject != null)
        {
            Node n = Selection.activeGameObject.GetComponent<Node>();
            if (n != null)
            {
                selectedGameObject = Selection.activeGameObject;
            }
            else
            {
                selectedGameObject = null;
            }
        }

        foreach (GameObject gameObject in Selection.gameObjects)
        {
            Node node = GetNodeFromGameObject(gameObject);

            if (node != null && node.name != selectedGameObject.name)
            {
                nodes.Add(node);
            }
        }

        if (nodes != null) allSelectedNodes = nodes.ToArray();

        Repaint();
    }

    Node GetNodeFromGameObject(GameObject gameObject)
    {
        return gameObject.GetComponent<Node>();
    }

    void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        obj.Update();
        EditorGUILayout.PropertyField(obj.FindProperty("nodePrefab"));

        if (nodePrefab == null)
        {
            EditorGUILayout.HelpBox("Set the Node Prefab", MessageType.Error);
            return;
        }

        if (selectedGameObject == null)
        {
            EditorGUILayout.HelpBox("Select a Node type to start", MessageType.Warning);
            // return;
        }

        EditorGUILayout.BeginVertical();
        DrawButtons();
        EditorGUILayout.EndVertical();

        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Node"))
        {
            CreateNode();
        }

        if (GUILayout.Button("Create Bi-Directional Node"))
        {
            CreateNode(true);
        }

        if (GUILayout.Button("Delete Node"))
        {
            DeleteNode();
        }

        if (GUILayout.Button("Connect Selection"))
        {
            ConnectTo();
        }

        if (GUILayout.Button("Connect Selection Bi-Directional"))
        {
            ConnectTo(true);
        }

    }

    void ConnectTo(bool bidirectional = false)
    {
        if (allSelectedNodes.Length == 0) return;

        foreach (Node node in allSelectedNodes)
        {
            GetNodeFromGameObject(selectedGameObject).AddNeighboor(node);
            if (bidirectional) node.AddNeighboor(GetNodeFromGameObject(selectedGameObject));
        }

        Repaint();
    }

    void CreateNode(bool bidirectional = false)
    {
        Node selectedNode = GetNodeFromGameObject(selectedGameObject);
        GameObject newGameObject = GameObject.Instantiate(nodePrefab, selectedNode.position + Vector3.forward, Quaternion.identity, selectedNode.parent);
        Node newNode = newGameObject.GetComponent<Node>();
        newNode.name = newNode.GetHashCode().ToString();

        selectedNode.AddNeighboor(newNode);
        if (bidirectional) newNode.AddNeighboor(selectedNode);

        Selection.SetActiveObjectWithContext(newGameObject, newGameObject);
    }

    void DeleteNode()
    {
        Node selectedNode = GetNodeFromGameObject(selectedGameObject);

        foreach (Transform transform in selectedNode.parent.transform)
        {
            Node childNode = transform.GetComponent<Node>();

            if (childNode != null)
            {
                childNode.DeleteNeighboor(selectedNode.name);
            }
        };

        GameObject.DestroyImmediate(selectedNode.gameObject);
    }
}

