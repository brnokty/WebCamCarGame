using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : Panel
{
    public override void WhenAppearFinished()
    {
        base.WhenAppearFinished();
        EventRunner.Instance.SavePhotos();
    }
}
