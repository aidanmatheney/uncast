import React, { FunctionComponent, useRef, useState, useEffect } from 'react';

import AudioPlayer from 'react-h5-audio-player';
import 'react-h5-audio-player/lib/styles.css';

const Player: FunctionComponent<{
  media?: {
    url: string;
    startTimeS?: number;
  }

  onTimeChanged?(timeS: number): void;
}> = ({
  media,
  onTimeChanged
}) => {
  const playerRef = useRef<AudioPlayer>(null);
  const player = playerRef.current;
  const audio = player?.audio.current;

  const [loadedStartTime, setLoadedStartTime] = useState(false);
  useEffect(() => {
    if (media != null) {
      setLoadedStartTime(false);
    }
  }, [media])

  return (
    <div>
      <AudioPlayer
        ref={playerRef}
        src={media?.url}

        onListen={() => onTimeChanged?.(audio!.currentTime)}

        onCanPlay={() => {
          if (media?.startTimeS != null && !loadedStartTime) {
            audio!.currentTime = media.startTimeS;
            audio!.play();
            setLoadedStartTime(true);
          }
        }}
      />
    </div>
  )
};
export default Player;
