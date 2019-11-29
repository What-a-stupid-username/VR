using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Saber : MonoBehaviour
{
    public Emitter emitter;


    // Start is called before the first frame update
    void Awake()
    {
        pre_pos = transform.position;
        v = pre_v = Vector3.zero;
        if (emitter == null)
        {
            emitter = GameObject.FindWithTag("Emitter").GetComponent<Emitter>();
        }
    }

    public Vector3 pre_pos;
    public Vector3 pre_v;
    public Vector3 v;
    void Update()
    {
        v = (transform.position - pre_pos) / Mathf.Max(Time.deltaTime, 0.00001f);
        v = pre_v * 0.9f + v * 0.1f;
        pre_v = v;
        pre_pos = transform.position;
        emitter.SetSaberInfo(transform.position - transform.lossyScale.y * transform.up / 2, transform.up * transform.lossyScale.y, v);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Particle")
        {
            var par = other.GetComponent<Particle>();
            par.Clip();            
        }
    }
}
