﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour
{

    //user input
    //will be filled with json data
    string[] intervalActions;
    string[] touchActions;
    //
    public float Tuli;
    Boid boid;

    bool _attached;
    public bool attached
    {
        get { return _attached; }
        set
        {
            _attached = value;
            if (value)
            {
                ThingGod.god.RemoveFromFlock(this);
            }
            else
            {
                ThingGod.god.AddToFlock(this);
                var joint = GetComponent<Joint>();
                if (joint != null)
                {
                    joint.connectedBody = null;
                    Destroy(joint);
                }
            }
        }
    }
    bool _dead;
    public bool dead
    {
        get { return _dead; }
        set
        {
            _dead = value;
            if (value)
            {
                ThingGod.god.RemoveFromFlock(this);
            }
            else
            {
                ThingGod.god.AddToFlock(this);
            }
        }
    }

    public Bounds bounds
    {
        get { return GetComponent<Renderer>().bounds; }
    }

    public void ResetFlags()
    {
        dead = false;
        attached = false;
    }

    public void Steal(Thing another)
    {
        Debug.Log(name + " steal " + another.name);
        Tuli += another.Tuli;
        another.Tuli = 0;
        ThingGod.god.FireStealParticle(transform.position);
    }

    public void Gift(Thing another)
    {
        another.Tuli += Tuli;
        Tuli = 0;
        Debug.Log(name + " gift " + another.name);
        ThingGod.god.FireGiftingParticle(transform.position);
        //Gifting, voluntarily transform one’s own Tulis to others;
    }

    public void Stick(Thing another)
    {
        Debug.Log(name + " stick " + another.name);
        //Follow, attach onto another thing for a limited period of time;
        transform.position = another.transform.position + another.transform.GetComponent<Collider>().bounds.extents.x * (transform.position - another.transform.position).normalized;
        boid.rb.velocity = Vector3.zero;
        //create a new joint to connect
        var myJoint = gameObject.AddComponent<CharacterJoint>();
        //connect to rb
        myJoint.connectedBody = another.boid.rb;

        Debug.Log("Sticking.");
        //release        
        Invoke("ReleaseSticking", 10);
    }

    public void Clone(Thing another)
    {
        Debug.Log(name + " mate " + another.name);
        ThingGod.god.CloneThing(another, transform.position, another.transform.localScale);
        //Mate, give birth to a baby that resembles other thing;
    }

    public void Erase(Thing another)
    {
        Debug.Log(name + " kill " + another.name);
        StartCoroutine(ThingGod.god.EraseThingEnum(another));

        //Kill, destroy other thing and force it to be reborn at the monolith;
    }

    public void Group()
    {
        boid.cohWeight *= 3f;
        boid.aliWeight *= 3f;
        boid.seekWeight /= 3f;
    }

    public void Hide()
    {
        boid.cohWeight /= 3f;
        boid.aliWeight /= 3f;
        boid.seekWeight *= 3f;
    }

    public void Seek(Thing another)
    {
        //Aim, walk towards the direction of a shan, an er, a monolith, or the tuli mountain. 
        boid.target = another.transform;
        Invoke("ReleaseTarget", 15f);
    }


    void ReleaseSticking()
    {
        attached = false;
    }

    void ReleaseTarget()
    {
        boid.target = null;
    }

    // Start is called before the first frame update
    void Start()
    {

        boid = gameObject.AddComponent<Boid>();
        var meetDetector = gameObject.AddComponent<SphereCollider>();
        meetDetector.isTrigger = true;
        meetDetector.radius = 1;

        InvokeRepeating("IntervalBasedActions", 5, 5);
    }


    IEnumerator IntervalBasedActions()
    {
        while (true)
        {
            if (!dead && !attached)
            {
                foreach (var func in intervalActions)
                {
                    Invoke(func, 0);
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

//TODO: pass action to that Thing I touched
    void OnTouch()
    {
        foreach (var func in touchActions)
        {
            Invoke(func, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        boid.inEffect = !attached;
    }

    void OnCollisionEnter(Collision collision)
    {
        //TODO
        // foreach (ContactPoint contact in collision.contacts)
        // {
        //     Debug.DrawRay(contact.point, contact.normal, Color.white);
        // }
    }
}