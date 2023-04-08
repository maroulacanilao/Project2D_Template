using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] private TestObjectPool TestObject;
    [SerializeField] private GameObject anotherTestObject;
    private IEnumerator Start()
    {
        var test = TestObject.gameObject.Reuse();
        var anotherTest = anotherTestObject.Reuse();
        yield return new WaitForSeconds(3f);
        test.Release();
        anotherTest.Release();
    }
}
