using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TVS : MonoBehaviour
{
    //bool[] visited = new bool[1000];
    public InputField maximumweight;
    GameObject[] nodes_list;
    List<int> splited;
    public int[] D = new int[100];
    public InputField totalNodes;
    public int yOfset = 80;
    public InputField condition;
    //public int offset = 100;
    public GameObject objLineR;
    public GameObject NODE;
    LineRenderer lr;

    public struct Edge
    {
        public int nodeNumber;
        public int child;
        public int wght;
        public float xPos;
        public float yPos;
        public bool isLeft;
    };
    public List<Edge> points;
    public List<Edge>[] tree;
    public bool[] unq;
    public List<int> nodes;
    public Queue<int> queue;
    public Edge temp;
    void Start()
    {
        tree = new List<Edge>[100];
        for (int i = 0; i < 100; i++)
        {
            tree[i] = new List<Edge>();
        }
        unq = new bool[200];
        nodes = new List<int>();
        queue = new Queue<int>();
        temp = new Edge();
        lr = objLineR.GetComponent<LineRenderer>();
        points = new List<Edge>();
        nodes_list = new GameObject[100];
        splited = new List<int>();
    }
    void drawTree()
    {
        lr.positionCount = (points.Count);
        lr.startColor = Color.black;
        lr.endColor = Color.black;
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, new Vector3(points[i].xPos-.1f, points[i].yPos, 0.1f));


            if (!unq[points[i].nodeNumber])
            {
                GameObject obj = GameObject.Instantiate(NODE, new Vector3(points[i].xPos, points[i].yPos, 0), Quaternion.identity);
                Text[] nodeText = new Text[3];
                nodeText = obj.GetComponentsInChildren<Text>();
                if(points[i].nodeNumber!=1&& points[i].nodeNumber != 0)
                {
                    if (points[i].isLeft)
                    {
                        nodeText[2].text = points[i].wght.ToString();
                    

                    }
                    else
                    {
                        nodeText[1].text = points[i].wght.ToString();

                    }
                }
              
                nodeText[0].text = points[i].nodeNumber.ToString();
                unq[points[i].nodeNumber] = true;

                nodes_list[points[i].nodeNumber] = obj;



            }

        }

        lr.startWidth = 1f;
        lr.endWidth = 1f;


        // lr.endColor = Color.red;

    }
    void dfsAddPoints(int current, int parrent, float xParent, float yParent, Edge prevRoot, int offset)
    {
        //if (visited[current])
        //{
        //    return;
        //}
        if (current == 1)
        {
            Edge root = new Edge();
            root.xPos = 0;
            root.yPos = 0;
            points.Add(root);

        }

        //visited[current] = true;
        for (int i = 0; i < tree[current].Count; i++)
        {

            if (tree[current][i].isLeft)
            {
                Edge tempLeft = new Edge();
                tempLeft.child = tree[current][i].child;
                tempLeft.isLeft = tree[current][i].isLeft;
                tempLeft.yPos = yParent - yOfset;
                tempLeft.nodeNumber = tree[current][i].nodeNumber;
                tempLeft.wght = tree[current][i].wght;
                tempLeft.xPos = xParent - offset;
                offset -= 20;

                tree[current][i] = tempLeft;
                points.Add(tree[current][i]);
            }
            else
            {
                Edge tempRight = new Edge();
                tempRight.child = tree[current][i].child;
                tempRight.isLeft = tree[current][i].isLeft;
                tempRight.yPos = yParent - yOfset;

                tempRight.nodeNumber = tree[current][i].nodeNumber;
                tempRight.wght = tree[current][i].wght;

                tempRight.xPos = xParent + offset;
                offset -= 20;


                tree[current][i] = tempRight;
                points.Add(tree[current][i]);
            }


            dfsAddPoints(tree[current][i].child, current, tree[current][i].xPos, tree[current][i].yPos, tree[current][i], offset);

            points.Add(prevRoot);


        }

    }

    public void generateTree()
    {
       
        //refreshAll();
        Edge rootNode = new Edge();
        rootNode.nodeNumber = 1;
        rootNode.child = 2;
        rootNode.wght = 0;
        rootNode.wght = 0;
        rootNode.yPos = 0;

        int n = int.Parse(totalNodes.text);
        //(int)Random.Range(5, 15);
        int weightMaximum = int.Parse(maximumweight.text) ;
        for (int i = 2; i <= n; i++)
        {
            queue.Enqueue(i);
        }
        for (int i = 1; i <= n; i++)
        {

            if (queue.Count > 0)
            {
                int left, right;

                left = queue.Dequeue();
                int weight = Random.Range(1, weightMaximum);
                temp.child = left;
                temp.wght = weight;
                temp.isLeft = true;
                temp.nodeNumber = left;
                tree[i].Add(temp);

                if (queue.Count <= 0)
                {
                    continue;
                }
                right = queue.Peek();
                queue.Dequeue();


                weight = Random.Range(1, weightMaximum);
                temp.child = right;
                temp.wght = weight;
                temp.isLeft = false;
                temp.nodeNumber = right;
                tree[i].Add(temp);

            }
        }
        Debug.Log(tree);

        dfsAddPoints(1, 1, 0, 0, rootNode, 100);

        drawTree();

        //for (int i = 0; i < nodes_list.Count; i++)
        //{
        //    if (i % 2 == 0)
        //    {
        //        Image btn = nodes_list[i].GetComponentInChildren<Image>();
        //        btn.color = Color.red;
        //    }
        //}
    }

    public void runTVSAlgo()
    {
        int root = 1;
        int delta = int.Parse(condition.text);
        Edge rootNode = new Edge();
        rootNode.nodeNumber = 1;
        rootNode.child = 2;
        rootNode.wght = 0;
        rootNode.wght = 0;
        rootNode.yPos = 0;
        for (int i = 0; i < 100; i++)
        {
            if (splited.Contains(i))
            {
                Image btn = nodes_list[i].GetComponentInChildren<Image>();
                btn.color = Color.white;
            }

        }
        splited.Clear();
        orgTVS(rootNode, delta, 0);
        for (int i = 0; i < 100; i++)
        {
            if (splited.Contains(i))
            {
                Image btn = nodes_list[i].GetComponentInChildren<Image>();
                btn.color = Color.red;
            }
          
        }
    }
    public void orgTVS(Edge rootNode, int delta, int parent)
    {
        D[rootNode.nodeNumber] = 0;
        for (int i = 0; i < tree[rootNode.nodeNumber].Count; i++)
        {
            orgTVS(tree[rootNode.nodeNumber][i], delta, rootNode.wght);
            D[rootNode.nodeNumber] = Mathf.Max(D[rootNode.nodeNumber], D[tree[rootNode.nodeNumber][i].child] + tree[rootNode.nodeNumber][i].wght);
        }
        if ((D[rootNode.nodeNumber] + rootNode.wght) > delta)
        {
            splited.Add(rootNode.nodeNumber);
            D[rootNode.nodeNumber] = 0;
        }
    }
    public void refreshAll()
    {
        SceneManager.LoadScene(0);
    }
}
