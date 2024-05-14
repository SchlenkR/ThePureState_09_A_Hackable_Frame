export type Dimensions = {
  w: number;
  h: number;
};

export type Preset = {
  name: string;
  dimensions: Dimensions;
  pitch: number;
};

export type Frame = {
  number: number;
  data: string[];
};

export type SetMatrixProperties = (values: {
  dimensions?: Dimensions;
  pitch?: number;
  brightness?: number;
}) => void;
