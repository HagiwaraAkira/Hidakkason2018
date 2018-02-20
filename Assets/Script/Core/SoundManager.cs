using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundVolume{
    public float Bgm   = 1.0f;
    public float Se    = 1.0f;

    public bool  Mute  = false;

    public void Reset(){
        Bgm  = 1.0f;
        Se   = 1.0f;
        Mute = false;
    }
}

public class SoundManager : SingletonMonoBehaviour< SoundManager > {
    public SoundVolume Volume = new SoundVolume();

    private AudioClip[] _seClips;
    private AudioClip[] _bgmClips;

    private Dictionary<string,int> seIndexes = new Dictionary<string,int>();
    private Dictionary<string,int> bgmIndexes = new Dictionary<string,int>();

    const int CNumChannel = 16;
    private AudioSource _bgmSource;
    private AudioSource[] seSources = new AudioSource[CNumChannel];

    Queue<int> seRequestQueue = new Queue< int >();

    //------------------------------------------------------------------------------
    void Awake () {
        if( this != Instance )
        {
            Destroy(this);
            return;
        }

        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;

        for(int i = 0 ; i < seSources.Length ; i++ ){
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        _seClips  = Resources.LoadAll<AudioClip>("Audio/SE");
        _bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

        for( int i = 0; i < _seClips.Length; ++i )
        {
            seIndexes[_seClips[i].name] = i;
        }

        for( int i = 0; i < _bgmClips.Length; ++i )
        {
            bgmIndexes[_bgmClips[i].name] = i;
        }

        /* Debug.Log("se ========================"); */
        /* foreach(var ac in seClips ) { Debug.Log( ac.name ); } */
        /* Debug.Log("bgm ========================"); */
        /* foreach(var ac in bgmClips ) { Debug.Log( ac.name ); } */
    }

    //------------------------------------------------------------------------------
    void Update()
    {
        _bgmSource.mute = Volume.Mute;
        foreach(var source in seSources ){
            source.mute = Volume.Mute;
        }

        _bgmSource.volume = Volume.Bgm;
        foreach(var source in seSources ){
            source.volume = Volume.Se;
        }

        int count = seRequestQueue.Count;
        if( count != 0 )
        {
            int sound_index = seRequestQueue.Dequeue();
            playSeImpl( sound_index );
        }
    }

    //------------------------------------------------------------------------------
    private void playSeImpl( int index )
    {
        if( 0 > index || _seClips.Length <= index ){
            return;
        }

        foreach(AudioSource source in seSources){
            if( false == source.isPlaying ){
                source.clip = _seClips[index];
                source.Play();
                return;
            }
        }  
    }

    //------------------------------------------------------------------------------
    public int GetSeIndex( string name )
    {
        return seIndexes[name];
    }

    //------------------------------------------------------------------------------
    public int GetBgmIndex( string name )
    {
        return bgmIndexes[name];
    }

    //------------------------------------------------------------------------------
    public void PlayBgm( string name ){
        int index = bgmIndexes[name];
        PlayBgm( index );
    }

    //------------------------------------------------------------------------------
    public void PlayBgm( int index ){
        if( 0 > index || _bgmClips.Length <= index ){
            return;
        }

        if( _bgmSource.clip == _bgmClips[index] ){
            return;
        }

        _bgmSource.Stop();
        _bgmSource.clip = _bgmClips[index];
        _bgmSource.Play();
    }

    //------------------------------------------------------------------------------
    public void StopBgm(){
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    //------------------------------------------------------------------------------
    public void PlaySe( string name )
    {
        PlaySe( GetSeIndex( name ) );
    }

    //一旦queueに溜め込んで重複を回避しているので
    //再生が1frame遅れる時がある
    //------------------------------------------------------------------------------
    public void PlaySe( int index )
    {
        if( !seRequestQueue.Contains( index ) )
        {
            seRequestQueue.Enqueue( index );
        }
    }

    //------------------------------------------------------------------------------
    public void StopSe(){
        ClearAllSeRequest();
        foreach(AudioSource source in seSources){
            source.Stop();
            source.clip = null;
        }  
    }

    //------------------------------------------------------------------------------
    public void ClearAllSeRequest()
    {
        seRequestQueue.Clear();
    }

}
