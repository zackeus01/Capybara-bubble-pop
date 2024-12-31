using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    public BallType ballType;
    public BallColor color;
    [SerializeField] private SpriteRenderer coverSprite;
    [SerializeField] private SpriteRenderer ballSprite;
    [SerializeField] private SpriteRenderer highlightSpite;
    [SerializeField] private ScorePopup scorePopup;

    [Header("VFX")]
    [SerializeField] private ParticleSystem sparkleEfx;
    [SerializeField] private ParticleSystem ringEfx;
    [SerializeField] private TrailRenderer trailEfx;
    [SerializeField] private GameObject SpecialVfx;

    private List<ParticleSystem> specialVfxList = new List<ParticleSystem>();
    public TrailRenderer TrailEfx => trailEfx;

    public SpriteRenderer BallSprite { get { return ballSprite; } }
    public SpriteRenderer CoverSprite { get { return coverSprite; } }

    [Header("Ball Attribute")]
    public bool isBallCollided;
    public bool isActiveBall;
    public bool isHavingCover;

    public Color hexColor = Color.white;
    public Vector2 targetVelocity;
    public HexCell targetCell;
    public HexCell parentCell;

    public bool isFirstLineBall;
    public string typeId;
    private WallSettings _wallSettingsCollision;
    public bool isCoroutineRunning;
    public bool isStay;
    public bool isFalling;

    //For firework
    public bool isCollided;

    private bool _isHighLight;
    public bool IsHighLight
    {
        get
        {
            return _isHighLight;
        }
        set
        {
            _isHighLight = value;
            highlightSpite.enabled = value;
        }
    }


    #region Cache Components
    public WallSettings WallSettingsCollision
    {
        get
        {
            if (_wallSettingsCollision == null)
                Debug.LogError("Wall settings collison is not set!");
            return _wallSettingsCollision;
        }

        set
        {
            if (value == null)
            {
                Debug.LogError("Wall settings collision is equal null!");
                return;
            }

            _wallSettingsCollision = value;
        }
    }
    private Rigidbody2D _rbBall;
    public Rigidbody2D rb2d
    {
        get
        {
            if (_rbBall == null)
                _rbBall = GetComponent<Rigidbody2D>();
            return _rbBall;
        }
    }

    private CameraSettings _cameraSettings;
    public CameraSettings CameraSettings
    {
        get
        {
            if (_cameraSettings == null)
                _cameraSettings = Camera.main.GetComponent<CameraSettings>();
            return _cameraSettings;
        }
    }

    private CircleCollider2D _circleCollider;
    public CircleCollider2D CircleCollider
    {
        get
        {
            if (_circleCollider == null)
                _circleCollider = GetComponent<CircleCollider2D>();
            return _circleCollider;
        }
    }

    private RelativeJoint2D _jointConnection;
    public RelativeJoint2D JointConnection
    {
        get
        {
            if (_jointConnection == null)
                Debug.LogWarning("Joint connection is not set!");
            return _jointConnection;
        }
    }

    private bool isHelperBall;
    public bool IsHelperBall => isHelperBall;
    #endregion

    private void Start()
    {
        GetListSpecialVfx();
        CheckHelper();
    }

    private void GetListSpecialVfx()
    {
        ParticleSystem blastPS = SpecialVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        specialVfxList.Add(blastPS);
    }

    private void CheckHelper()
    {
        if (ballType == BallType.Ziczac)
        {
            isHelperBall = true;
        }
        if (ballType == BallType.Bomb)
        {
            isHelperBall = true;
        }
        if (ballType == BallType.Rainbow)
        {
            isHelperBall = true;
        }
        if (ballType == BallType.Firework)
        {
            isHelperBall = true;
        }
    }

    public Ball SetBall(BallColor color)
    {
        this.color = color;
        hexColor = ColorCtrl.GetColor(color);
        if (hexColor != Color.white)
        {
            //Debug.Log("GetColor successfully");
        }
        ballType = BallType.Normal;
        SetSprite(color, ballType);
        return this;
    }
    public Ball SetBall(BallColor color, BallType ballType)
    {
        this.color = color;
        hexColor = ColorCtrl.GetColor(color);
        if (hexColor != Color.white)
        {
            //Debug.Log("GetColor successfully");
        }
        this.ballType = ballType;
        SetSprite(color, ballType);

        if (ballType.Equals(BallType.Grass)) isHavingCover = true;

        return this;
    }

    public void SetSprite(BallColor color, BallType type)
    {
        if (color == BallColor.None)
        {
            ballSprite.sprite = (SkinDataController.Instance.GetCurrentThemeSkinSO(SkinType.BALL) as ThemeSO).GetBallColorSprite("NONE");
        }
        else
        {
            ballSprite.sprite = (SkinDataController.Instance.GetCurrentThemeSkinSO(SkinType.BALL) as ThemeSO).GetBallColorSprite(color);
        }

        //Debug.Log(color + " " + type, this.gameObject);
        if (type != BallType.Normal)
        {
            coverSprite.sprite = (SkinDataController.Instance.GetCurrentThemeSkinSO(SkinType.BALL) as ThemeSO).GetBallTypeSprite(type);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targetCell) return;
        if (!isActiveBall) return;
        if (collision.gameObject == targetCell.gameObject)
        {
            if (IsHelperBall)
            {
                //Debug.Log("Helper");
                HelperEvent.OnHelperBallCollided.Invoke(this);
            }
            else
            {
                //Debug.Log("Gameplay");
                GameplayEvent.OnActiveBallCollided.Invoke(this);
            }

            if (ballType == BallType.Firework)
            {
                return;
            }
            else
            {
                //Debug.Log("BallType = " + ballType, this);
                //Debug.LogError("CircleCollider.isTrigger = false;");
            }

            CircleCollider.isTrigger = false;
            isStay = true;
            isBallCollided = true;
        }
    }

    public float GetBallRadius
    {
        get
        {
            return transform.localScale.x * CircleCollider.radius;
        }
    }

    #region New Move Ball
    public void MoveBall(Vector2 force)
    {
        targetVelocity = force;
        if (gameObject.activeInHierarchy)
        {
            isCoroutineRunning = true;
            StartCoroutine(MoveBallCr());
        }
    }

    private IEnumerator MoveBallCr()
    {
        isBallCollided = false;
        while (!isBallCollided)
        {
            //Debug.LogWarning("MoveBallCr");
            rb2d.velocity = targetVelocity;

            yield return null;
        }
        isCoroutineRunning = false;
        yield break;
    }

    #endregion
    public void AddJoint(Vector2 connectedAnchorPos)
    {
        //_jointConnection = gameObject.AddComponent<FixedJoint2D>();
        //_jointConnection.autoConfigureConnectedAnchor = false;
        //_jointConnection.connectedAnchor = connectedAnchorPos;

        _jointConnection = gameObject.AddComponent<RelativeJoint2D>();
        _jointConnection.maxForce = 500f;
        _jointConnection.maxTorque = 1000f;
        _jointConnection.correctionScale = 1.0f;
        _jointConnection.autoConfigureOffset = true;
    }

    public void DestroyBall()
    {
        if (isActiveBall)
        {
            isActiveBall = false;
            GameplayEvent.OnActiveBallDestroyed.Invoke(this);
        }
        else
        {
            GameplayEvent.OnBallDestroyed.Invoke(this);
        }

        this.gameObject.SetActive(false);
    }
    public void DestroyBall(ScoreType scoreType, BallType destroyball)
    {
        // Add source effect for destroying ball
        SoundManager.Instance.PlayOneShotSFX(SoundKey.BubblePop);

        if (isActiveBall)
        {
            isActiveBall = false;
            GameplayEvent.OnActiveBallDestroyed.Invoke(this);
        }
        else
        {
            GameplayEvent.OnBallDestroyed.Invoke(this);
        }

        if (scoreType == ScoreType.Drop)
        {
            this.rb2d.gravityScale = 0;
            this.rb2d.velocity = Vector2.zero;
        }

        StartCoroutine(PlayScoreVFX(scoreType));
        PlayExplodeBallVFX();
    }

    private void PlayExplodeBallVFX()
    {


        Vector3 initialScale = transform.localScale;
        Vector3 zoomIn = new Vector3(2.8f, 2.8f, 0f);

        Sequence explodeSequence = DOTween.Sequence();
        explodeSequence.Append(this.transform.DOScale(zoomIn, 0.2f))
                     .Append(this.BallSprite.DOFade(0f, 0.1f))
                     .Join(this.CoverSprite.DOFade(0f, 0.1f));

        //On complete
        explodeSequence.OnComplete(() =>
        {
            

            if (ballType == BallType.Normal)
            {
                SetVFXColor();
                sparkleEfx.Play();
                ringEfx.Play();
            }
            else
            {
                PlaySpecialExplodeVfx();
            }

            StartCoroutine(WaitForParticleToEnd(initialScale));
        });
    }

    private void PlaySpecialExplodeVfx()
    {
        if (ballType == BallType.Blast)
        {
            specialVfxList[0].Play();
        }
        
    }

    public void SetVFXColor()
    {
        ParticleSystem.MainModule ringMain = ringEfx.main;
        ringMain.startColor = hexColor;
        ParticleSystem.MainModule sparkleMain = sparkleEfx.main;
        sparkleMain.startColor = hexColor;
        trailEfx.material.SetColor("_Color2", hexColor);
    }

    private IEnumerator WaitForParticleToEnd(Vector3 initialScale)
    {
        yield return new WaitForSeconds(ringEfx.main.duration);

        this.gameObject.SetActive(false);
        // Reset position, scale, and color
        this.transform.localScale = initialScale;
    }

    public void DestroyCover()
    {
        isHavingCover = false;
        coverSprite.gameObject.SetActive(false);
    }

    public void PlayDropVFX()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = 8f;
        float multiply = 1f;
        rb2d.AddForce(new Vector2(x * multiply, y * multiply), ForceMode2D.Impulse);
    }

    public IEnumerator PlayScoreVFX(ScoreType scoreType)
    {
        //this.transform.parent = ScoreController.Instance.transform;
        yield return new WaitForSeconds(0.3f);
        scorePopup.gameObject.SetActive(true);
        scorePopup.PlayVFX(scoreType);
    }

    public void PlayCurveVFX()
    {
        Vector3[] waypoints = new Vector3[] {
            new Vector3(0, 0, 0), // Start position
            new Vector3(2, 4, 0), // Control point
            new Vector3(4, 0, 0)  // End position
        };

        // Move the object along the curved path
        transform.DOPath(waypoints, 0.4f, PathType.CatmullRom)
                 .SetEase(Ease.Linear); // Use CatmullRom for smooth curves
    }

    public void PlayWinVFX(Vector3 destination, float delay)
    {
        rb2d.gravityScale = 0f;
        this.transform.DOMove(destination, 0.3f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            ScoreController.Instance.AddScore(ScoreType.Win, 0.01f);
            this.DestroyBall(ScoreType.Win, BallType.Normal);
        });
    }
}