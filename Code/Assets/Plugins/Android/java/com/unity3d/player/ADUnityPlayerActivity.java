package com.unity3d.player;

import android.os.Bundle;

public class ADUnityPlayerActivity extends UnityPlayerActivity
{
    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        super.onDestroy();
    }

}
