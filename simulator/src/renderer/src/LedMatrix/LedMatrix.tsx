import './LedMatrix.css';
import { Dimensions, Frame } from '@renderer/domain';

interface LedMatrixProperties {
  dimensions: Dimensions;
  pitch: number;
  brightness: number;
  frame: Frame;
}

export default function LedMatrix(props: LedMatrixProperties) {
  return (
    <div
      id="pxlPanel"
      className="pxlPanel"
      style={{
        gridTemplateColumns: `repeat(${props.dimensions.w}, 1fr)`,
        gridTemplateRows: `repeat(${props.dimensions.h}, 1fr)`,
        width: `${props.dimensions.w * props.pitch}px`,
        height: `${props.dimensions.h * props.pitch}px`,
      }}
    >
      {props.frame.data.map((color, idx) => (
        <div
          className="led"
          id={`led_${idx}`}
          key={idx}
          style={{ backgroundColor: `${color}` }}
        />
      ))}
    </div>
  );
}
