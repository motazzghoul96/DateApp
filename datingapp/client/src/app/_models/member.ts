import { Photo } from "./photo";


    export interface Member {
        userName: string;
        gender: string;
        dateOfBirth: Date;
        photoUrl:string;
        knownAs: string;
        created: Date;
        age:number;
        lastActive: Date;
        introduction: string;
        lookingFor: string;
        instrests: string;
        city: string;
        country: string;
        photos: Photo[];
    }



