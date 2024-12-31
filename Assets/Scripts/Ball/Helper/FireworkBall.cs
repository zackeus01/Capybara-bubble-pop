using UnityEngine;

public class FireworkBall : MonoBehaviour
{
    [SerializeField]
    public GameObject Hitbox;

    public Collider2D Collider;

    [SerializeField]
    private Ball ball;

    public bool isCollided = false;

    private bool isDestroyed = false;

    private void Reset()
    {
        ball = this.gameObject.GetComponent<Ball>();
    }

    private void Start()
    {
        Collider = Hitbox.GetComponent<Collider2D>();
        Debug.Log("Start Firework isDestroyed = " + isDestroyed);
    }

    private void Update()
    {

        if (isDestroyed) return;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);

        // Check if the object is outside the camera's view
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            isDestroyed = true;

            Debug.Log("Destroy Firework");
            ball.DestroyBall(ScoreType.Destroy, BallType.Firework);

            if (isCollided)
            {
                HelperEvent.OnAllHelperTargetDestroyed.Invoke();
            }
            else
            {
                GameplayEvent.OnAllFieldActionsEnd.Invoke();
                HelperEvent.OnHelperDeactivated.Invoke();
            }
        }
    }
}
