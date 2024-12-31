using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UIReturnToMenu : UIChangeScene
{
    protected override void EventSubscribe()
    {
        base.EventSubscribe();
        ChangeSceneButton.onClick.AddListener(ClickRestart);
    }

    protected override void EventUnsubscribe()
    {
        base.EventUnsubscribe();
        ChangeSceneButton.onClick.AddListener(ClickRestart);
    }

    public void ClickRestart()
    {
        UIEvent.OnClickRestart.Invoke();
    }
}
