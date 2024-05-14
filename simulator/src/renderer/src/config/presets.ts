import { Preset } from '@renderer/domain';

export const presets: Preset[] = [
  { name: 'Default 24 x 24', dimensions: { w: 24, h: 24 }, pitch: 30 },
  { name: 'Mini 16 x 16', dimensions: { w: 16, h: 16 }, pitch: 30 },
  { name: 'Large 32 x 32', dimensions: { w: 32, h: 32 }, pitch: 30 },
  { name: 'X-Large 64 x 64', dimensions: { w: 64, h: 64 }, pitch: 18 },
];
