import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery/lib/ngx-gallery-image';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery/lib/ngx-gallery-options';
import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
member:Member;
galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[];
  constructor(private memberServices:MemberService,private route:ActivatedRoute) { }


  ngOnInit(): void {
    this.loadMember();
    this.galleryOptions=[
      {
        width:'500px',
        height:'400px',
        imagePercent:100,
        thumbnailsColumns:4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview:false
      }
    ]
    
  }
  getImages():NgxGalleryImage[]{
    const imageUrls=[];
    for(const photo of this.member.photos){
      imageUrls.push({
        small:photo?.url,
        medium:photo?.url,
        big:photo?.url
      });
     
    }
    return imageUrls;
  }
loadMember(){
  this.memberServices.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member=>{
    this.member=member;
    this.galleryImages=this.getImages();
  })
}
}
