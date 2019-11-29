using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public GameObject ParticlePrefab;
    public AnimationCurve BeatCurve;

    List<Particle> particles;



    Vector3 pos_, dir_, vel_;

    // Start is called before the first frame update
    void Start()
    {
        particles = new List<Particle>();
        null_particles = new List<Particle>();
        particle_pool = new HashSet<Particle>();
        List<Particle> tmp_par = new List<Particle>();
        for (int i = 0; i < 5; i++)
        {
            tmp_par.Add(GetParticle());
        }
        foreach (var par in tmp_par)
        {
            par.DoDestroy();
        }
        actived = new List<bool>(BeatCurve.keys.Length);
        for (int i = 0; i < BeatCurve.keys.Length; i++)
        {
            actived.Add(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        TryEmmit();
        SetupParticles();
    }


    List<bool> actived;
    float time = 0;
    int index = 0;
    void TryEmmit()
    {
        while (index < BeatCurve.keys.Length && (float)BeatCurve.keys[index].time <= time)
        {
            if (!actived[index])
            {
                Emmit(index);
                index++;
            }
        }
        time += Time.deltaTime;
    }

    void Emmit(int index)
    {
        float value = BeatCurve.keys[index].value;
        {
            Particle par = GetParticle();
            par.index = index;
            particles.Add(par);
            // deal with particle's position and movement
            {
                // 瞎几把写的
                par.transform.position = new Vector3(0, value, 0);
            }
        }
        actived[index] = true;
    }


    List<Particle> null_particles;
    void SetupParticles()
    {
        null_particles.Clear();
        foreach (var par in particles)
        {
            if (par == null)
            {
                null_particles.Add(par);
            }
            else
            {
                par.SetSaberInfo(pos_, dir_, vel_);
            }
        }
        foreach (var par in null_particles)
        {
            particles.Remove(par);
        }
    }
    
    public void SetSaberInfo(Vector3 pos, Vector3 dir, Vector3 vel)
    {
        pos_ = pos;
        dir_ = dir;
        vel_ = vel;
    }

    HashSet<Particle> particle_pool;
    public void MarkReuse(Particle par)
    {
        if (particles.Contains(par))
            particles.Remove(par);
        particle_pool.Add(par);
    }

    public void Cliped(Particle par)
    {
        // deal with cliped particle
        Debug.Log("Cliped" + par.index);
    }

    Particle GetParticle()
    {
        if (particle_pool.Count != 0)
        {
            var enu = particle_pool.GetEnumerator();
            enu.MoveNext();
            var par = enu.Current;
            particle_pool.Remove(par);
            par.DoInit();
            return par;
        }
        else
        {
            GameObject go = GameObject.Instantiate(ParticlePrefab) as GameObject;
            var par = go.GetComponent<Particle>();
            go.transform.parent = transform;
            par.emitter = this;
            return par;
        }
    }
}
