using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Char : MonoBehaviour,IHitable 
{




    const float life = 100f;  
    public float currentlife = 100f;
    public float shieldPower = 0.5f;
    public bool currentPlayer = false;
    float firstBarSize;

    public RawImage lifebar;

    public enum CharState{
          unknown,
          idle,
          walk,
          attack,
          defend,
          hit,
          die
    }
    
    Dictionary<CharState,bool> stateType = new Dictionary<CharState,bool> ()
    {
        {CharState.idle,true},
        {CharState.attack,true},
        {CharState.defend,false},
        {CharState.die,false},
        {CharState.walk,true},
        {CharState.hit,false}

         
    };
     
    Vector3 camtrackOffset = new Vector3(3,7,-11);
    float speed = 3;
    float rotSpeed = 160;
    float gravity = 8;

    float rot = 0;
    Animator anim;
    CharacterController controller;
    Vector3 moveDir = Vector3.zero; 
    
    CharState currentState = CharState.idle;
    
    // Start is called before the first frame update
    Camera m_MainCamera;

    public HitableType GetHitableType(){
        return HitableType.Char;
    }
   
    public CharState state{
        get{
            return this.currentState;
        }
    } 

    void Start()
    {
        m_MainCamera = Camera.main;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        lifebar =  GameObject.FindWithTag("lifeBar").GetComponent<RawImage>();
        firstBarSize = lifebar.GetComponent<RectTransform>().rect.width;
        
    }
    

    void setState(CharState state){
        
        if(this.currentState != state){
            //print(state.ToString()+"---"+currentState.ToString());
            if(stateType[this.currentState]){
                anim.SetFloat(this.currentState.ToString(),0f);
            }
            this.currentState = state;
            if(stateType[this.currentState]){
                anim.SetFloat(this.currentState.ToString(),1f);
            }else{
                anim.SetTrigger(this.currentState.ToString());
            }
        }

    }
    void idle(){
       // print("idle");
        moveDir = new Vector3(0,0,0);
        this.setState(CharState.idle);
        Vector3 pos =this.transform.position;
        pos.y = 0f;
        this.transform.position = pos;

    }

    void walk(){
        moveDir = new Vector3(0,0,1);
        //print("walk");
        this.setState(CharState.walk);
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
    }
    
    void attack(){
        this.setState(CharState.attack);

    }

    void defend(){
        
        this.setState(CharState.defend);
        
    }
    
    public void onHit(Weapon weapon){
        print("On hit");
        this.hit(weapon.force);
    }
    void hit(float hitForce){
        if(this.currentState == CharState.hit){
            return;
        }
        this.currentlife -= hitForce;
        this.currentlife = this.currentlife < 0 ? 0:this.currentlife;
        float percentOfLife = this.currentlife / life;
        
        RectTransform rt = lifebar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(firstBarSize * percentOfLife ,rt.sizeDelta.y);
        this.setState(this.currentlife > 0 ? CharState.hit: CharState.die);
        

    }


    void die(){
        
        this.setState(CharState.die);
        
    }

    void setRotation(){
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0,rot,0);
        moveDir.y -= gravity *Time.deltaTime;
        
         
    }
    public void defended(int value){
       // print("DefendComplete");
         this.setState(CharState.idle);

    }
    public void HitCompleted(){
        //print("HitCompleted");
        this.setState(CharState.idle);
    }

    private void OnCollisionEnter(Collision other){
        if(other.contacts.Length > 0){
            Collider myCollider = other.contacts[0].thisCollider;
            //print("----->>"+myCollider.gameObject.name+" -- "+other.gameObject.name);
          
            if(other.gameObject.transform.parent!=null){
                if(myCollider.gameObject.name != other.gameObject.transform.parent.gameObject.transform.parent.gameObject.name ){

                }
                //print("----->>"+myCollider.gameObject.name+" -- "+other.gameObject.name+"--sword parent-"+other.gameObject.transform.parent.gameObject.transform.parent.gameObject.name);
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //print("----->>>"+other.gameObject.tag);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(currentState != CharState.die && currentPlayer ){
           if(Input.GetKey(KeyCode.X)){
               attack();
           }else if(Input.GetKey(KeyCode.W)){
               walk();
           }else if(Input.GetKey(KeyCode.Space)){
               hit(1f);
           }else if(Input.GetKey(KeyCode.C)){
               defend();
           }else if(Input.GetKeyUp(KeyCode.C)||Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.X)){
               idle();
           }
           controller.Move(moveDir * Time.deltaTime); 
          // m_MainCamera.transform.position = this.transform.position + camtrackOffset;
          if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)){
              setRotation();
          }

           
         }
         
         
    }
}
