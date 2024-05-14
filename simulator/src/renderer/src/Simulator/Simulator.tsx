import './Simulator.css';
import { useCallback, useEffect, useState } from 'react';
import { Frame, SetMatrixProperties } from '../domain';
import { presets } from '@renderer/config/presets';
import LedMatrix from '@renderer/LedMatrix/LedMatrix';
import ControlPanel from '@renderer/ControlPanel/ControlPanel';

const DEFAULT = presets[0];

export const useMatrixProperties = () => {
  const [size, setDimensions] = useState(DEFAULT.dimensions);
  const [pitch, setPitch] = useState(DEFAULT.pitch);
  const [brightness, setBrightness] = useState(100);

  const setMatrixProperties: SetMatrixProperties = ({
    dimensions,
    pitch,
    brightness,
  }) => {
    if (dimensions) {
      setDimensions(dimensions);
    }

    if (pitch !== undefined) {
      setPitch(pitch);
    }

    if (brightness !== undefined) {
      setBrightness(brightness);
    }
  };

  useEffect(() => {
    console.log(
      'Matrix properties updated:',
      'dimensions:',
      size,
      'pitch',
      pitch,
      'brightness',
      brightness,
    );
  }, [size, pitch, brightness]);

  return {
    size: size,
    setDimensions,
    pitch,
    setPitch,
    brightness,
    setBrightness,

    setMatrixProperties,

    // normally one would load presets from JSON or some other source
    // and also provide a way to update them
    presets,
  };
};

function Simulator() {
  const [lastError, setLastError] = useState<string | undefined>(undefined);
  const matrix = useMatrixProperties();
  const [frame, setFrame] = useState<Frame>({
    number: 0,
    data: Array.from({ length: matrix.size.w * matrix.size.h }).map(
      () => '#000',
    ),
  });

  const applyBrightness = useCallback(
    (color: string) => {
      const r = parseInt(color.slice(1, 3), 16);
      const g = parseInt(color.slice(3, 5), 16);
      const b = parseInt(color.slice(5, 7), 16);

      const newR = Math.floor((r * matrix.brightness) / 100);
      const newG = Math.floor((g * matrix.brightness) / 100);
      const newB = Math.floor((b * matrix.brightness) / 100);

      const th = (value: number) => value.toString(16).padStart(2, '0');

      return `#${th(newR)}${th(newG)}${th(newB)}`;
    },
    [matrix.brightness],
  );

  useEffect(() => {
    let frameCount = 0;

    const expectedLength = matrix.size.w * matrix.size.h;
    window.api.onFrameReceived((data) => {
      frameCount += 1;

      const [currError, newData] =
        data.length !== expectedLength
          ? [
              `Invalid frame received: expected ${expectedLength} pixels, got ${data.length}`,
              frame.data,
            ]
          : [undefined, data.map(applyBrightness)];

      setLastError(currError);
      setFrame({ number: frameCount, data: newData });
    });

    return () => {
      window.api.offFrameReceived();
    };
  }, [matrix.size, applyBrightness]);

  return (
    <>
      <h1>PXL Remote Display</h1>
      <div className="main">
        <LedMatrix
          dimensions={matrix.size}
          pitch={matrix.pitch}
          brightness={matrix.brightness}
          frame={frame}
        />
        <ControlPanel
          dimensions={matrix.size}
          pitch={matrix.pitch}
          brightness={matrix.brightness}
          presets={presets}
          frameNumber={frame.number}
          setMatrixProperties={matrix.setMatrixProperties}
        />
        {lastError && <p className="error">{lastError}</p>}
      </div>
    </>
  );
}

export default Simulator;
