using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Trajectory))]
public class Shooter : MonoBehaviour
{
    [Header("General")]
    [Range(10f, 180f)]
    [SerializeField] private CircleCollider2D _changeBallArea;
    [SerializeField] private float _maxMoveBallAngle;
    bool isAiming = false;
    bool isShoot = false;
    //bool isGameOver = false;
    bool canRotate = false;
    private Transform _availableBallsHolder;
    [SerializeField]
    private Ball _activeBall;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField]
    private Ball _nextBall;
    [SerializeField]
    private Ball _tempBall;
    private readonly Queue<Ball> _availableBalls = new Queue<Ball>();
    [SerializeField] private TextMeshProUGUI countBall;
    private int countBallLeft;
    private Ball _helperBall;

    [SerializeField]
    private bool isHelperEnable;
    [SerializeField]
    private bool isHelperBallShooted;
    [SerializeField]
    private GameObject shooterGFX;

    [Header("Ball")]
    [SerializeField] GameObject currentBall;
    [SerializeField] GameObject nextBall;
    [SerializeField] LevelField _levelField;

    private Vector3[] wayToNextBall, wayToCurrentBall;

    [SerializeField]
    private List<Ball> listHelper;

    #region Cache Components
    private Rigidbody2D _catapultRb;
    public Rigidbody2D CatapultRb
    {
        get
        {
            if (_catapultRb == null)
                _catapultRb = GetComponent<Rigidbody2D>();
            return _catapultRb;
        }
    }

    [SerializeField] private BoxCollider2D _catapultBoxCollider;
    public BoxCollider2D CatapultBoxCollider
    {
        get
        {
            if (_catapultBoxCollider == null)
                _catapultBoxCollider = GetComponent<BoxCollider2D>();
            return _catapultBoxCollider;
        }
    }
    public CircleCollider2D changeBallArea
    {
        get
        {
            if (_changeBallArea == null)
            {
                _changeBallArea = GetComponent<CircleCollider2D>();
            }
            return _changeBallArea;
        }
    }
    private CameraSettings _cameraSettings;
    private CameraSettings CameraSettings
    {
        get
        {
            if (_cameraSettings == null)
                _cameraSettings = Camera.main.GetComponent<CameraSettings>();
            return _cameraSettings;
        }
    }

    private Trajectory _trajectory;
    public Trajectory CatapultTrajectory
    {
        get
        {
            if (_trajectory == null)
                _trajectory = GetComponent<Trajectory>();
            return _trajectory;
        }
    }
    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 90;

        //GameplayEvent
        GameplayEvent.OnAllFieldActionsEnd.AddListener(UnlockCatapult);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(MoveSecondBallToActiveBall);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(SpawnBall);
        GameplayEvent.OnAllFieldActionsEnd.AddListener(CheckBallColorOnField);
        GameplayEvent.OnCountBallsOnFieldChecked.AddListener(CheckAvailableBallsCount);
        GameplayEvent.OnNoBallOnField.AddListener(LockCatapult);
        GameplayEvent.OnNoBallOnField.AddListener(ShootAllBallLeft);
        GameplayEvent.OnGameOver.AddListener(LockCatapult);

        //UIEvent
        UIEvent.OnClickPause.AddListener(PauseHandle);
        UIEvent.OnClickResume.AddListener(UnpauseHandle);

        //HelperEvent
        HelperEvent.OnHelperActivated.AddListener(ActiveHelperBall);
        HelperEvent.OnHelperDeactivated.AddListener(RemoveHelperBall);

        LockCatapult();
    }
    private void OnDestroy()
    {
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(UnlockCatapult);
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(MoveSecondBallToActiveBall);
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(SpawnBall);
        GameplayEvent.OnAllFieldActionsEnd.RemoveListener(CheckBallColorOnField);
        GameplayEvent.OnCountBallsOnFieldChecked.RemoveListener(CheckAvailableBallsCount);

        UIEvent.OnClickPause.RemoveListener(PauseHandle);
        UIEvent.OnClickResume.RemoveListener(UnpauseHandle);

        HelperEvent.OnHelperActivated.RemoveListener(ActiveHelperBall);
        HelperEvent.OnHelperDeactivated.RemoveListener(RemoveHelperBall);

        GameplayEvent.OnNoBallOnField.RemoveListener(LockCatapult);
        GameplayEvent.OnNoBallOnField.RemoveListener(ShootAllBallLeft);
        GameplayEvent.OnGameOver.RemoveListener(LockCatapult);
    }
    private void Start()
    {
        LoadComponent();
        SetUpValue();
    }
    private void OnMouseDown()
    {
        //countBall.text = _availableBalls.Count.ToString();
        OnPlayerDraggingMouse();
        HandleMouseDown();
    }
    private void OnMouseDrag()
    {
        OnPlayerDraggingMouse();
    }
    private void OnMouseExit()
    {
        isShoot = false;
        isAiming = false;

        //countBall.text = _availableBalls.Count.ToString();
    }
    private void OnMouseUp()
    {
        OnPlayerDraggingMouse();
        HandleShooter();
    }
    private void HandleShooter()
    {
        //Get input for mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Swap ball if touch catapult
        if (IsMouseOverCollider(changeBallArea, mousePosition))
        {
            //Debug.Log("Change ball click");
            //Debug.Log("Event complete: " + _levelField.isDropBallCompleted);
            SwapBall();
            if (_levelField.isDropBallCompleted)
            {

            }
        }
        else if (IsMouseOverCollider(_catapultBoxCollider, mousePosition))
        {
            //Debug.Log("Shoot ball click");
            if (!isShoot)
            {
                MoveBallAtCatapultPosition(_activeBall);
                CatapultTrajectory.HideTrajectory(_activeBall);
                //return;
            }
            else
            {
                _activeBall.CircleCollider.enabled = true;
                _activeBall.SetVFXColor();
                _activeBall.TrailEfx.enabled = true;
                GameplayEvent.OnActiveBallShooted.Invoke(_activeBall);


                if (_activeBall.ballType == BallType.Firework)
                {
                    Debug.Log("--------------------Shooted firework---------------------");
                }

                _activeBall.MoveBall(CalculateForceForBall());
                LockCatapult();
                CatapultTrajectory.HideTrajectory(_activeBall);

                if (!isHelperEnable)
                {
                    countBallLeft--;
                    GameplayEvent.OnAvailableBallsCountChanged.Invoke(countBallLeft);
                    countBall.text = countBallLeft.ToString();
                }
                else
                {
                    isHelperBallShooted = true;
                    HelperEvent.OnHelperBallShooted.Invoke();
                }
            }
        }
        isShoot = false;
        isAiming = false;
    }
    private void HandleMouseDown()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (IsMouseOverCollider(_catapultBoxCollider, mousePosition))
        {
            //countBall.text = _availableBalls.Count.ToString();
            isAiming = true;
        }
    }
    private void SetUpValue()
    {
        //UnpauseHandle();

        if (!LevelDataHolder.LevelData)
        {
            Debug.LogError("Level data is not set!");
            return;
        }

        StartCoroutine(CheckTextUpdate());
        //Debug.Log(countBall.text);


        // Spawn the first 2 balls

        for (int i = 0; i < 3; ++i)
        {
            SpawnBall();
        }
        //GenerateAvailableBalls(LevelDataHolder.LevelData.BallsTypeInLevel, LevelDataHolder.LevelData.CountAvailableBalls);
        //Debug.Log(LevelDataHolder.LevelData.BallsTypeInLevel.Count);
        GenerateBallOnStart();
    }
    private IEnumerator CheckTextUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        countBallLeft = LevelDataHolder.LevelData.CountAvailableBalls;
        countBall.text = countBallLeft.ToString();
        if (countBall.text.Equals(countBallLeft.ToString()))
        {
            GameplayEvent.OnAvailableBallsCountChanged.Invoke(countBallLeft);
            yield break;
        }
    }
    private void LoadComponent()
    {
        currentBall = GameObject.Find("BallCurrent");
        nextBall = GameObject.Find("BallNext");
        _availableBallsHolder = new GameObject("AvailableBallsHolder").transform;
        _availableBallsHolder.SetParent(transform);
        _levelField = GameObject.Find("LevelField").GetComponent<LevelField>();
    }
    private bool IsMouseOverCollider(Collider2D collider, Vector2 mousePosition)
    {
        if (collider is BoxCollider2D box)
        {
            canRotate = true;
            return box.OverlapPoint(mousePosition);
        }
        else if (collider is CircleCollider2D circle)
        {
            canRotate = false;
            return circle.OverlapPoint(mousePosition);
        }
        return false;
    }
    private void OnPlayerDraggingMouse()
    {
        if (isAiming)
        {
            if (canRotate)
            {
                SetBallRotationByMouse();
                SetGunRotationByMouse();
            }
            if (_activeBall == null)
            {
                return;
            }
            if (!_activeBall.isCoroutineRunning)
            {
                CatapultTrajectory.ShowTrajectory(_activeBall);
            }
            isShoot = true;
        }
        else
        {
            CatapultTrajectory.HideTrajectory(_activeBall);
        }
    }
    private void SetGunRotationByMouse()
    {
        if (countBallLeft > 0)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = worldMousePosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            shooterGFX.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    public void LockCatapult()
    {
        CustomDebug.Log("LockCatapult", Color.red);
        CatapultBoxCollider.enabled = false;
        changeBallArea.enabled = false;
    }
    public void UnlockCatapult()
    {
        CustomDebug.Log("UnlockCatapult", Color.red);
        CatapultBoxCollider.enabled = true;
        changeBallArea.enabled = true;
    }
    public void PauseHandle()
    {
        LockCatapult();
        Time.timeScale = 0f;
    }
    public void UnpauseHandle()
    {
        UnlockCatapult();
        Time.timeScale = 1f;
    }
    private void SetBallRotationByMouse()
    {
        if (countBallLeft <= 0) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = worldMousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _activeBall.rb2d.rotation = angle - 90;

    }
    private void SpawnBall()
    {
        //Debug.Log("Spawn Ball");

        Ball ball = Instantiate(_ballPrefab, _availableBallsHolder.transform).SetBall(GetRandomColor(), BallType.Normal);
        ball.rb2d.mass = 0f;
        ball.rb2d.gravityScale = 0f;
        ball.CircleCollider.isTrigger = true;
        ball.gameObject.SetActive(false);
        _availableBalls.Enqueue(ball);
    }
    private BallColor GetRandomColor()
    {
        return _levelField.GetRandomBallColor();
    }
    private void MoveBallAtCatapultPosition(Ball ball)
    {
        if (ball)
        {
            //Debug.LogWarning("MoveBallAtCatapultPosition");
            ball.rb2d.position = currentBall.transform.position;
        }
    }
    private void MoveSecondBallToActiveBall()
    {
        Debug.Log("MoveSecondBallToActiveBall");

        if (isHelperEnable)
        {
            //Debug.Log("Inactive ");
            return;
        }

        //Debug.Log("Active ");
        if (_activeBall.transform.position == currentBall.transform.position)
        {
            //Debug.LogError("Acive ball exist!");
            //_activeBall.gameObject.SetActive(false);
        }

        _activeBall = _nextBall;
        _activeBall.transform.rotation = shooterGFX.transform.rotation;

        GameplayEvent.OnActiveBallChanged.Invoke(_activeBall);
        //Debug.Log("Active ball " + _activeBall.name);

        if (_activeBall)
        {
            _activeBall.transform.position = currentBall.transform.position;
        }

        if (!_availableBalls.TryDequeue(out _nextBall))
        {
            GameplayEvent.OnAvailableBallsEnd.Invoke();
            return;
        }

        if (!_levelField.BallsOnField.Exists(b => b.color.Equals(_activeBall.color)))
            _activeBall.SetBall(GetRandomColor(), BallType.Normal);
        if (!_levelField.BallsOnField.Exists(b => b.color.Equals(_nextBall.color)))
            _nextBall.SetBall(GetRandomColor(), BallType.Normal);

        _nextBall.gameObject.SetActive(true);
        _nextBall.transform.SetParent(transform);
        _nextBall.transform.position = nextBall.transform.position;
        _nextBall.isActiveBall = true;
        _nextBall.CircleCollider.enabled = false;

        if (_availableBalls.Count > 0)
        {
            GameplayEvent.OnNextBallChanged.Invoke(_availableBalls.Peek());
        }
    }
    private void GenerateBallOnStart()
    {
        if (!_availableBalls.TryDequeue(out _activeBall))
        {
            GameplayEvent.OnAvailableBallsEnd.Invoke();
            return;
        }
        GameplayEvent.OnActiveBallChanged.Invoke(_activeBall);

        countBall.text = _availableBalls.Count.ToString();
        //GameplayEvent.OnAvailableBallsCountChanged.Invoke(countBallLeft);

        _activeBall.gameObject.SetActive(true);
        _activeBall.transform.SetParent(transform);
        _activeBall.transform.position = currentBall.transform.position;
        _activeBall.isActiveBall = true;
        _activeBall.CircleCollider.enabled = false;

        if (_availableBalls.Count > 0)
        {
            GameplayEvent.OnNextBallChanged.Invoke(_availableBalls.Peek());
        }
        if (!_availableBalls.TryDequeue(out _nextBall))
        {
            GameplayEvent.OnAvailableBallsEnd.Invoke();
            return;
        }

        //GameplayEvent.OnAvailableBallsCountChanged.Invoke(_availableBalls.Count);

        _nextBall.gameObject.SetActive(true);
        _nextBall.transform.SetParent(transform);
        _nextBall.transform.position = nextBall.transform.position;
        _nextBall.isActiveBall = true;
        _nextBall.CircleCollider.enabled = false;

        if (_availableBalls.Count > 0)
        {
            GameplayEvent.OnNextBallChanged.Invoke(_availableBalls.Peek());
        }
    }
    private void SwapBall()
    {

        if (isHelperEnable)
        {
            return;
        }

        shooterGFX.transform.DORotate(Vector3.zero, 0.1f);
        Ball temp = _activeBall;
        _activeBall = _nextBall;
        _activeBall.transform.rotation = shooterGFX.transform.rotation;
        GameplayEvent.OnActiveBallChanged.Invoke(_activeBall);
        _nextBall = temp;

        Vector3 _nextBallPos = _nextBall.transform.position;
        Vector3 _activeBallPos = _activeBall.transform.position;

        wayToNextBall = new Vector3[] {
            _nextBallPos + new Vector3(0, 2.6f, 0),
            _nextBallPos + new Vector3(2.2f, 4.5f, 0),
            _nextBallPos + new Vector3(3.88f, 1.29f, 0),
            nextBall.transform.position
        };

        float duration = 0.3f;

        wayToCurrentBall = new Vector3[] {
            _nextBallPos + new Vector3(3.88f, 1.29f, 0),
            _nextBallPos + new Vector3(2.2f, 4.5f, 0),
            _nextBallPos + new Vector3(0, 2.6f, 0),
            currentBall.transform.position
        };


        _nextBall.transform.DOPath(wayToNextBall, duration, PathType.CatmullRom).SetEase(Ease.OutCubic);
        _nextBall.transform.DORotate(Vector3.zero, 0.1f);

        _activeBall.transform.DOPath(wayToCurrentBall, duration, PathType.CatmullRom).SetEase(Ease.OutCubic);
        _activeBall.transform.DORotate(Vector3.zero, 0.1f);

    }
    private void CheckBallColorOnField()
    {
        if (!_levelField.BallColorsOnFields.Exists(b => b.Equals(_activeBall.color)))
        {
            _activeBall.SetBall(GetRandomColor());
        }
    }
    private void CheckAvailableBallsCount()
    {
        if (_availableBalls.Count <= 0 && _activeBall == null)
            GameplayEvent.OnGameOver.Invoke();
    }
    private Vector2 CalculateForceForBall()
    {
        Vector2 direction = (Vector2)_activeBall.transform.up;
        float magnitude = 60f;
        return direction.normalized * magnitude;
    }
    public void IncreaseCountBall(int amount)
    {
        countBallLeft += amount;
        countBall.text = countBallLeft.ToString();
        GameplayEvent.OnAvailableBallsCountChanged.Invoke(countBallLeft);
    }

    #region Helper ball
    private void ActiveHelperBall(HelperBtn btn)
    {
      int count =  HelperController.Instance.GetHelperCount(btn.Type);
        if(count > 0)
        {
            isHelperEnable = true;
            isHelperBallShooted = false;
            //Set current active ball as temp ball
            _tempBall = _activeBall;
            //Debug.Log("_tempBall: " + _tempBall.name);

            //Deactive active ball and next ball
            _nextBall.gameObject.SetActive(false);

            _tempBall.gameObject.SetActive(false);

            //Instantiate new helper ball
            GameObject helperBallObj = Instantiate(Getball(btn.Type));
            HelperEvent.OnHelperBallGenerated.Invoke(helperBallObj.GetComponent<Ball>());

            _helperBall = helperBallObj.GetComponent<Ball>();

            _helperBall.gameObject.SetActive(true);
            _activeBall = _helperBall;
            _activeBall.transform.rotation = shooterGFX.transform.rotation;
            GameplayEvent.OnActiveBallChanged.Invoke(_activeBall);
            _activeBall.transform.SetParent(transform);
            _activeBall.transform.position = currentBall.transform.position;
            _activeBall.isActiveBall = true;
            _activeBall.CircleCollider.enabled = false;
            HelperController.Instance.UseHelper(btn.Type);
        }
        
    }

    private void RemoveHelperBall()
    {
        //Debug.Log("Remove helper");
        if (_tempBall && isHelperEnable)
        {
            //Turn on and not shooted
            if (!isHelperBallShooted)
            {
                //Debug.Log("Destroy helper in RemoveHelperBall/Shooter");
                _activeBall.DestroyBall();
            }

            _activeBall = _tempBall;
            _activeBall.transform.rotation = shooterGFX.transform.rotation;
            GameplayEvent.OnActiveBallChanged.Invoke(_activeBall);
            _activeBall.gameObject.SetActive(true);
            _nextBall.gameObject.SetActive(true);
            isHelperEnable = false;
            //Debug.Log("Remove helper done");
        }
    }

    private GameObject Getball(BallType type)
    {
        switch (type)
        {
            case BallType.Bomb:
                {
                    return listHelper.FirstOrDefault(b => b.ballType == BallType.Bomb).gameObject;
                }
            case BallType.Ziczac:
                {
                    return listHelper.FirstOrDefault(b => b.ballType == BallType.Ziczac).gameObject;
                }
            case BallType.Rainbow:
                {
                    return listHelper.FirstOrDefault(b => b.ballType == BallType.Rainbow).gameObject;
                }
            case BallType.Firework:
                {
                    return listHelper.FirstOrDefault(b => b.ballType == BallType.Firework).gameObject;
                }
            default:
                {
                    return null;
                }
        }
    }

    #endregion

    #region OnNoBallOnField

    private bool startShootingBall;

    private void ShootAllBallLeft()
    {
        if (startShootingBall) return;

        startShootingBall = true;
        //Debug.LogError("ShootAllBallLeft");

        List<Ball> leftBallList = new List<Ball>();

        for (int i = 0; i <= countBallLeft; i++)
        {
            Ball ball = Instantiate(_ballPrefab, _availableBallsHolder.transform).SetBall(GetRandomColor(), BallType.Normal);
            ball.gameObject.SetActive(false);
            leftBallList.Add(ball);
        }

        //Debug.LogError("countBallLeft = " + countBallLeft);
        //Debug.LogError("leftBallList = " + leftBallList);

        StartCoroutine(ShootBallLeft(leftBallList));
    }

    private IEnumerator ShootBallLeft(List<Ball> leftBallList)
    {
        yield return new WaitForSeconds(0.3f);

        float delay = (float)2f / leftBallList.Count;

        while (countBallLeft > 0)
        {
            Ball ball = leftBallList[countBallLeft - 1];

            //Debug.Log("Count left " + countBallLeft);

            ball.gameObject.SetActive(true);

            ball.transform.position = currentBall.transform.position;

            float randomX = currentBall.transform.position.x + Random.Range(-7f, 7f);
            float randomY = currentBall.transform.position.y + Random.Range(11f, 22f);
            Vector3 destination = new Vector3(randomX, randomY, 0f);

            ball.PlayWinVFX(destination, delay);

            //Count ball
            countBallLeft--;
            countBall.text = countBallLeft.ToString();

            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(0.3f);

        GameplayEvent.OnGameWin.Invoke();
    }

    #endregion
}
