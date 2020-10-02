using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public interface IHitable
{
    HitableType GetHitableType();
    void onHit(Weapon weapon);
    

    // Start is called before the first frame update
    
}
