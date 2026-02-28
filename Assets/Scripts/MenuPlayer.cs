namespace MenuPlayer
{
    using UnityEngine;

    public class MenuPlayer : MonoBehaviour
    {
        private static MenuPlayer _instance;
        public static MenuPlayer Instance { get { return _instance; } }

        public SphereCollider headCollider;
        public CapsuleCollider bodyCollider;

        public Transform leftHandFollower;
        public Transform rightHandFollower;
        public Transform rightHandTransform;
        public Transform leftHandTransform;

        private Vector3 lastLeftHandPosition;
        private Vector3 lastRightHandPosition;
        private Vector3 lastHeadPosition;
        private Vector3 lastPosition;

        private Rigidbody playerRigidBody;

        public int velocityHistorySize;
        public float maxArmLength = 1.5f;
        public float unStickDistance = 1f;
        public float velocityLimit;
        public float maxJumpSpeed;
        public float jumpMultiplier;

        private Vector3[] velocityHistory;
        private int velocityIndex;
        private Vector3 currentVelocity;
        private Vector3 denormalizedVelocityAverage;

        public Vector3 rightHandOffset;
        public Vector3 leftHandOffset;

        public LayerMask locomotionEnabledLayers;

        public bool wasLeftHandTouching;
        public bool wasRightHandTouching;
        public bool disableMovement = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;

            InitializeValues();
        }

        public void InitializeValues()
        {
            playerRigidBody = GetComponent<Rigidbody>();
            velocityHistory = new Vector3[velocityHistorySize];
            lastLeftHandPosition = leftHandFollower.position;
            lastRightHandPosition = rightHandFollower.position;
            lastHeadPosition = headCollider.transform.position;
            velocityIndex = 0;
            lastPosition = transform.position;
        }

        private Vector3 CurrentLeftHandPosition()
        {
            Vector3 pos = leftHandTransform.position + leftHandTransform.rotation * leftHandOffset;
            if ((pos - headCollider.transform.position).magnitude < maxArmLength)
                return pos;
            return headCollider.transform.position + (pos - headCollider.transform.position).normalized * maxArmLength;
        }

        private Vector3 CurrentRightHandPosition()
        {
            Vector3 pos = rightHandTransform.position + rightHandTransform.rotation * rightHandOffset;
            if ((pos - headCollider.transform.position).magnitude < maxArmLength)
                return pos;
            return headCollider.transform.position + (pos - headCollider.transform.position).normalized * maxArmLength;
        }

        private void Update()
        {
            bool leftHandColliding = false;
            bool rightHandColliding = false;
            Vector3 finalPosition;
            Vector3 firstIterationLeftHand = Vector3.zero;
            Vector3 firstIterationRightHand = Vector3.zero;
            Vector3 rigidBodyMovement = Vector3.zero;

            // left hand
            Vector3 distanceTraveled = CurrentLeftHandPosition() - lastLeftHandPosition;
            if (IterativeCollisionSphereCast(lastLeftHandPosition, 0.05f, distanceTraveled, 0.995f, out finalPosition))
            {
                firstIterationLeftHand = wasLeftHandTouching ? lastLeftHandPosition - CurrentLeftHandPosition() : finalPosition - CurrentLeftHandPosition();
                playerRigidBody.linearVelocity = Vector3.zero;
                leftHandColliding = true;
            }

            // right hand
            distanceTraveled = CurrentRightHandPosition() - lastRightHandPosition;
            if (IterativeCollisionSphereCast(lastRightHandPosition, 0.05f, distanceTraveled, 0.995f, out finalPosition))
            {
                firstIterationRightHand = wasRightHandTouching ? lastRightHandPosition - CurrentRightHandPosition() : finalPosition - CurrentRightHandPosition();
                playerRigidBody.linearVelocity = Vector3.zero;
                rightHandColliding = true;
            }

            // Combine movements
            if ((leftHandColliding || wasLeftHandTouching) && (rightHandColliding || wasRightHandTouching))
                rigidBodyMovement = (firstIterationLeftHand + firstIterationRightHand) / 2;
            else
                rigidBodyMovement = firstIterationLeftHand + firstIterationRightHand;

            // Apply rigidbody movement (just floats in air)
            transform.position += rigidBodyMovement;

            // Update last positions
            lastLeftHandPosition = leftHandColliding ? finalPosition : CurrentLeftHandPosition();
            lastRightHandPosition = rightHandColliding ? finalPosition : CurrentRightHandPosition();
            lastHeadPosition = headCollider.transform.position;

            // Store velocity for potential jumps
            StoreVelocities();

            // Apply jump force if velocity is high enough
            if ((rightHandColliding || leftHandColliding) && !disableMovement)
            {
                if (denormalizedVelocityAverage.magnitude > velocityLimit)
                {
                    playerRigidBody.linearVelocity = (denormalizedVelocityAverage * jumpMultiplier).magnitude > maxJumpSpeed ?
                        denormalizedVelocityAverage.normalized * maxJumpSpeed :
                        denormalizedVelocityAverage * jumpMultiplier;
                }
            }

            // Unstick hands
            if (leftHandColliding && (CurrentLeftHandPosition() - lastLeftHandPosition).magnitude > unStickDistance)
            {
                lastLeftHandPosition = CurrentLeftHandPosition();
                leftHandColliding = false;
            }

            if (rightHandColliding && (CurrentRightHandPosition() - lastRightHandPosition).magnitude > unStickDistance)
            {
                lastRightHandPosition = CurrentRightHandPosition();
                rightHandColliding = false;
            }

            leftHandFollower.position = lastLeftHandPosition;
            rightHandFollower.position = lastRightHandPosition;
            wasLeftHandTouching = leftHandColliding;
            wasRightHandTouching = rightHandColliding;
        }

        private bool IterativeCollisionSphereCast(Vector3 startPosition, float radius, Vector3 movementVector, float precision, out Vector3 endPosition)
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(startPosition, radius * precision, movementVector, out hitInfo, movementVector.magnitude, locomotionEnabledLayers.value))
            {
                endPosition = hitInfo.point + hitInfo.normal * radius;
                return true;
            }
            endPosition = Vector3.zero;
            return false;
        }

        public void Turn(float degrees)
        {
            transform.RotateAround(headCollider.transform.position, transform.up, degrees);
            denormalizedVelocityAverage = Quaternion.Euler(0, degrees, 0) * denormalizedVelocityAverage;
            for (int i = 0; i < velocityHistory.Length; i++)
                velocityHistory[i] = Quaternion.Euler(0, degrees, 0) * velocityHistory[i];
        }

        private void StoreVelocities()
        {
            velocityIndex = (velocityIndex + 1) % velocityHistorySize;
            Vector3 oldestVelocity = velocityHistory[velocityIndex];
            currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
            denormalizedVelocityAverage += (currentVelocity - oldestVelocity) / velocityHistorySize;
            velocityHistory[velocityIndex] = currentVelocity;
            lastPosition = transform.position;
        }
    }
}