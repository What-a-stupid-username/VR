using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class Particle : MonoBehaviour
{
    public float DestroyAfter = 2;
    VisualEffect vfx;
    Collider col;

    public Emitter emitter;
    public int index;

    // Start is called before the first frame update
    void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        col = GetComponent<Collider>();
    }

    Vector3 pos_, dir_, vel_;
    // Update is called once per frame
    void Update()
    {
        vfx.SetVector3("SaberPos", pos_);
        vfx.SetVector3("SaberDirection", dir_);
        vfx.SetVector3("SaberVelocity", vel_);
    }

    public void DoDestroy()
    {
        vfx.enabled = false;
        emitter.MarkReuse(this);
    }

    public void DoInit()
    {
        vfx.SetInt("isClip", 0);
        vfx.SetFloat("RemainTime", 1);
        col.enabled = true;
        vfx.Reinit();
        vfx.enabled = true;
    }

    public void SetSaberInfo(Vector3 pos, Vector3 dir, Vector3 vel)
    {
        pos_ = pos;
        dir_ = dir;
        vel_ = vel;
    }

    public void Clip()
    {
        vfx.SetInt("isClip", 1);
        col.enabled = false;
        emitter.Cliped(this);
        StartCoroutine(CancelForce());
    }
    IEnumerator CancelForce()
    {
        float remainTime = DestroyAfter;
        yield return null;
        while (true)
        {
            remainTime -= Time.deltaTime;
            if (remainTime < 0)
            {
                DoDestroy();
                yield break;
            }
            vfx.SetFloat("RemainTime", remainTime / DestroyAfter);
            yield return null;
        }
    }
}
