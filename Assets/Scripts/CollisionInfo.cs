public struct CollisionInfo {
	public bool above, below;
	public bool left, right;

	public void Reset () {
		above = below = false;
		left = right = false;
	}

	public override string ToString () {
		return "Up: " + above + " Down: " + below + " Left: " + left + " Right: " + right;
	}

}