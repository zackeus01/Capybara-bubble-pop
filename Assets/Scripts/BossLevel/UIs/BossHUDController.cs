using UnityEngine;

public class BossHUDController : MonoBehaviour
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
        BossUIEvent.OnBossHealthSetUp.RemoveListener(healthBarController.SetupHealthBar);

        BossUIEvent.OnBossHealthUpdate.RemoveListener(healthBarController.UpdateHealthBar);

        BossUIEvent.OnBossShieldSetUp.RemoveListener(shieldBarController.SetupShieldBar);

        BossUIEvent.OnBossShieldUpdate.RemoveListener(shieldBarController.UpdateShieldBar);

        BossUIEvent.OnBossManaSetUp.RemoveListener(manaBarController.SetupManaBar);

        BossUIEvent.OnBossManaUpdate.RemoveListener(manaBarController.UpdateManaBar);
    }

    private void AddEventListener()
    {
        BossUIEvent.OnBossHealthSetUp.AddListener(healthBarController.SetupHealthBar);

        BossUIEvent.OnBossHealthUpdate.AddListener(healthBarController.UpdateHealthBar);

        BossUIEvent.OnBossShieldSetUp.AddListener(shieldBarController.SetupShieldBar);

        BossUIEvent.OnBossShieldUpdate.AddListener(shieldBarController.UpdateShieldBar);

        BossUIEvent.OnBossManaSetUp.AddListener(manaBarController.SetupManaBar);

        BossUIEvent.OnBossManaUpdate.AddListener(manaBarController.UpdateManaBar);
    }

    private void LoadComponent()
    {
        healthBarController = GetComponentInChildren<HealthBarController>();
        shieldBarController = GetComponentInChildren<ShieldBarController>();
        manaBarController = GetComponentInChildren<ManaBarController>();
    }
}
