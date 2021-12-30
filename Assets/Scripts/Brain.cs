using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNALength = 2; //2 decisions to be made (when you see and when you don't see platform)
    public float timeAlive; //fitness func based on how long we can stay alive
    public float timeWalking; //make sure that they don't stay in one place and turn
    public DNA dna;
    public GameObject eyes;// use eyes to determine if we are on platform or not
    private bool alive = true;
    private bool canSeeGround = true;

    public GameObject ethanPrefab;
    GameObject ethan;

    void OnDestroy()
    {
        Destroy(ethan);
    }

    void OnCollisionEnter(Collision obj)
    {
        //collide with black cube and you are dead
        Debug.Log("off ground");
        if(obj.gameObject.tag == "dead")
        {
            alive = false;
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void Init()
    {
        //dna length = 2, max value of genes = 3
        // (if see platform)0/1/2 || (if can't see platform)0/1/2
        //initlialize dna
        //0 --> forward
        //1 --> left
        //2 --> right
        dna = new DNA(DNALength, 3);
        timeAlive = 0;
        alive = true;
        ethan = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
        ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
    }

    void Update()
    {
        if(!alive) return; //if bot is dead then don't do anything

        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        canSeeGround = false;
        RaycastHit hit;
        if(Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if(hit.collider.gameObject.tag == "platform")
            {
                canSeeGround = true;
            }
        }
        timeAlive = PopulationManager.elapsed;

        //read DNA
        float turn = 0; //turn left or right
        float move = 0; //forward
        if(canSeeGround)
        {
            //turn character appropriately and always move forward 
            if(dna.GetGene(0) == 0) {move = 1; timeWalking += 1;} //timewalking happens only when moving
            else if(dna.GetGene(0) == 1) turn = -90;
            else if(dna.GetGene(0) == 2) turn = 90;
        }
        else
        {
            if(dna.GetGene(1) == 0) {move = 1; timeWalking += 1;}
            else if(dna.GetGene(1) == 1) turn = -90;
            else if(dna.GetGene(1) == 2) turn = 90;
        }

        this.transform.Rotate(0f, turn, 0f);
        this.transform.Translate(0f, 0f, move * Time.deltaTime);
    }
}
