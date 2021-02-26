using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySetGoNow : MonoBehaviour
{
    private ReadySetGoManagerScript RSGMS;

    public void SetReadySetGoNow ()
    {
        RSGMS = GameObject.Find("ReadySetGoManager").GetComponent<ReadySetGoManagerScript>();
        RSGMS.ReadySetGoDone = true;
    }
}
