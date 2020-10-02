using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update

   
    public Char owner = null;
    public float force = 0;
    
    private void OnCollisionEnter(Collision other){
           GameObject current = other.contacts[0].thisCollider.gameObject;
           Char _owner = null;
           if(current.GetComponent<Weapon>()!=null){
               _owner = current.GetComponent<Weapon>().owner;
                GameObject target = other.gameObject;
                print("owner "+_owner +"---"+target.name +"-----"+current.name); 
                if(_owner != null){
                    if(_owner.state != Char.CharState.attack){
                        return;
                    }
                }
                IHitable hitable = target.GetComponent<IHitable>();
                if(hitable != null){
                    hitable.onHit(this);
                }
           }
          

    }
    


   
}
