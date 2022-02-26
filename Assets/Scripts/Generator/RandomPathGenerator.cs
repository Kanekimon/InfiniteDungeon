using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public class Node
{
    public Node pre;

    public Vector2 nodePos;
    public bool isStart;
    public float gCost;
    public float fCost;
    public float hCost;


    public Node(Vector2 nodePos, float gCost, float fCost, float hCost)
    {
        this.nodePos = nodePos;
        this.gCost = gCost;
        this.fCost = fCost;
        this.hCost = hCost;
    }

}

public static class RandomPathGenerator
{

    static int[] maskX = { 0, 1, 0, -1 };
    static int[] maskY = { -1, 0, 1, 0 };






    public static List<Vector2> GenerateRandomPath(Room r, Vector2 startPoint)
    {
        Profiler.BeginSample("My Sample");
        Debug.Log("This code is being profiled");

        Boundary b = r.GetBoundary();

        Vector2 nDoor = new Vector2(b.startX + (r.xLength / 2), b.endY - 1);
        Vector2 sDoor = new Vector2(b.startX + (r.xLength / 2), b.startY + 1);
        Vector2 eDoor = new Vector2(b.endX - 1, r.center.y);
        Vector2 wDoor = new Vector2(b.startX + 1, r.center.y);

        List<Vector2> doors = new List<Vector2>();

        if (nDoor != startPoint)
            doors.Add(nDoor);
        if (sDoor != startPoint)
            doors.Add(sDoor);
        if (eDoor != startPoint)
            doors.Add(eDoor);
        if (wDoor != startPoint)
            doors.Add(wDoor);


        int rand = UnityEngine.Random.Range(0, doors.Count);

        Vector2 endPoint = doors[rand];

        int g = 0;
        Node start = new Node(startPoint, g, 0, 0);
        start.isStart = true;

        Node end = new Node(endPoint, float.MaxValue, float.MaxValue, float.MaxValue);



        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        bool[,] visited = new bool[r.xLength, r.yLength];


        openNodes.Add(start);

        bool run = true;


        while (run && openNodes.Count > 0)
        {
            Node current = GetRandom(openNodes); //openNodes.OrderBy(a => a.fCost).First(); ////GetAverage(openNodes);

            closedNodes.Add(current);
            visited[(int)current.nodePos.x - b.startX, (int)current.nodePos.y - b.startY] = true;

            openNodes.Remove(current);

            if (current.nodePos == end.nodePos)
            {
                end = closedNodes.Last();
                run = false;
            }
            else
            {

                List<Vector2> adjNodePos = AdjacentNodes(current, r);
                g++;


                foreach (Vector2 adj in adjNodePos)
                {

                    if (visited[(int)adj.x - b.startX, (int)adj.y - b.startY]) //closedNodes.Any(a => a.nodePos == adj))
                        continue;

                    if (!openNodes.Any(a => a.nodePos == adj))
                    {
                        float h = Vector2.Distance(adj, end.nodePos);
                        float f = h + g;
                        Node n = new Node(adj, g, f, h);
                        n.pre = current;
                        openNodes.Add(n);
                    }
                    else
                    {
                        Node n = openNodes.Where(a => a.nodePos == adj).FirstOrDefault();
                        if (g + n.hCost < n.fCost)
                        {
                            n.gCost = g;
                            n.fCost = n.gCost + n.hCost;
                            n.pre = current;
                        }
                    }
                }
            }

        }

        List<Vector2> path = BackTrackPath(end);
        Profiler.EndSample();
        return BackTrackPath(end);
    }

    private static Node GetAverage(List<Node> op)
    {
        float sum = op.Sum(a => a.fCost) / op.Count;

        return op.OrderBy(a => Mathf.Abs(a.fCost - sum)).First();
    }

    public static Node GetRandom(List<Node> op)
    {
        return op[UnityEngine.Random.Range(0, op.Count)];
    }


    private static List<Vector2> AdjacentNodes(Node current, Room r)
    {

        Boundary b = r.GetBoundary();

        List<Vector2> adjNodePos = new List<Vector2>();

        for (int i = 0; i < 4; i++)
        {
            int x = (int)current.nodePos.x + maskX[i];
            int y = (int)current.nodePos.y + maskY[i];



            if (!b.IsInsideWalls(new Vector2(x, y)))
                continue;

            adjNodePos.Add(new Vector2(x, y));

        }

        return adjNodePos;
    }



    private static List<Vector2> DrawAllClosed(List<Node> clo)
    {
        List<Vector2> path = new List<Vector2>();

        foreach (Node node in clo)
        {
            path.Add(node.nodePos);
        }
        return path;
    }


    private static List<Vector2> BackTrackPath(Node n)
    {
        List<Vector2> path = new List<Vector2>();
        path.Add(n.nodePos);

        bool start = false;

        List<Node> nodes = new List<Node>();

        nodes.Add(n);

        while (!start)
        {
            Node last = nodes.Last();
            if (last == null || last.isStart)
                start = true;
            else
                nodes.Add(nodes.Last().pre);

        }

        foreach (Node node in nodes)
        {
            path.Add(node.nodePos);
        }

        return path;
    }

}
