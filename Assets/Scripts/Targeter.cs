using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter :NetworkBehaviour
{
    [SerializeField] private Targetable target;

    #region Server
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if(!targetGameObject.TryGetComponent<Targetable>(out Targetable newTarget))
        {
            return;
        }

        target = newTarget;
    }
    #endregion

    #region Client
    public void ClearTarget()
    {
        target = null;
    }
    #endregion
   

}
