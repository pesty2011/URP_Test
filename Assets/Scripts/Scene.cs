using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{

	Vector3 start;
	Vector3 end;
	List<Vector3> jumpSpline;
	Jump jump = new Jump();

    // Start is called before the first frame update
    void Start()
    {
		start = GameObject.FindGameObjectWithTag("Start").transform.position;
		end = GameObject.FindGameObjectWithTag("End").transform.position;
		//float height = 100.0f;
		//int numPoints = 50;

		//jumpSpline = jump.simulateJump(start, end, height, numPoints);


	}

	// Update is called once per frame
	void OnDrawGizmos()
    {
		//		Gizmos.color = Color.green;
		//		Gizmos.DrawSphere(start, 1);
		//		Gizmos.color = Color.red;
		//		Gizmos.DrawSphere(end, 1);
		//jump.DrawCatmullRomSpline();
		jump.DrawJumpPath();

#if false
		for (int i = 0; i < jumpSpline.Count; i++) 
		{
			// Draw a yellow sphere at the transform's position
			Vector3 p = jumpSpline[i];
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(p, 1);
		}
#endif
    }
}
