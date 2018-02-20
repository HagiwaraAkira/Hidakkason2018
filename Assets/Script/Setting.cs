using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Setting :SingletonMonoBehaviour<Setting>
{
    public int EnemyMode = 0;
    public int VisibleMode = 0;
    public bool SameMode;
    public bool PlusMode;
    public bool ReverseMode;
    public int Win;
    public int Lose;
    public int Draw;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

    }

    public void SetEnemyMode(int value)
    {
        EnemyMode = value;
    }
    
        public void SetVisibleMode(int value)
        {
            VisibleMode = value;
        }
    
        public void Same(bool value)
        {
        SameMode = value;
    }
            public void Plus(bool value)
    {
        PlusMode = value;
    }
            public void Reverse(bool value)
    {
        ReverseMode = value;
    }
}
