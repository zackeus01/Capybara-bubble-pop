using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    #region Components
    [SerializeField]
    private HealthBarController healthBarController;
    [SerializeField]
    private ShieldBarController shieldBarController;
    [SerializeField]
    private ManaBarController manaBarController;
    #endregion

    private void Reset()
    {
        this.LoadComponent();
    }

    private void Awake()
    {
        this.AddEventListener();
    }

    private void OnDisable()
    {
        this.RemoveEventListener();
    }

    private void RemoveEventListener()
    {
        BossUIEvent.OnPlayerHealthSetUp.RemoveListener(healthBarController.SetupHealthBar);

        BossUIEvent.OnPlayerHealthUpdate.RemoveListener(healthBarController.UpdateHealthBar);

        BossUIEvent.OnPlayerShieldSetUp.RemoveListener(shieldBarController.SetupShieldBar);

        BossUIEvent.OnPlayerShieldUpdate.RemoveListener(shieldBarController.UpdateShieldBar);

        BossUIEvent.OnPlayerManaSetUp.RemoveListener(manaBarController.SetupManaBar);

        BossUIEvent.OnPlayerManaUpdate.RemoveListener(manaBarController.UpdateManaBar);
    }

    private void AddEventListener()
    {
        BossUIEvent.OnPlayerHealthSetUp.AddListener(healthBarController.SetupHealthBar);

        BossUIEvent.OnPlayerHealthUpdate.AddListener(healthBarController.UpdateHealthBar);

        BossUIEvent.OnPlayerShieldSetUp.AddListener(shieldBarController.SetupShieldBar);

        BossUIEvent.OnPlayerShieldUpdate.AddListener(shieldBarController.UpdateShieldBar);

        BossUIEvent.OnPlayerManaSetUp.AddListener(manaBarController.SetupManaBar);

        BossUIEvent.OnPlayerManaUpdate.AddListener(manaBarController.UpdateManaBar);
    }

    private void LoadComponent()
    {
        healthBarController = GetComponentInChildren<HealthBarController>();
        shieldBarController = GetComponentInChildren<ShieldBarController>();
        manaBarController = GetComponentInChildren<ManaBarController>();
    }
}
