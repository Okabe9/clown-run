using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public int force = 1000; 
    public int jumpforce = 1500; 
    public bool grounded = true; 
    private Rigidbody2D rb; 
    private float inputBufferTimer = 1f; 
    private float coyoteTime = 0.2f; 
    private float coyoteTimer = 0.0f; 
    private bool coyoteMark = true;
    private int jumps = 1; 
    private int currentJumps = 0;  
    [SerializeField] LayerMask capaSuelo; 
    RaycastHit2D raycastSuelo; 
    Queue<KeyCode> inputBuffer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        inputBuffer = new Queue<KeyCode>();
    }

    // Update is called once per frame
    void Update()
    {
        coyoteController(); 
        move();
    }
    public void move(){
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * force, rb.velocity.y);
        raycastSuelo = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, capaSuelo); 
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.JoystickButton0)){
            Debug.Log("Entro"); 
            inputBuffer.Enqueue(KeyCode.W); 
            Invoke("eraseAction", inputBufferTimer); 
        }
        if ((Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.JoystickButton0)|| inputBuffer.Count >0) && currentJumps > 0){
            
            if(grounded || coyoteMark){
                currentJumps -= 1; 
                grounded = false; 
                rb.velocity = new Vector2(rb.velocity.x, jumpforce); 
                if (inputBuffer.Count > 0){
                    inputBuffer.Dequeue(); 
                }
            }
          
        }
    }
    public void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag == "Floor"){
            grounded = true; 
            currentJumps = jumps; 
            coyoteTimer = 0.0f; 
        }
    }
    public void coyoteController(){
        if (raycastSuelo == false){
            grounded = false; 
            coyoteTimer += Time.deltaTime; 
            if (coyoteTimer < coyoteTime){
                coyoteMark = true; 
            }
            else{
                coyoteMark = false;
            }
        }
    }
    void eraseAction(){
        if (inputBuffer.Count > 0){
            inputBuffer.Dequeue(); 
        }
    }
}
