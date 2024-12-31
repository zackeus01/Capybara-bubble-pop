using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HelperEvent
{
    private readonly static UnityEvent<HelperBtn> _onHelperActivated = new UnityEvent<HelperBtn>();
    private readonly static UnityEvent _onHelperDeactivated = new UnityEvent();
    private readonly static UnityEvent<Ball> _onHelperBallCollided = new UnityEvent<Ball>();
    private readonly static UnityEvent _onHelperBallShooted = new UnityEvent();
    private readonly static UnityEvent<Ball> _onHelperBallGenerated = new UnityEvent<Ball>();
    private readonly static UnityEvent _onAllHelperTargetDestroyed = new UnityEvent();
    public static UnityEvent OnHelperBallShooted { get => _onHelperBallShooted; }
    public static UnityEvent<HelperBtn> OnHelperActivated { get => _onHelperActivated; }
    public static UnityEvent OnHelperDeactivated { get => _onHelperDeactivated; }
    public static UnityEvent<Ball> OnHelperBallCollided { get => _onHelperBallCollided; }
    public static UnityEvent<Ball> OnHelperBallGenerated { get => _onHelperBallGenerated; }

    public static UnityEvent OnAllHelperTargetDestroyed { get => _onAllHelperTargetDestroyed; }
}
