using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ATTALA
{
    public class PoolingManager : MonoBehaviour
    {
        public static PoolingManager PM;
        private List<GameObject> Objecttypes = new List<GameObject>();
        private List<List<GameObject>> ReusableObjects = new List<List<GameObject>>();

        void Awake()
        {
            //GameObject.DontDestroyOnLoad(gameObject);
            if (PM == null)
                PM = this;
        }

        /// <summary>
        /// Spawns, pools and returns a gameobject with a particle system on a parent transform. The object can have children with more particle systems.
        /// The disbableafter decides if the object should despawn by itself. Pass NULL as parentTransform if the object should not have a parent.
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="ObjName"></param>
        /// <param name="localpos"></param>
        /// <param name="rot"></param>
        public GameObject SpawnParticleObject(Transform parentTransform, GameObject Obj, Vector3 localpos, Quaternion rot, bool disableafter)
        {
            GameObject E = GetObject(Obj);
            E.transform.localPosition = localpos;
            E.transform.rotation = rot;
            if(parentTransform != null)
                E.transform.SetParent(parentTransform);
            if (!E.activeInHierarchy)
                E.SetActive(true);
            ResetParticlesAndPlay(E, disableafter);
            AudioSource aud = E.GetComponent<AudioSource>();
            if (aud)
                aud.Play();

            return E;
        }

        /// <summary>
        /// Spawns and pools a gameobject with a particle system on a parent transform. The object can have children with more particle systems.
        /// The disbableafter decides if the object should despawn by itself. Pass NULL as parentTransform if the object should not have a parent.
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="ObjName"></param>
        /// <param name="localpos"></param>
        /// <param name="rot"></param>
        public void SpawnParticleSystem(Transform parentTransform, GameObject Obj, Vector3 localpos, Quaternion rot, bool disableafter)
        {
            GameObject E = GetObject(Obj);
            E.transform.localPosition = localpos;
            E.transform.rotation = rot;
            if (parentTransform != null)
                E.transform.SetParent(parentTransform, true);
            if (!E.activeInHierarchy)
                E.SetActive(true);
            ResetParticlesAndPlay(E, disableafter);
            AudioSource aud = E.GetComponent<AudioSource>();
            if (aud)
                aud.Play();
        }

        /// <summary>
        /// Spawns and pools a gameobject on a parent.
        /// Remember to disable the object after it has done it's thing. (Otherwise this manager will treat it as "busy" forever, 
        /// (And will always spawn new instances).
        /// </summary>
        /// <param name="parentTransform"></param>
        /// <param name="ObjName"></param>
        /// <param name="localpos"></param>
        /// <param name="rot"></param>
        public GameObject SpawnObject(Transform parentTransform, GameObject Obj, Vector3 localpos, Quaternion rot, bool persistent = false)
        {
            GameObject E = GetObject(Obj, persistent);
            E.transform.SetParent(parentTransform, true);
            E.transform.localPosition = localpos;
            E.transform.rotation = rot;
            if (!E.activeInHierarchy)
                E.SetActive(true);
            AudioSource aud = E.GetComponent<AudioSource>();
            if (aud)
                aud.Play();
            return E;
        }

        /// <summary>
        /// Spawns and pools a gameobject.
        /// Remember to disable the object after it has done it's thing. (Otherwise this manager will treat it as "busy" forever, 
        /// (And will always spawn new instances).
        /// </summary>
        /// <param name="ObjName"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        public GameObject SpawnObject(GameObject Obj, Vector3 pos, Quaternion rot, bool persistent = false)
        {
            GameObject E = GetObject(Obj, persistent);
            E.transform.position = pos;
            E.transform.rotation = rot;
            if (!E.activeInHierarchy)
                E.SetActive(true);
            E.transform.parent = transform;
            AudioSource aud = E.GetComponent<AudioSource>();
            if (aud)
                aud.Play();
            return E;
        }

        /// <summary>
        /// Spawns and pools a gameobject.
        /// Remember to disable the object after it has done it's thing. (Otherwise this manager will treat it as "busy" forever, 
        /// (And will always spawn new instances).
        /// </summary>
        /// <param name="ObjName"></param>
        public GameObject SpawnObject(GameObject Obj, bool persistent = false)
        {
            GameObject E = GetObject(Obj, persistent);
            if (!E.activeInHierarchy)
                E.SetActive(true);
            AudioSource aud = E.GetComponent<AudioSource>();
            E.transform.parent = transform;
            if (aud)
                aud.Play();
            return E;
        }

        /// <summary>
        /// Spawns and pools a gameobject with sound.
        /// The object will despawn by itself when the sound is finished playing.
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="ObjName"></param>
        /// <param name="localpos"></param>
        /// <param name="rot"></param>
        public void SpawnSoundObject(GameObject Obj, Vector3 pos)
        {
            GameObject E = GetObject(Obj);
            E.transform.position = pos;
            if (!E.activeInHierarchy)
                E.SetActive(true);
            AudioSource aud = E.GetComponent<AudioSource>();
            if (aud)
                aud.Play();
            E.transform.parent = transform;
            StartCoroutine(DelayedDespawn(E, aud.clip.length));
        }

        GameObject GetObject(GameObject type, bool persistent = false)
        {
            //Examine if there are any available objects in the pool.
            for (int i = 0; i < ReusableObjects.Count; i++)
            {
                if (type.name == Objecttypes[i].name)
                {
                    for (int o = 0; o < ReusableObjects[i].Count; o++)
                    {
                        if (!ReusableObjects[i][o].activeInHierarchy)
                            return ReusableObjects[i][o];
                    }
                    //No available objects, then we instantiate a new one and add it to the pool.
                    GameObject O = Instantiate(Objecttypes[i]) as GameObject;
                    //Add it to the pool if it's not persistent, (can be used again).
                    if(persistent == false)
                    {
                        ReusableObjects[i].Add(O);
                    }
                    return O;
                }
            }
            //If the type has not been pooled yet we add it
            Objecttypes.Add(type);
            ReusableObjects.Add(new List<GameObject>());
            GameObject Ob = Instantiate(Objecttypes[Objecttypes.Count - 1]) as GameObject;
            ReusableObjects[ReusableObjects.Count - 1].Add(Ob);
            return Ob;
        }

        void ResetParticlesAndPlay(GameObject go, bool disableafter)
        {
            //"Rewind" all particle systems on the object and play them from the start.
            ParticleSystem mainps = go.GetComponent<ParticleSystem>();
            ParticleSystem[] ps = go.GetComponentsInChildren<ParticleSystem>();
            float highestDuration = 0.0f;
            if (mainps)
            {
                mainps.Clear();
                mainps.Play();
                if (mainps.main.duration > highestDuration)
                    highestDuration = mainps.main.duration;

                AudioSource auds = mainps.GetComponent<AudioSource>();
                if (auds)
                {
                    if (auds.clip.length > highestDuration)
                        highestDuration = auds.clip.length;
                }
            }
            if (ps.Length > 0)
            {
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i].Clear();
                    ps[i].Play();
                    if (ps[i].main.duration > highestDuration)
                        highestDuration = ps[i].main.duration;

                    AudioSource auds = ps[i].GetComponent<AudioSource>();
                    if (auds)
                    {
                        if (auds.clip.length > highestDuration)
                            highestDuration = auds.clip.length;
                    }
                }
            }

            //When finished playing, disable it and send it back to the pool.
            if (disableafter)
                StartCoroutine(DelayedDespawn(go, highestDuration));
        }

        IEnumerator DelayedDespawn(GameObject obj, float delay)
        {
            float timer = 0.1f;
            while (timer < delay)
            {
                timer += Time.deltaTime;
                yield return false;
            }
            obj.SetActive(false);
        }
    }
}