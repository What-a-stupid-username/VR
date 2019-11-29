using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Saber : MonoBehaviour
{
    public VisualEffect particle;

    public float DestroyAfter = 2;

    // Start is called before the first frame update
    void Awake()
    {
        pre_pos = transform.position;
        v = pre_v = Vector3.zero;
    }

    public Vector3 pre_pos;
    public Vector3 pre_v;
    public Vector3 v;
    void Update()
    {
        v = (transform.position - pre_pos)/ Time.deltaTime;
        v = pre_v * 0.9f + v * 0.1f;
        pre_v = v;
        pre_pos = transform.position;
        if (particle != null)
        {
            particle.SetVector3("SaberPos", transform.position - transform.lossyScale.y * transform.up / 2);
            particle.SetVector3("SaberDirection", transform.up * transform.lossyScale.y);
            particle.SetVector3("SaberVelocity", v);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Particle")
        {
            Debug.Log("Clip");
            particle.SetInt("isClip", 1);
            particle.GetComponent<Collider>().enabled = false;
            StartCoroutine(CancelForce(particle));
        }
    }

    IEnumerator CancelForce(VisualEffect vfx)
    {
        float remainTime = DestroyAfter;
        yield return null;
        while (true)
        {
            remainTime -= Time.deltaTime;
            if (remainTime < 0)
            {
                Destroy(vfx.gameObject);
                yield break;
            }
            vfx.SetFloat("RemainTime", remainTime / DestroyAfter);
            yield return null;
        }
    }
}
