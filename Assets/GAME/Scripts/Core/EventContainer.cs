using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EventContainer : IDisposable
{
    public Queue<eCollectable> OnCollectResource;
    
    public EventContainer()
    {
        
    }
    
    public void Dispose()
    {
        
    }
}
