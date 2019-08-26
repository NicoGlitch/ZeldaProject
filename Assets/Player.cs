using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private enum Direction { North, East, South, West };
    private Direction playerDirection = Direction.South;
    private Animator animator;
    public float maxSpeed = 7;
    private Vector2 targetVelocity;
    private Rigidbody2D playerRigidbody;
    private ContactFilter2D mouvementContactFilter;

    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;
    private bool fireIsPress = false;
    private bool controleAreEnabled = true;
    // Use this for initialization
    void Start() {
        mouvementContactFilter = BuildContactFilter2DForLayer(LayerMask.LayerToName(gameObject.layer));
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (controleAreEnabled)
        {
            ProcessInput();
        }
        Vector2 velocityX = new Vector2();
        Vector2 velocityY = new Vector2();

        velocityX.x = targetVelocity.x;
        velocityY.y = targetVelocity.y;
        Vector2 deltaPositionX = velocityX * Time.deltaTime;
        Mouvement(deltaPositionX);
        Vector2 deltaPositionY = velocityY * Time.deltaTime;
        Mouvement(deltaPositionY);

    }
    public void ProcessInput() {
        ComputeVelocity();

    }
    public void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        targetVelocity = move.normalized * maxSpeed;
        UpdateDirectiion(move.x, move.y);
        UpdateAnimationSpeed(targetVelocity.magnitude);
    }

    private void UpdateDirectiion(float mouvementX, float mouvementY)
    {

    }

    private void UpdateAnimationSpeed(float speed)
    {

    }

    private void Mouvement(Vector2 move)
    {
        float distance = move.magnitude;
        RaycastHit2D[] hitbuffer = new RaycastHit2D[16];
        if (distance > minMoveDistance)
        {
            int movementCollisionHit = playerRigidbody.Cast(move, mouvementContactFilter,hitbuffer,distance + shellRadius);
            List<RaycastHit2D> hitBufferList = BufferArrayHitList(hitbuffer, movementCollisionHit);
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currenNormal = hitBufferList[i].normal;
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        playerRigidbody.position = playerRigidbody.position + move.normalized * distance;
    }



    private ContactFilter2D BuildContactFilter2DForLayer(string layerName)
    {
        ContactFilter2D contactFilter2d = new ContactFilter2D();
        contactFilter2d.useTriggers = false;
        contactFilter2d.SetLayerMask(Physics2D.GetLayerCollisionMask(LayerMask.NameToLayer(layerName)));
        contactFilter2d.useLayerMask = true;
        return contactFilter2d;
    }

    private List<RaycastHit2D> BufferArrayHitList(RaycastHit2D[] hitBuffer, int count)
    {
        List<RaycastHit2D> hitBufferl = new List<RaycastHit2D>(count);
        hitBufferl.Clear();
        for(int i =0; i < count; i++)
        {
            hitBufferl.Add(hitBuffer[i]);
        }
        return hitBufferl;
    }


}
