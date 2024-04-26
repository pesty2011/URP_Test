using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Jump
{
	int numberOfPoints = 50;
	Vector3 end = new Vector3(0, 20, 0);
	Vector3 start = new Vector3(50, 0, 0);
	float height = 10.0f;
	Vector3 controlPoint1;
	Vector3 controlPoint2;

	int segments = 50;
	float jumpHeight = 100.0f;


	public void DrawCatmullRomSpline()
	{
		Vector3[] points = new Vector3[numberOfPoints];

		Vector3 midPoint = (start + end) * 0.5f;
		midPoint.y += jumpHeight;

		controlPoint1 = CalculateControlPoint(start, midPoint, 0.25f);
		controlPoint2 = CalculateControlPoint(midPoint, end, 0.75f); 
		controlPoint1.y += jumpHeight;
		controlPoint2.y += jumpHeight;



		for (int i = 0; i < numberOfPoints; i++)
		{
			float t = (float)i / (numberOfPoints - 1);
			points[i] = CalculateCatmullRomPoint(t, start, end, controlPoint1, controlPoint2);
		}


		// Draw the curve
		for (int i = 0; i < numberOfPoints - 1; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
	}


	private Vector3 CalculateCatmullRomPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float c0 = -0.5f * t3 + t2 - 0.5f * t;
		float c1 = 1.5f * t3 - 2.5f * t2 + 1.0f;
		float c2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
		float c3 = 0.5f * t3 - 0.5f * t2;

		Vector3 point = c0 * p0 + c1 * p1 + c2 * p2 + c3 * p3;
		return point;
	}


	Vector3 CalculateControlPoint(Vector3 startPoint, Vector3 endPoint, float percentage)
	{
		// Calculate a point between start and end based on the percentage
		Vector3 controlPoint = startPoint + (endPoint - startPoint) * percentage;
		controlPoint.y += 1.0f; // Adjust the control point's height as needed
		return controlPoint;
	}




	public void DrawJumpPath()
	{
		Vector3 p0 = start;
		Vector3 p1 = end;

		// Calculate control point
		Vector3 controlPoint = CalculateControlPoint(p0, p1);

		// Draw the path
		Gizmos.color = Color.black;
		for (int i = 0; i <= segments; i++)
		{
			float t = (float)i / segments;
			Vector3 point = CalculatePoint(p0, controlPoint, p1, t);
			Gizmos.DrawSphere(point, 0.2f);
		}
	}

	Vector3 CalculatePoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		float u = 1 - t;
		return u * u * p0 + 2 * u * t * p1 + t * t * p2;
	}

	Vector3 CalculateControlPoint(Vector3 p0, Vector3 p1)
	{
#if false
		// Calculate midpoint between p0 and p1
		Vector3 midpoint = (p0 + p1) / 2;
		// Raise the midpoint to create the jump peak
		midpoint.y += height;
		return midpoint;
#else
		// Calculate a point that's vertically offset from the midpoint
		Vector3 midpoint = (p0 + p1) / 2;
		Vector3 controlPoint = midpoint + Vector3.up * jumpHeight * 0.5f;

		// Calculate the distance between the start and end points
		float distance = Vector3.Distance(p0, p1);

		// Calculate the adjustment factor based on the distance
		float adjustmentFactor = 0.25f * (distance / 10f); // Adjust this factor as needed

		// Adjust control point's position for faster initial start and quicker fall
		controlPoint += ((p0 - p1).normalized * adjustmentFactor);

		return controlPoint;
#endif
	}


	public void DrawCurve()
	{
		
		Vector3 p0 = GameObject.Find("StartPoint").transform.position;
		Vector3 p1 = GameObject.Find("MidPoint").transform.position;
		Vector3 p2 = GameObject.Find("EndPoint").transform.position;

		Gizmos.color = Color.red;
		Vector3 prevPoint = p0;
		for (int i = 1; i <= segments; i++)
		{
			float t = i / (float)segments;
			Vector3 point = CalculatePoint(p0, p1, p2, t);
			Gizmos.DrawLine(prevPoint, point);
			prevPoint = point;
		}
	}

	public void DrawCubicBezierCurve()
	{
		Vector3 p0 = GameObject.Find("StartPoint").transform.position;
		Vector3 p1 = GameObject.Find("MidPoint").transform.position;
		Vector3 p2 = GameObject.Find("EndPoint").transform.position;


		Vector3[] curvePoints = CalculateCubicBezierCurve(p0, p1, p2, segments);

		Gizmos.color = Color.red;
		for (int i = 1; i < curvePoints.Length; i++)
		{
			Gizmos.DrawLine(curvePoints[i - 1], curvePoints[i]);
		}

	}

	Vector3[] CalculateCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, int segments)
	{
		Vector3[] points = new Vector3[segments + 1];

		for (int i = 0; i <= segments; i++)
		{
			float t = i / (float)segments;
			points[i] = CalculateCubicBezierPoint(p0, p1, p2, t);
		}

		return points;
	}


	Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;

		Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;

		return p;
	}
}
