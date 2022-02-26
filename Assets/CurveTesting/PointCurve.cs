using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Point
{
    public Vector2 point = Vector2.zero;
    public Vector2 dir = Vector2.zero;
}

public class PointCurve : MonoBehaviour
{
    public Point start;
    public Point goal;
    public Point lookDir;
    public float angle;
    public float stepSize;

    public List<Point> curvePoints = new List<Point>();

    bool drawCurve = false;

    // Start is called before the first frame update
    void Start()
    {
        start = new Point();
        goal = new Point();
        lookDir = new Point();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.S))
        {
            SetPoint("s", Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.G))
        {
            SetPoint("g", Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.D))
        {
            SetPoint("d", Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            drawCurve = true;
        }
    }

    public void SetPoint(string p, Vector2 newPoint)
    {
        if (p.Equals("s"))
            start.point = newPoint;
        else if (p.Equals("g"))
            goal.point = newPoint;
        else
            lookDir.point = newPoint;

        start.dir = (lookDir.point - start.point).normalized;

        angle = Vector2.Angle(start.dir, goal.point);
        curvePoints.Clear();
    }


    private void CreatePoints()
    {
        int count = 0;
        while (!curvePoints.Any(a => Vector2.Distance(a.point, goal.point) < 1f) && count < 100)
        {
            Point lastPoint = curvePoints.Count > 0 ? curvePoints.Last() : start;
            Point point = new Point();
            point.point = lastPoint.point + lastPoint.dir;


            if (Mathf.Abs(angle) < 10)
                point.dir = (goal.point - point.point).normalized;//Quaternion.Euler(0f, 0f, angle) * lastPoint.dir;
            else
            {
                point.dir = Quaternion.Euler(0f, 0f, angle) * lastPoint.dir;
                angle = Vector2.Angle(lastPoint.dir, goal.point);
            }


            Debug.Log($"Point Nr.{count} ({point.point.x}|{point.point.y}) Dir: ({point.dir.x}|{point.dir.y}) Angle: {angle}");
            curvePoints.Add(point);
            count++;
        }
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (start != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(start.point, 0.5f);


                //for(int i = 180; i >= -180; i -= 18)
                //{
                //    Gizmos.DrawRay(start.point, Quaternion.Euler(0, 0, i) * start.point);
                //}



            }
            if (goal != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(goal.point, 0.5f);
            }

            if (lookDir != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(lookDir.point, new Vector3(0.5f, 0.5f, 0.5f));

                start.dir = (lookDir.point - start.point).normalized;

                Gizmos.DrawRay(start.point, start.dir);

            }

            if (drawCurve)
            {
                if (curvePoints.Count > 0)
                {
                    Gizmos.color = Color.grey;
                    foreach (Point c in curvePoints)
                    {
                        Gizmos.DrawSphere(c.point, 0.1f);
                        Gizmos.DrawRay(c.point, c.dir);
                    }
                }
            }
        }
    }


}
