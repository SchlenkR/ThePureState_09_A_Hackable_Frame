import './ControlPanel.css';
import { Dimensions, Preset, SetMatrixProperties } from '@renderer/domain';

type ControlPanelProperties = {
  dimensions: Dimensions;
  pitch: number;
  brightness: number;
  presets: Preset[];
  frameNumber: number;
  setMatrixProperties: SetMatrixProperties;
};

export default function ControlPanel(props: ControlPanelProperties) {
  return (
    <div className="controlPanel">
      <div className="control">
        <label htmlFor="brightness">Brightness: </label>
        <input
          type="range"
          id="brightness"
          name="brightness"
          min="0"
          max="100"
          onChange={(e) =>
            props.setMatrixProperties({ brightness: Number(e.target.value) })
          }
          value={String(props.brightness)}
        ></input>
      </div>
      <div className="control">
        <label htmlFor="preset">Preset: </label>
        <select
          id="preset"
          name="preset"
          onChange={(event) =>
            props.setMatrixProperties(props.presets[Number(event.target.value)])
          }
        >
          {props.presets.map((preset, idx) => (
            <option key={idx} value={idx}>
              {preset.name}
            </option>
          ))}
        </select>
      </div>
      <div className="control">
        <label htmlFor="width">Width: </label>
        <input
          type="number"
          id="width"
          name="width"
          value={String(props.dimensions.w)}
          onChange={(e) =>
            props.setMatrixProperties({
              dimensions: { w: Number(e.target.value), h: props.dimensions.h },
            })
          }
        />
      </div>
      <div className="control">
        <label htmlFor="height">Height: </label>
        <input
          type="number"
          id="height"
          name="height"
          value={String(props.dimensions.h)}
          onChange={(e) =>
            props.setMatrixProperties({
              dimensions: { w: props.dimensions.w, h: Number(e.target.value) },
            })
          }
        />
      </div>
      <div className="control">
        <label htmlFor="pitch">Pitch: </label>
        <input
          type="number"
          id="pitch"
          name="pitch"
          value={String(props.pitch)}
          onChange={(e) =>
            props.setMatrixProperties({ pitch: Number(e.target.value) })
          }
        />
      </div>
      <hr />
      <div className="control">
        <label htmlFor="frameCount">Frame no.: </label>
        <span>{props.frameNumber}</span>
      </div>
    </div>
  );
}
