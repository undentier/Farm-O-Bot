using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlocksLogic : MonoBehaviour
{
	[SerializeField] private float FOVAngle;
	[SerializeField] private float smoothDamp;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private Vector3[] directionsToCheckWhenAvoidingObstacles;

	private List<FlocksLogic> cohesionNeighbours = new List<FlocksLogic>();
	private List<FlocksLogic> avoidanceNeighbours = new List<FlocksLogic>();
	private List<FlocksLogic> aligementNeighbours = new List<FlocksLogic>();

	private ManagerMob assignedFlock;
	private Vector3 currentVelocity;
	private Vector3 currentObstacleAvoidanceVector;
	private float speed;

	public Transform myTransform { get; set; }

	private void Awake()
	{
		myTransform = transform;
	}

	public void AssignFlock(ManagerMob flock)
	{
		assignedFlock = flock;
	}

	public void InitializeSpeed(float speed)
	{
		this.speed = speed;
	}

	public void MoveUnit()
	{
		FindNeighbours();
		CalculateSpeed();

		var cohesionVector = CalculateCohesionVector() * assignedFlock.cohesionWeight;
		var avoidanceVector = CalculateAvoidanceVector() * assignedFlock.avoidanceWeight;
		var aligementVector = CalculateAligementVector() * assignedFlock.aligementWeight;
		var boundsVector = CalculateBoundsVector() * assignedFlock.boundsWeight;
		var obstacleVector = CalculateObstacleVector() * assignedFlock.obstacleWeight;

		var moveVector = cohesionVector + avoidanceVector + aligementVector + boundsVector + obstacleVector;
		moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
		moveVector = moveVector.normalized * speed;
		/*if (moveVector == Vector3.zero)
			moveVector = transform.forward;*/

		Debug.Log(moveVector);

		myTransform.forward = moveVector;
		myTransform.position += moveVector * Time.deltaTime;
	}



	private void FindNeighbours()
	{
		cohesionNeighbours.Clear();
		avoidanceNeighbours.Clear();
		aligementNeighbours.Clear();
		FlocksLogic[] allUnits = assignedFlock.allMobs;
		for (int i = 0; i < allUnits.Length; i++)
		{
			var currentUnit = allUnits[i];
			if (currentUnit != this)
			{
				float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.myTransform.position - myTransform.position);
				if (currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
				{
					cohesionNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
				{
					avoidanceNeighbours.Add(currentUnit);
				}
				if (currentNeighbourDistanceSqr <= assignedFlock.aligementDistance * assignedFlock.aligementDistance)
				{
					aligementNeighbours.Add(currentUnit);
				}
			}
		}
	}

	private void CalculateSpeed()
	{
		if (cohesionNeighbours.Count == 0)
			return;
		speed = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			speed += cohesionNeighbours[i].speed;
		}

		speed /= cohesionNeighbours.Count;
		speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
	}

	private Vector3 CalculateCohesionVector()
	{
		if (cohesionNeighbours.Count == 0)
			return Vector3.zero;
		Vector3 cohesionVector = Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < cohesionNeighbours.Count; i++)
		{
			if (IsInFOV(cohesionNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				cohesionVector += cohesionNeighbours[i].myTransform.position;
			}
		}

		cohesionVector /= neighboursInFOV;
		cohesionVector -= myTransform.position;
		cohesionVector = cohesionVector.normalized;
		return cohesionVector;
	}

	private Vector3 CalculateAligementVector()
	{
		var aligementVector = myTransform.forward;
		if (aligementNeighbours.Count == 0)
			return myTransform.forward;
		int neighboursInFOV = 0;
		for (int i = 0; i < aligementNeighbours.Count; i++)
		{
			if (IsInFOV(aligementNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				aligementVector += aligementNeighbours[i].myTransform.forward;
			}
		}

		aligementVector /= neighboursInFOV;
		aligementVector = aligementVector.normalized;
		return aligementVector;
	}

	private Vector3 CalculateAvoidanceVector()
	{
		var avoidanceVector = Vector3.zero;
		if (aligementNeighbours.Count == 0)
			return Vector3.zero;
		int neighboursInFOV = 0;
		for (int i = 0; i < avoidanceNeighbours.Count; i++)
		{
			if (IsInFOV(avoidanceNeighbours[i].myTransform.position))
			{
				neighboursInFOV++;
				avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
			}
		}

		avoidanceVector /= neighboursInFOV;
		avoidanceVector = avoidanceVector.normalized;
		return avoidanceVector;
	}

	private Vector3 CalculateBoundsVector()
	{
		var offsetToCenter = assignedFlock.transform.position - myTransform.position;
		if (offsetToCenter.magnitude >= assignedFlock.boundsDistance * 0.9f)
		{
			return offsetToCenter.normalized;
		}
		else
		{
			return Vector3.zero;
		}

	}

	private Vector3 CalculateObstacleVector()
	{
		Vector3 obstacleVector = Vector3.zero;
		RaycastHit hit;
		if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
		{
			obstacleVector = FindBestDirectionToAvoidObstacle();
		}
		else
		{
			currentObstacleAvoidanceVector = Vector3.zero;
		}
		return obstacleVector;
	}

	private Vector3 FindBestDirectionToAvoidObstacle()
	{
		if (currentObstacleAvoidanceVector != Vector3.zero)
		{
			RaycastHit hit;
			if (!Physics.Raycast(myTransform.position, myTransform.forward, out hit, assignedFlock.obstacleDistance, obstacleMask))
			{
				return currentObstacleAvoidanceVector;
			}
		}
		float maxDistance = int.MinValue;
		var selectedDirection = Vector3.zero;
		for (int i = 0; i < directionsToCheckWhenAvoidingObstacles.Length; i++)
		{

			RaycastHit hit;
			Vector3 currentDirection = myTransform.TransformDirection(directionsToCheckWhenAvoidingObstacles[i].normalized);
			if (Physics.Raycast(myTransform.position, currentDirection, out hit, assignedFlock.obstacleDistance, obstacleMask))
			{

				float currentDistance = (hit.point - myTransform.position).sqrMagnitude;
				if (currentDistance > maxDistance)
				{
					maxDistance = currentDistance;
					selectedDirection = currentDirection;
				}
			}
			else
			{
				selectedDirection = currentDirection;
				currentObstacleAvoidanceVector = currentDirection.normalized;
				return selectedDirection.normalized;
			}
		}
		return selectedDirection.normalized;
	}

	private bool IsInFOV(Vector3 position)
	{
		return Vector3.Angle(myTransform.forward, position - myTransform.position) <= FOVAngle;
	}
}
