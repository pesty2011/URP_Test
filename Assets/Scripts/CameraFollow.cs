using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject Target { get; set; }
    Vector3 cameraVelocity { get; set; }
    Camera baseCamera { get; set; }

    public bool followTarget { get; set; }
    public bool trackZPlane { get; set; }

    public Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 7.0f);

    public Vector3 DesiredPosition
    {
        get
        {
            UpdateWorldPosition();
            return desiredPositionOffset;
        }
    }

    private Vector3 desiredPosition = Vector3.zero;
    public Vector3 LookAtOffset = new Vector3(0.0f, 2.8f, 0.0f);
	public float SmoothedSpeed = 0.125f;

	#region Camera Spring
	
	[SerializeField] public bool bSpringCamera = false;

	/// <summary>
	/// Physics coefficient which controls the influence of the camera's position
	/// over the spring force.  The stiffer the spring, the closer it will stay to
	/// the chased object
	/// </summary>
	[SerializeField] public float Stiffness = 4000f;

	/// <summary>
	/// Physics coefficient which approximates internal friction of the sprint.
	/// Sufficient damping will prevent the spring from oscillating infinitely.
	/// </summary>
	[SerializeField] public float Damping = 600f;



	/// <summary>
	/// Mass of the camera body.  Heavier objects require stiffer springs with less
	/// damping to move at the same rate as light objects.
	/// </summary>
	[SerializeField] public float Mass = 1f;

	#endregion



	static float pixelToUnits = 100.0f;		// this value should come from target sprite-renderer component.
	public float RoundToNearestPixel(float unityUnits)
	{
		float valueInPixels = unityUnits * pixelToUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
		return roundedUnityUnits;

	}

    /// <summary>
    /// calculates the camera position based upon the target transform.
    /// </summary>
    private void UpdateWorldPosition()
    {
        if (Target != null)
        {
            float targetXPos = Target.transform.position.x;
            Vector3 cameraPos = baseCamera.transform.position;
            cameraPos.x = targetXPos;

            desiredPosition = cameraPos;
        }
    }


	private void GetDesiredPosition()
	{
		if (Target != null)
		{
			float targetXPos = Target.transform.position.x;
			float roundXPos = RoundToNearestPixel(targetXPos);

			Vector3 cameraPos = baseCamera.transform.position;
			cameraPos.x = roundXPos;

			desiredPosition = cameraPos;
		}
	}

	/// <summary>
	/// Forces camera to be at desired position and to stop moving.  This is useful
	/// when the chased object is first created or after it has been killed or similar
	/// 
	/// Failing to call this after a large change to the chased object's position will
	/// result in the camera quickly flying across the world
	/// </summary>
	public void Reset()
	{
        UpdateWorldPosition();
	}



	// Start is called before the first frame update
	void Start()
    {
        baseCamera = Camera.main;

		Target = GameObject.FindGameObjectWithTag("Player");
        followTarget = true;

        Reset();
	}


	/// <summary>
	/// Moves the camera from its current position towards the desired offset
	/// behind the chased object.  The camera's movement is controlled by a
	/// simple physical spring attached to the camera and anchored to the
	/// desired position.
	/// </summary>
	private void Update()
	{
        float deltaTime = Time.deltaTime;

        //UpdateWorldPosition();
		GetDesiredPosition();




		if (followTarget == true)
        {

			if (bSpringCamera)
			{
				// calculate spring
				Vector3 stretch = baseCamera.transform.position - desiredPosition;
				Vector3 force = -Stiffness * stretch - Damping * cameraVelocity;

				// apply acceleration
				Vector3 acceleration = force / Mass;
				cameraVelocity += acceleration * Time.deltaTime;

				Vector3 targetPosition = baseCamera.transform.position + cameraVelocity;
				baseCamera.transform.position = targetPosition;
			}
			else
			{
				// Smoothly move the camera towards the target position
				baseCamera.transform.position = Vector3.Lerp(baseCamera.transform.position, desiredPosition, 5.0f * deltaTime);
			}
		}
	}
}
