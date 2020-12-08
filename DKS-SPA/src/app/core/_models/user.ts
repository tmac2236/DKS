import { Photo } from './photo';
export interface User {
  id: number;
  account: string;
  email: string;
  age: number;
  lastActive: Date;
  city: string;
  country: string;
  photoUrl: string;

  interest?: string;
  introduction?: string;
  lookingFor?: string;
  photos?: Photo[];
}
