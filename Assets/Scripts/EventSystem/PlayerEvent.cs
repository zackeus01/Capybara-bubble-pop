using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvent
{
    private readonly static UnityEvent<List<Ball>> _onBallAreaDestroyed = new UnityEvent<List<Ball>>();
    public static UnityEvent<List<Ball>> OnBallAreaDestroyed { get => _onBallAreaDestroyed; }

}
