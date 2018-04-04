using System.Collections;
using UnityEngine;


public class CharacterMotor : MonoBehaviour {

	/***** Variable Declaration *****/
	#region Physics
	[Header ("Physics")]
	[SerializeField]
	bool hasGravity = true;
	[SerializeField]
	float gravityScale = 10f;
	[SerializeField]
	float linearDrag = 0.1f;
	[SerializeField]
	bool clampPhysicsVelocity = false;
	[SerializeField]
	float physicsVelocityClamp = 20f;
	#endregion

	#region Collisions 
	[Header ("Collisions")]
	[SerializeField]
	bool collisionDetection = true;
	[SerializeField]
	float skinWidth = .015f;
	[SerializeField]
	int horizontalRayCount = 4;
	[SerializeField]
	int verticalRayCount = 4;
	[SerializeField]
	LayerMask collisionMask = 1;
	#endregion

	#region Debugging
	[Header ("Debugging")]
	bool debugging = true;
	#endregion

	#region Public Variables
	public Vector2 physicsVelocity {
		get {
			return _pVelocity;
		}
	}

	public CollisionInfo collisionInfo {
		get {
			return collisions;
		}
	}

	public int flipDirection { get; private set; }
	#endregion

	#region Private Variables
	Vector2 _velocity;
	Vector2 _pVelocity;

	Collider2D _collider;
	SpriteRenderer _renderer;

	CollisionInfo collisions;
	RaycastOrigins raycastOrigins;
	float horizontalRaySpacing;
	float verticalRaySpacing;

	bool physicsEnabled = true;
	bool xAxisFrozen = false;
	bool yAxisFrozen = false;
	#endregion

	/***** Structures *****/
	#region Structures
	struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
	#endregion

	/***** Start and Update *****/
	#region Start & Update 
	void Start () {
		_collider = GetComponent<Collider2D> ();
		UnityEngine.Assertions.Assert.IsNotNull (_collider, "Missing Collider2D");

		_renderer = GetComponent<SpriteRenderer> ();
		flipDirection = 1;
		CalculateRaySpacing ();
	}
	#endregion

	/***** Velocity Modification *****/
	#region Velocity
	public void IncreaseVelocity (Vector2 v) {
		_velocity += v;
	}

	public void SetVelocity (Vector2 v) {
		_velocity = v;
	}

	public void ApplyForce (Vector2 force) {
		_pVelocity += force;
	}

	public void ResetPhysics () {
		_pVelocity = Vector2.zero;
	}
	#endregion

	/***** Movement Execution *****/
	#region Movement 
	void Move () {
		UpdateRaycastOrigins ();
		collisions.Reset ();

		if (xAxisFrozen)
			_pVelocity.x = _velocity.x = 0;
		if (yAxisFrozen)
			_pVelocity.y = _velocity.y = 0;
		if (_pVelocity.magnitude > linearDrag * Time.deltaTime)
			_pVelocity -= _pVelocity.normalized * linearDrag * Time.deltaTime;
		else
			_pVelocity = Vector2.zero;
		if (clampPhysicsVelocity)
			_pVelocity = Vector2.ClampMagnitude (_pVelocity, physicsVelocityClamp);
		Vector2 movement = MovementCollisions ((_pVelocity + _velocity) * Time.deltaTime);
		transform.Translate (movement, Space.World);
	}

	void LateUpdate () {
		if (hasGravity && physicsEnabled)
			ApplyGravity ();
		Move ();
		if (Grounded () || ObstacleAbove ()) _pVelocity.y = 0;

		_velocity = Vector2.zero;
	}
	#endregion

	/*****      Physics      *****/
	#region Physics 
	public void EnableGravity (bool value) {
		hasGravity = value;
	}

	void ApplyGravity () {
		_pVelocity.y -= gravityScale * 10f * Time.deltaTime;
	}
	#endregion

	/***** Collision Checking *****/
	#region Collisions
	public void EnableCollision (bool vlaue) {
		collisionDetection = vlaue;
	}

	public Vector2 MovementCollisions (Vector2 velocity) {
		if (!collisionDetection) return velocity;
		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}
		return velocity;
	}

	void HorizontalCollisions (ref Vector2 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (debugging)
				Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	void VerticalCollisions (ref Vector2 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			if (debugging)
				Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	void UpdateRaycastOrigins () {
		Bounds bounds = _collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing () {
		Bounds bounds = _collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	#endregion

	/***** Public Functions *****/
	#region Public
	public bool Grounded () {
		return collisions.below;
	}

	public bool ObstacleAbove () {
		return collisions.above;
	}

	public bool CheckObstacle (Vector2 direction, float distance) {
		float rayLength = distance;

		Bounds bounds = _collider.bounds;
		bounds.Expand (skinWidth * -2);
		Vector2 rayOrigin;

		direction.x = (direction.x != 0) ? (int) Mathf.Sign (direction.x) : 0;
		direction.y = (direction.y != 0) ? (int) Mathf.Sign (direction.y) : 0;

		if (direction.x == 0) {
			rayOrigin.x = (bounds.max.x + bounds.min.x) / 2f;
		} else {
			rayOrigin.x = (direction.x > 0) ? bounds.max.x : bounds.min.x;
		}

		if (direction.y == 0) {
			rayOrigin.y = (bounds.max.y + bounds.min.y) / 2f;
		} else {
			rayOrigin.y = (direction.y > 0) ? bounds.max.y : bounds.min.y;
		}

		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, direction, rayLength, collisionMask);

		if (debugging)
			Debug.DrawRay (rayOrigin, direction * rayLength, Color.magenta);

		if (hit) return true;
		return false;

	}
    public GameObject CheckLayerObstacle(Vector2 direction, float distance, LayerMask layer)
    {
        float rayLength = distance;

        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidth * -2);
        Vector2 rayOrigin;

        direction.x = (direction.x != 0) ? (int)Mathf.Sign(direction.x) : 0;
        direction.y = (direction.y != 0) ? (int)Mathf.Sign(direction.y) : 0;

        if (direction.x == 0)
        {
            rayOrigin.x = (bounds.max.x + bounds.min.x) / 2f;
        }
        else
        {
            rayOrigin.x = (direction.x > 0) ? bounds.max.x : bounds.min.x;
        }

        if (direction.y == 0)
        {
            rayOrigin.y = (bounds.max.y + bounds.min.y) / 2f;
        }
        else
        {
            rayOrigin.y = (direction.y > 0) ? bounds.max.y : bounds.min.y;
        }

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayLength, layer);

        if (debugging)
            Debug.DrawRay(rayOrigin, direction * rayLength, Color.magenta);

        if (hit) return hit.transform.gameObject;
        return null;

    }

    public void Flip () {
		_renderer.flipX ^= true;
		flipDirection *= -1;
	}

	public void EnablePhysics (bool value) {
		physicsEnabled = value;
	}

	public void FreezeXAxis (bool value) {
		xAxisFrozen = value;
	}

	public void FreezeYAxis (bool value) {
		yAxisFrozen = value;
	}
	#endregion

}