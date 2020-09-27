using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnigthBlue : MonoBehaviour
{



    const float life = 100f;  
    public float currentlife = 100f;
    public float shieldPower = 0.5f;
    
    float firstBarSize;

    public RawImage lifebar;

    public enum footmanState{
          unknown,
          idle,
          walk,
          attack,
          defend,
          hit,
          die
    }
    
    Dictionary<footmanState,bool> stateType = new Dictionary<footmanState,bool> ()
    {
        {footmanState.idle,true},
        {footmanState.attack,true},
        {footmanState.defend,false},
        {footmanState.die,false},
        {footmanState.walk,true},
        {footmanState.hit,false}

         
    };
     
    Vector3 camtrackOffset = new Vector3(3,7,-11);
    float speed = 3;
    float rotSpeed = 160;
    float gravity = 8;

    float rot = 0;
    Animator anim;
    CharacterController controller;
    Vector3 moveDir = Vector3.zero; 
    
    footmanState currentState = footmanState.idle;
    
    // Start is called before the first frame update
    Camera m_MainCamera;
    void Start()
    {
        m_MainCamera = Camera.main;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        lifebar =  GameObject.FindWithTag("lifeBar").GetComponent<RawImage>();
        firstBarSize = lifebar.GetComponent<RectTransform>().rect.width;
    }
    

    void setState(footmanState state){
        
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
        this.setState(footmanState.idle);
        Vector3 pos =this.transform.position;
        pos.y = 0f;
        this.transform.position = pos;

    }

    void walk(){
        moveDir = new Vector3(0,0,1);
        //print("walk");
        this.setState(footmanState.walk);
        moveDir *= speed;
        moveDir = transform.TransformDirection(moveDir);
    }
    
    void attack(){
        this.setState(footmanState.attack);

    }

    void defend(){
        
        this.setState(footmanState.defend);
        
    }

    void hit(float hitForce){
        if(this.currentState == footmanState.hit){
            return;
        }
        this.currentlife -= hitForce;
        this.currentlife = this.currentlife < 0 ? 0:this.currentlife;
        float percentOfLife = this.currentlife / life;
        
        RectTransform rt = lifebar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(firstBarSize * percentOfLife ,rt.sizeDelta.y);
        this.setState(this.currentlife > 0 ? footmanState.hit: footmanState.die);
        

    }
    void die(){
        
        this.setState(footmanState.die);
        
    }

    void setRotation(){
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0,rot,0);
        moveDir.y -= gravity *Time.deltaTime;
        
         
    }
    public void defended(int value){
       // print("DefendComplete");
         this.setState(footmanState.idle);

    }
    public void HitCompleted(){
        //print("HitCompleted");
        this.setState(footmanState.idle);
    }

    private void OnCollisionEnter(Collision other){
        Collider myCollider = other.contacts[0].thisCollider;
        print("----->>"+myCollider.gameObject.name+" -- "+other.gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("----->>>"+other.gameObject.tag);
    }
    // Update is called once per frame
    void Update()
    {
        
        if(currentState != footmanState.die){
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
