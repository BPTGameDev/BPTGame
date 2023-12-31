using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float topBound = 20;
    [SerializeField]
    private float botBound = 0;
    public GameManager manager;
    public Sprite sprites;
    private SpriteRenderer _spriteRenderer;
    public static int maxHealth = 10;
    public int currentHealth = 10;
    public static int bulletNumber = 200;
    public static int playerHealth = 10;
    public HealthBar healthBar;
    public float rotateSpeed = 5f;
    public GameObject bulletPrefab;
    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool _thrusting;
    private float _turnDirection;
    //private bool _touched = false; not used
    private float fireRate = 0.2f;
    private float nextFire = 0.0f;
    protected float bulletTimer;
    public int DelayAmount = 5;

    public AudioSource CollisionAudioSource;

    PhotonView view;

    //this function is called in the spawner to change the color
    public void Color(int players, GameManager m)
    {
        Debug.Log("COLOR CHANGE CALLED");
        _spriteRenderer.sprite = sprites;
        manager = m;
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        playerHealth = 10;
        bulletNumber = 200;
    }
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        bulletTimer += Time.deltaTime;
        if (bulletTimer > DelayAmount)
        {
            bulletTimer = 0;
            bulletNumber++;
        }

        if (view.IsMine) //this is used for photon so that you only control one player
        {

            /////////// MOUSE CONTROLS
            // Move and shoot bullets with left mouse click
            if (Input.GetMouseButton(0))
            {
                if (Time.time > nextFire && bulletNumber > 0)
                {
                    Shoot();
                    nextFire = Time.time + fireRate;
                    bulletNumber--;
                }
                // Locating mouse and player object
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 0;
                Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

                // Follow the mouse properly (offset)
                mousePos.x = mousePos.x - objectPos.x;
                mousePos.y = mousePos.y - objectPos.y;

                // Rotate player according to mouse position
                float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

                // Move player according to mouse position
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = 0;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, thrustSpeed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, botBound, topBound), transform.position.z);
            }

            // Shooting function (only shoot once per click and not hold down)

            /*if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            if (Input.GetKeyDown("space"))
            {
                Shoot();
            }*/

            /////////// TOUCH CONTROLS
            /*if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                var mouse = Input.mousePosition;
                var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
                var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
                if (touch.phase == TouchPhase.Stationary)
                {
                    _thrusting = true;
                }
                else if (!_touched)
                {
                    _touched = true;
                    _thrusting = false;
                    Shoot();
                }
            }
            else
            {
                _thrusting = false;
                _touched = false;
            }*/
        }

    }
    private void FixedUpdate()
    {
        if (_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

    }

    void Shoot()
    {
        GameObject bulletobj = PhotonNetwork.Instantiate(this.bulletPrefab.name, this.transform.position, this.transform.rotation);
        BulletScript bullet = bulletobj.GetComponent<BulletScript>();
        bullet.addPlayer(this);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            CollisionAudioSource.Play();
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;
            currentHealth -= 2;
            playerHealth = currentHealth;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                this.gameObject.SetActive(false);
                manager.PlayerDied();
            }
        }
    }
}
