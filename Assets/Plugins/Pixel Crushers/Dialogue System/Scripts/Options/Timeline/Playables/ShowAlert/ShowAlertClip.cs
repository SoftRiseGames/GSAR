// Recompile at 8.10.2023 13:04:07
#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{
    [Serializable]
    public class ShowAlertClip : PlayableAsset, ITimelineClipAsset
    {
        public ShowAlertBehaviour template = new ShowAlertBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ShowAlertBehaviour>.Create(graph, template);
            return playable;
        }
    }
}
#endif
#endif
