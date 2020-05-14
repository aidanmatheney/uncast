import React, { FunctionComponent, useRef, useState } from 'react';

import AudioPlayer from 'react-h5-audio-player';
import 'react-h5-audio-player/lib/styles.css';

import { EpisodeAudio } from '../podcast/EpisodeCard'

const Player: FunctionComponent<{
  audioUrl: string;

  startTime?: number;
  onTimeChanged?(time: number): void;
}> = ({
  audioUrl,

  startTime,
  onTimeChanged
}) => {
  const playerRef = useRef<AudioPlayer>(null);
  const player = playerRef.current;
  const audio = player?.audio.current;

  const [loadedStartTime, setLoadedStartTime] = useState(false);

  return (
    <div>
      <AudioPlayer
        ref={playerRef}
        src={audioUrl}

        onListen={() => onTimeChanged?.(audio!.currentTime)}

        onCanPlay={() => {
          if (startTime != null && !loadedStartTime) {
            audio!.currentTime = startTime;
            setLoadedStartTime(true);
          }
        }}
      />
    </div>
  )
};
export default Player;
