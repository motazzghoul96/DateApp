import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/Pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';



@Injectable({
  providedIn: 'root'
})
export class MemberService {
baseUrl=environment.apiUrl;
members:Member[]=[];
memberCach=new Map();
user:User;
userParams:UserParams;

  constructor(private http:HttpClient,private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams=new UserParams(user);
    })
   }

   getUserParams(){
     return this.userParams
   }
   setUserParams(params:UserParams){
    this.userParams=params;
   }
   
   resetUserParams(){
      this.userParams=new UserParams(this.user);
      return this.userParams
   }

  getMembers(userparams:UserParams){
    
    const resopnse=this.memberCach.get(Object.values(userparams).join('-'));
    if (resopnse) {
      return of(resopnse)
    }
     let params=this.paginationHeader(userparams.pageNumber,userparams.pageSize)

     params=params.append("maxAge",userparams.maxAge.toString());
     params=params.append("minAge",userparams.minAge.toString());
     params=params.append("gender",userparams.gender);
     params=params.append("orderBy",userparams.orderBy);

        return this.getPaginationResult<Member[]>(this.baseUrl+'user',params)
        .pipe(map(resopnse=>{
          this.memberCach.set(Object.values(userparams).join('-'),resopnse);
          return resopnse;
        }))
  }

 
  getMember(username){
    const member=[...this.memberCach.values()]
    .reduce((arr,elm)=>arr.concat(elm.result),[])
    .find((member:Member)=>member.userName===username)

    if(member){
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl+'user/'+username)
  }


  updateMember(member:Member){
    return this.http.put(this.baseUrl+'user',member).pipe(
      map(()=>{
        const index=this.members.indexOf(member);
        this.members[index]=member;
      })
     );
    }


    setMainPhoto(photoId:number){
      return this.http.put(this.baseUrl+'user/set-main-photo/'+photoId,{});
    }


    deletePhoto(photoId:number){
      return this.http.delete(this.baseUrl+"user/delete-photo/"+photoId)
    }

    private getPaginationResult<T>(url,params) {
      const  paginatedResult:PaginatedResult<T>=new PaginatedResult<T>()
      return this.http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
    }
  
    private paginationHeader(pageNumber:number,pageSize:number){
      let params=new HttpParams();
        params=params.append("pageNumber",pageNumber.toString())
        params=params.append("pageSize",pageSize.toString())
      return params;
    }
  }


