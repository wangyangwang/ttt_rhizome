﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(Boid))]
public class Thing : MonoBehaviour
{

    //user input
    //will be filled with json data
    public string[] intervalActions = new string[] { };
    public string[] touchActions = new string[] { };
    //
    public float Tuli;
    public Boid boid
    {
        get { return GetComponent<Boid>(); }
    }

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


    public Dictionary<Thing, int> fRecord = new Dictionary<Thing, int>();

    public Bounds bounds
    {
        get { return GetComponent<Renderer>().bounds; }
    }


    //MONOBEHAVIOUR///////////////////////////////////////
    void Start()
    {

        /*
        var triggerSphere = GetComponent<SphereCollider>();        
        if (triggerSphere == null) triggerSphere = gameObject.AddComponent<SphereCollider>();
        triggerSphere.isTrigger = true;
        triggerSphere.radius = 4;
        */


        StartCoroutine(IntervalBasedActions());
    }

    void Update()
    {
        boid.inEffect = !attached;


    }

    //MONOBEHAVIOUR///////////////////////////////////////


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

        if (ThingGod.StealEvent != null) ThingGod.StealEvent(this, another);
    }

    public void Gift(Thing another)
    {
        another.Tuli += Tuli;
        Tuli = 0;
        Debug.Log(name + " gift " + another.name);
        if (ThingGod.GiftingEvent != null) ThingGod.GiftingEvent(this, another);
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
        if (ThingGod.StickEvent != null) ThingGod.StickEvent(this, another);
        Debug.Log("Sticking.");
        //release        
        Invoke("ReleaseSticking", 10);
    }

    public void Clone(Thing another)
    {
        Debug.Log(name + " mate " + another.name);
        ThingGod.god.CloneThing(another, transform.position, another.transform.localScale * 0.9f);
        if (ThingGod.CloneEvent != null) ThingGod.CloneEvent(this, another);
        //Mate, give birth to a baby that resembles other thing;
    }

    public void Erase(Thing another)
    {
        Debug.Log(name + " kill " + another.name);
        ThingGod.god.TryErase(another);
        if (ThingGod.EraseEvent != null) ThingGod.EraseEvent(this, another);
    }

    public void Group(Thing another)
    {
        boid.cohWeight *= 3f;
        boid.aliWeight *= 3f;
        boid.seekWeight /= 3f;
    }

    public void Hide(Thing another)
    {
        boid.cohWeight /= 3f;
        boid.aliWeight /= 3f;
        boid.seekWeight *= 3f;
    }

    public void Seek(Thing another)
    {
        //Aim, walk towards the direction of a shan, an er, a monolith, or the tuli mountain. 
        boid.target = another.transform;
        if (ThingGod.SeekEvent != null) ThingGod.SeekEvent(this, another);
        Invoke("ReleaseTarget", 15f);
    }

    public void DecreaseScore(int n, Thing who)
    {
        if (fRecord.ContainsKey(who))
        {
            fRecord[who] -= n;
        }
        else
        {
            fRecord[who] = -n;
        }
    }
    public void IncreaseScore(int n, Thing who)
    {
        if (fRecord.ContainsKey(who))
        {
            fRecord[who] += n;
        }
        else
        {
            fRecord[who] = n;
        }
    }

    void ReleaseSticking()
    {
        attached = false;
    }

    void ReleaseTarget()
    {
        boid.target = null;
    }

    Thing GetClosestThing(Thing center)
    {
        foreach (var thing in ThingGod.god.things)
        {
            if (thing == center) continue;
            if (Vector3.Distance(thing.transform.position, center.transform.position) < 10)
            {
                return thing;
            }
        }

        return null;
    }

    IEnumerator IntervalBasedActions()
    {
        while (true)
        {
            if (!dead && !attached)
            {
                foreach (var func in intervalActions)
                {
                    if (string.IsNullOrWhiteSpace(func)) continue;
                    Thing closestThing = GetClosestThing(this);
                    MethodInfo mi = this.GetType().GetMethod(func);
                    mi.Invoke(this, new object[] { closestThing });
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    // Update is called once per frame


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Thing>())
        {
            Thing other = collision.gameObject.GetComponent<Thing>();
            foreach (var func in touchActions)
            {
                MethodInfo mi = this.GetType().GetMethod(func);
                mi.Invoke(this, new object[] { other });
            }
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (fRecord.ContainsKey(other.GetComponent<Thing>()))
    //     {

    //     }

    // }
}
