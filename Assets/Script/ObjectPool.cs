using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PoolContent content = default;
    Queue<PoolContent> objQueue;
    [SerializeField] int maxObjs = 200;

    // Start is called before the first frame update
    void Start()
    {
        objQueue = new Queue<PoolContent>(maxObjs);
        //poolçÏê¨
        for(int i= 0; i < maxObjs; i++)
        {
            var tmpObj = Instantiate(content);
            tmpObj.transform.parent = transform;
            tmpObj.transform.localPosition = new Vector3(100, 0, 100);
            objQueue.Enqueue(tmpObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PoolContent Launch(Vector3 _position,float _angle)
    {
        if(objQueue.Count <= 0)
        {
            return null;
        }
        var tmpObj = objQueue.Dequeue();
        tmpObj.gameObject.SetActive(true);
        tmpObj.ShowInStage(_position,_angle);
        return tmpObj;
    }

    public void Collect(PoolContent _obj)
    {
        _obj.gameObject.SetActive(false);
        objQueue.Enqueue(_obj);
    }

    public void ResetAll()
    {
        BroadcastMessage("HideFromStage", SendMessageOptions.DontRequireReceiver);
    }

}
