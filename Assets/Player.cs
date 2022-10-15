using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField,Range(0.01f,1f)] float moveDuration=0.2f;
    [SerializeField,Range(0.01f,1f)] float jumpHeight=0.5f;
    [SerializeField] private int maxTravel;
    [SerializeField] private int currentTravel;
    
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    public int MaxTravel {get => maxTravel;}
    public int CurrentTravel {get => currentTravel;}
    public bool IsDie {get => this.enabled == false;}

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos -1;
        leftBoundary = -(extent +1);
        rightBoundary= extent +1;
    }
    
    private void Update()
    {
       // if(Input.GetKeyDown(KeyCode.UpArrow))
       //     Debug.Log("forward");
       // if(Input.GetKeyDown(KeyCode.DownArrow))
       //     Debug.Log("back");

       var moveDir = Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow))
            moveDir += new Vector3(0, 0, 1);

        if(Input.GetKey(KeyCode.DownArrow))
            moveDir += new Vector3(0, 0, -1);
        
        if(Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector3(-1, 0, 0);

        if(Input.GetKey(KeyCode.RightArrow))
            moveDir += new Vector3(1, 0, 0);

        if(moveDir != Vector3.zero && IsJumping()==false)
            Jump(moveDir);  
    }
    private void Jump(Vector3 targetDirection)
    {
        //var TargetPosition = transform.position + new Vector3(x: dir.x,y: 0,z: dir.y);
    
        // transform.DOMoveY(0.5f, 0.1f).OnComplete(() =>transform.DOMoveY(0, 0.1f));
        var targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);

        var moveSeq =  DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if (targetPosition.z <= backBoundary || targetPosition.x <= leftBoundary ||
            targetPosition.x >= rightBoundary)
            return;

        if (Tree.AllPositions.Contains(targetPosition))
            return;

        // Bergerak
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration).OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int) this.transform.position.z;
        if(currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "POINT : "+ maxTravel.ToString();
    }
    
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.enabled==false)
            return;
            
        var car = other.GetComponent<Car>();
        if(car != null)
        {
            AnimateCrash();
        }
        // if(other.tag == "Car")
        // {

        // }
    }
    
    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void AnimateCrash()
    {
       // Animasi kelempar
        //var isRight = car.transform.rotation.y == 90;
        //transform.DOMoveX(isRight ? 3 : -3, 0.2f);
        //transform.DORotate(Vector3.forward*360,0.2f).SetLoops(-1,LoopType.Restart);

        // Animasi Gepeng
        transform.DOScaleY(0.1f,0.2f);
        transform.DOScaleX(3,0.2f);
        transform.DOScaleZ(2,0.2f);

        this.enabled = false;
        dieParticles.Play();
    }

}
