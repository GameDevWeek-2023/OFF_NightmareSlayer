using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    public float minSize = .18f;
    public float maxSize = .24f;
    public float minVelocity = 2f;
    public float maxVelocity = 6f;
    public float obtainableTime = .5f;
    public float obtainSpeed = 10f;
    public float despawnTime = 10f;

    private Rigidbody2D parentRigidbody;
    private Collider2D parentCollider;
    private CircleCollider2D collider;

    private float awakeTime;
    private bool obtained;

    private void Awake()
    {
        parentRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        parentCollider = transform.parent.GetComponent<Collider2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        obtained = false;
        
        float size = Random.Range(minSize,maxSize);
        transform.parent.localScale = size * Vector3.one;
        
        float velX = Random.Range(-1f,1f);
        float velY = Random.Range(0f,1f);
        float velMag = Random.Range(minVelocity,maxVelocity);

        parentRigidbody.velocity = new Vector2(velX,velY).normalized * velMag;

        awakeTime = Time.time;

        StartCoroutine(Despawn());
    }

    public void Obtain()
    {
        if (Time.time - awakeTime < obtainableTime) return;
        obtained = true;
        parentRigidbody.gravityScale = 0f;
        parentCollider.enabled = false;
        collider.radius = 1f;
        StartCoroutine(BecomeObtainable());
    }

    private IEnumerator BecomeObtainable()
    {
        yield return 0;
        gameObject.tag = "Coin";
    }

    private void Update()
    {
        if (!obtained) return;

        parentRigidbody.velocity =
            (PlayerScript.instance.transform.position - transform.position).normalized * obtainSpeed;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(transform.parent.gameObject);
    }
}
